using UnityEngine;

public class Door : MonoBehaviour, IInteractable
{
    private Animator animator; 
    private bool isOpen = false; 
    private bool playerInTrigger = false;

    void Start()
    {
        animator = GetComponent<Animator>();
    }
    public bool canInteract()
    {
        return playerInTrigger && !isOpen; // Chỉ cho phép tương tác nếu cửa chưa mở
    }

    public void Interact()
    {
        if (!isOpen)
        {
            isOpen = true;
            animator.SetTrigger("OpenDoor"); // Trigger animation mở cửa
                                             // spriteRenderer.sprite = openSprite; // Nếu dùng sprite
            Debug.Log("Door opened!");
        }
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = true;
            if (canInteract())
            {
                Interact(); // Tự động mở cửa khi chạm
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInTrigger = false;
            isOpen = false;
            animator.SetTrigger("CloseDoor"); // Trigger animation đóng cửa
        }
    }
}
