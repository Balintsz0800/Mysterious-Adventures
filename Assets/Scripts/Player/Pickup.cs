using TMPro;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    [SerializeField] private float pickupRadius = 1f;
    [SerializeField] private LayerMask itemLayer;
    [SerializeField] private TMP_Text pickupText;

    private PlayerMovement playerMovement;
    private WorldItem previousItem;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, pickupRadius, itemLayer);

        WorldItem itemNear = null;

        if (hit != null)
        {
            itemNear = hit.GetComponent<WorldItem>();
        }

        if (previousItem != null && previousItem != itemNear)
        {
            previousItem.RemoveOutline();
        }

        if (itemNear != null)
        {
            itemNear.OutlineItem();

            if (Input.GetKeyDown(KeyCode.E))
            {
                itemNear.Pickup();
                itemNear = null;
            }
        }

        previousItem = itemNear;
    }
}
