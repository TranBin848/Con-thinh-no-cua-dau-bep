using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // tốc độ di chuyển
    private Vector2 movement;

    private Rigidbody2D rb;
    private Animator animator;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    void Update()
    {
       
    }

    public void Move(InputAction.CallbackContext context)
    {
        animator.SetBool("isWalking", true); // Cập nhật trạng thái hoạt hình
        if (context.canceled)
        {
            animator.SetBool("isWalking", false); // Dừng hoạt hình khi không có di chuyển
            animator.SetFloat("LastInputX", movement.x);
            animator.SetFloat("LastInputY", movement.y);
        }

        // Lấy giá trị di chuyển từ Input System
        movement = context.ReadValue<Vector2>();
        animator.SetFloat("InputX", movement.x); // Cập nhật giá trị di chuyển theo trục X
        animator.SetFloat("InputY", movement.y); // Cập nhật giá trị di chuyển theo trục Y

    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed; // Cập nhật vận tốc của Rigidbody2D 
    }
}
