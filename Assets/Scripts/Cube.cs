using System.Collections;
using UnityEngine;

public class Cube : MonoBehaviour
{

    
    private GridManager gridManager;
    public int gridX;
    public int gridY;

    [HideInInspector] public Colorize colorize;
    [SerializeField] private Animator cubeAnimator;

    // private Renderer rend;

    public bool isTraget = false;

 
    private void Awake()
    {
        colorize = GetComponent<Colorize>();
        // rend = GetComponentInChildren<Renderer>();
        cubeAnimator = GetComponent<Animator>();

    }

    public void SetGridManager(GridManager manager, int x, int y)
    {
        gridManager = manager;
        gridX = x;
        gridY = y;
    }

    public void Hit()
    {     
        //destroy only if it's in the first row
        gridManager.CubeDestroyed(gridX, gridY);
        PlayDestroyHit(); 
    }



    public void PlayJustHit()
    {
        if (cubeAnimator != null)
        {
            cubeAnimator.CrossFade("JustHit", 0f);
        }

    }
    //animation event HitDestroy
    private void PlayDestroyHit()
    {      
        cubeAnimator.CrossFade("HitDestroy", 0f);  
        StartCoroutine(DestroyCube());
    }

    public IEnumerator DestroyCube()
    {
        yield return new WaitForSeconds(0.15f);
        
        Destroy(gameObject);
    }
}

