using UnityEngine;

public class SpawnManager : MonoBehaviour
{
    public GameObject shooterPrefab;
    public ShooterPattern shooterPattern;

    public SpotManager spotManager;
    
    public SpawnPoint[] spawnCoulumns01;
    public SpawnPoint[] spawnCoulumns02;
    public SpawnPoint[] spawnCoulumns03;

    private void Start()
    {
        SpawnShooters();
    }

    
    public void SpawnShooters()
    {
        SpawnShooterColumn(spawnCoulumns01, shooterPattern.column01);
        SpawnShooterColumn(spawnCoulumns02, shooterPattern.column02);
        SpawnShooterColumn(spawnCoulumns03, shooterPattern.column03);
    }


    private void SpawnShooterColumn(SpawnPoint[] column, ShooterEntry[] pattern)
    {
        for (int i = 0; i < column.Length; i++)
        {
            GameObject shooter = Instantiate(shooterPrefab, column[i].transform.position, Quaternion.identity);
            Shooter shooterScript = shooter.GetComponent<Shooter>();

 
            if (shooterScript != null)
            {
                shooterScript.SetColor(pattern[i].color);
                shooterScript.numberOfBullets = pattern[i].bullets;  
            }


            

            //set interaction only for the first row
            SpawnPoint spInteraction = column[i].GetComponent<SpawnPoint>();
            if (spInteraction != null)
            {
                spInteraction.shooterAtPoint = shooterScript;
                spInteraction.isClickable = (i == 0); // Only first one is clickable

                spInteraction.spotManager = spotManager;

                spInteraction.myColumn = column;
                spInteraction.myRow = i;
            }

        }
    }
}
