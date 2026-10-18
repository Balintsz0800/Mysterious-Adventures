using UnityEngine;
using UnityEngine.Tilemaps;

[CreateAssetMenu(fileName =  "CustomTile", menuName = "2D/Tiles/CustomTile")]
public class CustomTile : Tile
{
    public GroundType.groundType groundType;
}
