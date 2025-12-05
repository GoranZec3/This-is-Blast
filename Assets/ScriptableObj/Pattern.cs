using UnityEngine;


[CreateAssetMenu(menuName = "Pattern")]
public class Pattern : ScriptableObject 
{

    public Row[] rows =  new Row[10];
}

[System.Serializable]
public class Row
{
    public BeColor[] cells = new BeColor[10];
}
