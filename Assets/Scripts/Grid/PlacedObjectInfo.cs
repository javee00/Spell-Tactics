using System;
using UnityEngine;

[Serializable]
public class PlacedObjectInfo
{
    public Vector2 position;
    public MapLoader.TileType tileType;

    public PlacedObjectInfo(Vector2 pos, MapLoader.TileType type)
    {
        position = pos;
        tileType = type;
    }
}