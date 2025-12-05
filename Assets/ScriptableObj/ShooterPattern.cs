using UnityEngine;

[System.Serializable]
public class ShooterEntry
{
    public BeColor color;
    public int bullets = 10;
}

[CreateAssetMenu(fileName = "ShooterPattern", menuName = "Scriptable Objects/ShooterPattern")]
public class ShooterPattern : ScriptableObject
{
    public ShooterEntry[] column01;
    public ShooterEntry[] column02;
    public ShooterEntry[] column03;
}


