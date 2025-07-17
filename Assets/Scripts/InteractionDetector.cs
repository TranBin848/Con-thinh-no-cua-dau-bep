using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InteractionDetector : MonoBehaviour
{
    private IInteractable interactableInRange = null;
    public GameObject interactionIcon; // Biểu tượng tương tác
    void Start()
    {
        interactionIcon.SetActive(false); // Ẩn biểu tượng tương tác ban đầu
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (context.performed)
        {
            Debug.Log("InteractionDetector.OnInteract called");
            interactableInRange?.Interact(); // Gọi phương thức Interact của đối tượng trong phạm vi nếu 
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.TryGetComponent(out IInteractable interactable) && interactable.canInteract())
        {
            interactableInRange = interactable;
            interactionIcon.SetActive(true); // Hiển thị biểu tượng tương tác khi có đối tượng trong phạm vi
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.TryGetComponent(out IInteractable interactable) && interactable == interactableInRange)
        {
            interactableInRange = null;
            interactionIcon.SetActive(false); // Ẩn biểu tượng tương tác khi không còn đối tượng trong phạm vi
        }
    }
}
