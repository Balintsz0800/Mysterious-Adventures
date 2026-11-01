using UnityEngine;

[CreateAssetMenu(menuName = "Inventory/Tools/Shovel")]
public class Shovel : Tool
{
    public override void Use()
    {
        TileManager.Instance.ModifyTile(TileManager.Instance.GetMouseCellPosition(), TileType.Dirt);
    }
}