using UnityEngine;

public class Card
{
    public Vector3Int position;
    public Type type;
    public int front;
    public bool revealed;
    public bool matched;

    public enum Type
    {
        Invalid,
        Front,
    }
}
