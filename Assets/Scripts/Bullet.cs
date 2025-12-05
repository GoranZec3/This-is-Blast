using UnityEngine;

public class Bullet : MonoBehaviour
{
    public float speed = 20f;
    private Cube targetCube;

    void Update()
    {
        if (targetCube == null)
        {
            // Destroy(gameObject);
            BulletPool.Instance.ReturnBullet(this);
            return;
        }

        Vector3 direction = (targetCube.transform.position - transform.position).normalized;
        float distanceThisFrame = speed * Time.deltaTime;

        // Raycast along the movement to detect any cubes in the path
        if (Physics.Raycast(transform.position, direction, out RaycastHit hit, distanceThisFrame))
        {
            Cube cube = hit.collider.GetComponent<Cube>();
            if (cube != null)
            {
                if (cube == targetCube)
                {
                    cube.Hit();
                    // Destroy(gameObject);
                    BulletPool.Instance.ReturnBullet(this);
                    return;
                }
                else
                {
                    cube.PlayJustHit(); // cube in the way but not target
                }
            }
        }

        // Distance check for target (prevents skipping)
        if (Vector3.Distance(transform.position, targetCube.transform.position) <= distanceThisFrame)
        {
            targetCube.Hit();
            // Destroy(gameObject);
            BulletPool.Instance.ReturnBullet(this);
            return;
        }

        // Move bullet
        transform.position += direction * distanceThisFrame;
        transform.forward = direction; // face the target
    }

    public void SetTarget(Cube cube)
    {
        targetCube = cube;
        if (targetCube == null)
        {
            Destroy(gameObject);
        }
    }


    void OnDrawGizmos()
    {
        // Draw the raycast direction (only visible in Scene view)
        Gizmos.color = Color.yellow;
        Gizmos.DrawRay(transform.position, transform.forward * 2f);
    }

}
