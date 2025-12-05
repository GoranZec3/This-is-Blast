using UnityEngine;
using UnityEngine.EventSystems;

public class SpawnPoint : MonoBehaviour, IPointerClickHandler
{
    public Shooter shooterAtPoint;
    public bool isClickable = false;
    public SpotManager spotManager;

    public SpawnPoint[] myColumn;
    public int myRow;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!isClickable) return;

        Spot freeSpot = spotManager.GetFirstFreeSpot();

        
        if (freeSpot != null)
        {
            // move shooter to board Spot (pass true to set isOnSpot)
            shooterAtPoint.MoveToSpot(freeSpot.transform.position, true);

            
            // mark as occupied
            freeSpot.isOccupied = true;
            shooterAtPoint.shooterSpot = freeSpot;
            

            ShiftColumnForward();
        }
        else
        {
            Debug.Log("No free spots available!");
        }


    }



    void ShiftColumnForward()
    {
        // Clear all shooter references in this column
        Shooter[] tempShooters = new Shooter[myColumn.Length];
        
        // Store references temporarily
        for (int row = 0; row < myColumn.Length; row++)
        {
            tempShooters[row] = myColumn[row].shooterAtPoint;
            myColumn[row].shooterAtPoint = null; // Clear all references first
        }

        // Move shooters forward in the temporary array
        for (int row = 1; row < myColumn.Length; row++)
        {
            if (tempShooters[row] != null)
            {
                // Clear the shooterSpot reference when moving in the column
                tempShooters[row].shooterSpot = null;
                // Move shooter to column position (pass false - not a board Spot)
                tempShooters[row].MoveToSpot(myColumn[row - 1].transform.position, false);
                myColumn[row - 1].shooterAtPoint = tempShooters[row];
            }
        }
        // Last spot becomes empty (no shooter moves into it)
        myColumn[myColumn.Length - 1].shooterAtPoint = null;

    }

}
