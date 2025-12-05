using System;
using System.Collections;
using TMPro;
using UnityEngine;

[RequireComponent(typeof(Colorize))]
public class Shooter : MonoBehaviour
{   
    // [SerializeField] private GameObject bulletPrefab;
    private Colorize shooterColor;

    public GridManager gridManager;

    public float shootInterval = 1f; // seconds between shots
    private float shootTimer = 0f;
    // movement
    [SerializeField] private float moveSpeed = 10f;
    private Coroutine moveRoutine = null;

    private Renderer rend;
    public Spot shooterSpot;
    private bool hasLeftSpot = false;
    public bool isOnSpot = false;
    

    public int numberOfBullets;
    [SerializeField] private TextMeshProUGUI bulletCountText;


    void Awake()
    {
        gridManager = FindFirstObjectByType<GridManager>();
        shooterColor = GetComponent<Colorize>();
        rend = GetComponentInChildren<Renderer>();


    }

    private void Start()
    {
        if (bulletCountText != null)
            bulletCountText.text = numberOfBullets.ToString();
        else
            Debug.LogWarning($"{name}: bulletCountText not assigned in Inspector.");
        
    }


    void Update()
    {
        // Only shoot if this shooter is actually on a spot (shooterSpot is assigned)
        if (shooterSpot != null && isOnSpot )
        {
            if (numberOfBullets <= 0 && !hasLeftSpot)
            {
                shooterSpot.isOccupied = false;
                hasLeftSpot = true;

                if (shooterSpot.spotIndex <= 3)
                {
                    MoveOutAndDestroy(gridManager.outLeftSpot.position);
                    
                }
                else
                {
                    MoveOutAndDestroy(gridManager.outRightSpot.position);           
                }

                return;
            }

            // SHOOTING
            shootTimer += Time.deltaTime;

            if (shootTimer >= shootInterval)
            {
                ShootAtNextCube();
                shootTimer = 0f;
            }

        } 
    }


    void ShootAtNextCube()
    {
        if (numberOfBullets <= 0) return;
        if (gridManager == null) return;
        if (BulletPool.Instance == null)
        {
            Debug.LogWarning("BulletPool instance missing!");
            return;
        }

        //sending info to lock target
        Cube target = gridManager.GetNextCube(shooterColor.color, this);

        if (target != null)
        {
            numberOfBullets--;
            if (bulletCountText != null)
                bulletCountText.text = numberOfBullets.ToString();

            // Rotate shooter to face target (Y-axis only)
            Vector3 direction = target.transform.position - transform.position;
            direction.y = 0f;
            if (direction.sqrMagnitude > 0.0001f)
            {
                transform.rotation = Quaternion.LookRotation(direction);
            }

            Vector3 spawnPos = transform.position + Vector3.up * 0.5f;

            // GameObject bullet = Instantiate(bulletPrefab, spawnPos, transform.rotation);
            // var bulletComp = bullet.GetComponent<Bullet>();
            // if (bulletComp != null)
            // {          
            //     bulletComp.SetTarget(target);
            // }
            Bullet bullet = BulletPool.Instance.GetBullet();
            bullet.transform.position = spawnPos;
            bullet.transform.rotation = transform.rotation;
            bullet.SetTarget(target);

        }
    }




    public void MoveToSpot(Vector3 newPosition, bool isMovingToShootSpot = false)
    {
        if (moveRoutine != null)
            StopCoroutine(moveRoutine);

        moveRoutine = StartCoroutine(MoveToPosition(newPosition, isMovingToShootSpot));
    }

    public void MoveOutAndDestroy(Vector3 target)
    {
        if (moveRoutine != null) StopCoroutine(moveRoutine);
        moveRoutine = StartCoroutine(MoveOutPosition(target));

    }

    private IEnumerator MoveToPosition(Vector3 target, bool isMovingToShootSpot = false)
    {
        while (Vector3.Distance(transform.position, target) > 0.01f)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                target,
                moveSpeed * Time.deltaTime
            );

            yield return null;
        }

        transform.position = target; // snap EXACTLY at the end
        moveRoutine = null;
        
        // Only set isOnSpot to true if this is a movement to a board Spot
        isOnSpot = isMovingToShootSpot;

        if (bulletCountText != null)
            bulletCountText.text = numberOfBullets.ToString();           
    }

    private IEnumerator MoveOutPosition(Vector3 target)
    {
        while (Vector3.Distance(transform.position, target) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, target, moveSpeed * Time.deltaTime);
            yield return null;
        }

        Destroy(gameObject); // ← clean destroy when reached exit
    }



    public void SetColor(BeColor beColor)
    {
        shooterColor.color = beColor;
        rend.material.color = shooterColor.GetColorFromEnum(beColor);
    }

}
