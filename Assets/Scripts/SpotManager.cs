using UnityEngine;

public class SpotManager : MonoBehaviour
{
    public Spot[] spots;
   
    public Spot GetFirstFreeSpot()
    {
        foreach (Spot s in spots)
        {
            if (!s.isOccupied)
                return s;
        }
        return null; // no free spots
    }   
}
