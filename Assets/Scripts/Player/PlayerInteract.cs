using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    [SerializeField] private float interactRadius = 1f;
    [SerializeField] private TMP_Text pickupText;

    private Interactable previousInteractable;

    private void Update()
    {
        Collider2D[] hits = Physics2D.OverlapCircleAll(transform.position, interactRadius);

        Interactable interactableNear = null;

        foreach (Collider2D hit in hits)
        {
            if (hit.TryGetComponent(out Interactable interactable))
            {
                interactableNear = interactable;
                break;
            }
        }

        if (previousInteractable != null &&
            previousInteractable != interactableNear)
        {
            previousInteractable.RemoveOutline();
        }

        if (interactableNear != null)
        {
            interactableNear.Outline();

            if (Input.GetKeyDown(KeyCode.E))
            {
                interactableNear.Interact();
            }
        }

        previousInteractable = interactableNear;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position, interactRadius);
    }
}
