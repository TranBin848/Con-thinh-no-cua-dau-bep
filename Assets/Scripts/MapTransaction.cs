using Unity.Cinemachine;
using UnityEngine;

public class MapTransaction : MonoBehaviour
{
    [SerializeField] PolygonCollider2D mapBoundary;
    CinemachineConfiner2D confiner;
    [SerializeField] Direction direction;
    [SerializeField] float addictivePos = 2f;

    enum Direction
    {
        Up,
        Down,
        Left,
        Right
    }
    private void Awake()
    {
        confiner = FindFirstObjectByType<CinemachineConfiner2D>();
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            confiner.BoundingShape2D = mapBoundary;
            UpdatePlayerPosition(collision.gameObject);
            Debug.Log("Player entered map boundary, confiner updated.");
        }
    }
    private void UpdatePlayerPosition(GameObject player)
    {
        Vector3 newPos = player.transform.position;
        switch (direction)
        {
            case Direction.Up:
                newPos.y += addictivePos;
                break;
            case Direction.Down:
                newPos.y -= addictivePos;
                break;
            case Direction.Left:
                newPos.x -= addictivePos;
                break;
            case Direction.Right:
                newPos.x += addictivePos;
                break;
        }
        player.transform.position = newPos;
    }
}
