using System;
using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float pickupDis;
    public LayerMask itemLayer;
    public TMP_Text pickupText;
    
    private PlayerMovement playerMovement;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            Vector2 dir = playerMovement.FacingDir;
            
            RaycastHit2D hit = Physics2D.Raycast(transform.position, dir, pickupDis, itemLayer);

            if (hit.collider != null)
            {
                Debug.Log("hit: " + hit.collider.gameObject.name);
                
                worldItem item =  hit.collider.GetComponent<worldItem>();

                if (item != null)
                {
                    item.Pickup();
                }
            }
        }
    }
    
    private void OnDrawGizmosSelected()
    {
        if (playerMovement == null) return;

        Gizmos.color = Color.green;

        Gizmos.DrawLine(transform.position, transform.position + (Vector3)playerMovement.FacingDir * pickupDis);
    }
}
