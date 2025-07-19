using UnityEngine;
using UnityEngine.XR;

public class Food : MonoBehaviour, IInteractable
{
    private bool canBeInteracted = true;
    public GameObject food;
    public void Interact()
    {
        if (canBeInteracted)
        {
            // Gắn món ăn vào người chơi
            PlayerMovement player = FindFirstObjectByType<PlayerMovement>();
            if (player != null)
            {
                transform.SetParent(player.transform);
                food.transform.localScale = new Vector3(0.6f, 0.6f, 0.6f); // Giảm kích thước món ăn
                canBeInteracted = false; // Ngăn tương tác lại
                player.SetCarryingFood(true, this); // Cập nhật trạng thái người chơi
            }
        }
    }

    public bool canInteract()
    {
        return canBeInteracted;
    }

    public void UpdatePosition(Vector2 direction)
    {
        // Cập nhật vị trí món ăn dựa trên hướng di chuyển
        if (!canBeInteracted && transform.parent != null)
        {
            Vector2 newPos = new Vector2(0, -0.5f);
            if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
            {
                // Ưu tiên hướng ngang (trái/phải)
                newPos = direction.x > 0 ? new Vector2(0.6f, -0.4f) : new Vector2(-0.55f, -0.4f); // Phải/Trái
            }
            else if (Mathf.Abs(direction.y) > 0)
            {
                // Ưu tiên hướng dọc (trên/dưới)
                newPos = direction.y > 0 ? new Vector2(0, 0.6f) : new Vector2(0, -0.5f); // Trên/Dưới
            }
            transform.localPosition = new Vector3(newPos.x, newPos.y, 0);
            Debug.Log($"{gameObject.name} updated localPosition to {transform.localPosition}");
        }
    }
}
