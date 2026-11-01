using System;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    private InvManager invManager;
    
    private void Update()
    {
        if (invManager == null)
        {
            invManager = InvManager.instance;
        }
        
        InvItem selectedItem = invManager.GetSelectedItem();
        
        if (Input.GetMouseButtonDown(0) && TileManager.Instance.HoveredTileInRange())
        {
            if (selectedItem != null && selectedItem.item is Tool tool)
            {
                tool.Use();
            }
        }
    }
}