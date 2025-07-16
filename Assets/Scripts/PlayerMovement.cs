using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f; // tốc độ di chuyển

    private Rigidbody2D rb;
    private Vector2 movement;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
       
    }

    public void Move(InputAction.CallbackContext context)
    {
        // Lấy giá trị di chuyển từ Input System
        movement = context.ReadValue<Vector2>();
    }

    void FixedUpdate()
    {
        rb.linearVelocity = movement * moveSpeed; // Cập nhật vận tốc của Rigidbody2D
    }
}
