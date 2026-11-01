using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private TMP_Text pickupText;

    private PlayerMovement playerMovement;
    private Interactable previousInteractable;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Update()
    {
        Collider2D hit = Physics2D.OverlapCircle(transform.position, interactRadius);

        Interactable interactableNear = null;

        if (hit != null)
        {
            interactableNear = hit.GetComponent<Interactable>();
        }

        if (previousInteractable != null && previousInteractable != interactableNear)
        {
            previousInteractable.RemoveOutline();
        }

        if (interactableNear != null)
        {
            interactableNear.Outline();

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableNear.Interact();
                interactableNear = null;
            }
        }

        previousInteractable = interactableNear;
    }
}
