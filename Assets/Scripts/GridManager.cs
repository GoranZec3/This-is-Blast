using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;


public class GridManager : MonoBehaviour
{
    [SerializeField] private Grid grid;   
    

    [SerializeField] private GameObject[] coloredCubePrefabs;//color of prefabs must match BeColor enum order
    private GameObject[,] cubeGrid;
    public Pattern pattern;

    [SerializeField] private Slider gameProgress;

    public Transform outLeftSpot;
    public Transform outRightSpot;

    public int width = 10;
    public int height = 10;

    //unique column for shooter
    private Dictionary<Shooter, int> nextColumns = new Dictionary<Shooter, int>();
    private int nextRow = 0; 

    public int currentNumberOfCubes;

    private int nextSceneIndex;

    [SerializeField] private ParticleSystem confetti;
    [SerializeField] private ParticleSystem confettiLong;
    [SerializeField] private GameObject endLevelBg;
    [SerializeField] private TextMeshProUGUI lvlNumber;


    private void Start()
    {
        GeneratePlayground();
        gameProgress.maxValue = currentNumberOfCubes; 
        gameProgress.value = currentNumberOfCubes;

        nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;

    }

    void GeneratePlayground()
    {
        cubeGrid = new GameObject[width, height];
        currentNumberOfCubes = width * height;

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                Vector3Int cellPos = new Vector3Int(x, y, 0);
                Vector3 worldPos = grid.CellToWorld(cellPos);

                // Read color from pattern
                BeColor beColor = pattern.rows[y].cells[x];

                GameObject prefab = coloredCubePrefabs[(int)beColor];

                // Instantiate it
                GameObject cube = Instantiate(prefab, worldPos, Quaternion.identity);
                cubeGrid[x, y] = cube;

                // Set grid info on cube
                Cube cubeComp = cube.GetComponent<Cube>();
                cubeComp.SetGridManager(this, x, y);
              
            }
        }
    }

    public void CubeDestroyed(int x, int y)
    {
        if(cubeGrid[x,y] != null)
        {
            currentNumberOfCubes--;
            gameProgress.value = currentNumberOfCubes;
            if(currentNumberOfCubes < 1)
            {
                EndLevel();
            }

        }
        StartCoroutine(DelayedRowShift(x, y));
    }

    private IEnumerator DelayedRowShift(int x, int y)
    {
        //Immediately mark the position as null to prevent targeting
        cubeGrid[x, y] = null;

        yield return new WaitForSeconds(0.25f);   //delay before row moves


        // Move all cubes above the destroyed one down by 1
        for (int row = y + 1; row < height; row++)
        {
            if (cubeGrid[x, row] != null)
            {
                Vector3 targetPos = grid.CellToWorld(new Vector3Int(x, row - 1, 0));
                cubeGrid[x, row].transform.position = targetPos;

                cubeGrid[x, row].GetComponent<Cube>().SetGridManager(this, x, row - 1);

                cubeGrid[x, row - 1] = cubeGrid[x, row];
                cubeGrid[x, row] = null;
            }
        }
    }
    

    public Cube GetNextCube(BeColor shooterColor, Shooter shooter)
    {
        // Initialize or get column for this shooter
        if (!nextColumns.ContainsKey(shooter))
        {
            nextColumns[shooter] = 0;
        }
        
        int shooterColumn = nextColumns[shooter];
        
        for (int i = 0; i < width; i++)
        {
            int x = (shooterColumn + i) % width;
            int y = nextRow;

            GameObject cubeObj = cubeGrid[x, y];

            if (cubeObj != null)
            {
                Cube cube = cubeObj.GetComponent<Cube>();

                if (cube != null) 
                {
                    if(cube.colorize.color == shooterColor && !cube.isTraget)
                    {
                        cube.isTraget = true;                 
                        nextColumns[shooter] = (x + 1) % width;
                        return cube;
                    }
                }
            }
        }
        
        // If no target found, still advance this shooter's column
        nextColumns[shooter] = (shooterColumn + 1) % width;
        return null;
    }

    public void EndLevel()
    {
        
        confetti.Play();
        confettiLong.Play();
        endLevelBg.gameObject.SetActive(true); 
        lvlNumber.text = nextSceneIndex.ToString();

    }

    public void StartNextLevel()
    {
        

        SceneManager.LoadScene(nextSceneIndex);
    }
}
