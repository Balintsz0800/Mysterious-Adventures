using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName = "TileGroundData", menuName = "2D/Tile Ground Data")]
public class TileGroundData : ScriptableObject
{
    public TileType tileType;
    public List<TileBase> tiles;
}
