using System;
using UnityEngine;

public class PlayerTool : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (InvManager.instance.GetSelectedItem().item is Tool tool)
            {
                tool.Use();
            }
        }
    }
}