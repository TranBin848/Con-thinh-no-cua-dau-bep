using UnityEngine;

public class Chef : MonoBehaviour
{
    public Transform storagePoint;
    public Transform kitchenPoint;
    public Transform privateRoomPoint;
    public float waitTimeAtPoint = 3f;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private enum BossState { Idle, MovingToStorage, MovingToKitchen, MovingToPrivateRoom }
    private BossState currentState = BossState.Idle;
    private float waitTimer;

    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        waitTimer = 0f;
    }

    void Update()
    {
        // Cập nhật animation di chuyển
        Vector3 velocity = agent.velocity;
        if (velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
            animator.SetFloat("moveX", velocity.normalized.x);
            animator.SetFloat("moveY", velocity.normalized.y);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }

        // Logic di chuyển dựa trên OrderCard
        switch (currentState)
        {
            case BossState.Idle:
                if (OrderManager.Instance.activeOrders.Count > 0)
                {
                    // Có OrderCard, đi đến kho
                    currentState = BossState.MovingToStorage;
                    agent.SetDestination(storagePoint.position);
                    Debug.Log("Boss moving to storage");
                }
                else
                {
                    // Không có OrderCard, đi đến phòng riêng
                    currentState = BossState.MovingToPrivateRoom;
                    agent.SetDestination(privateRoomPoint.position);
                    Debug.Log("Boss moving to private room");
                }
                break;

            case BossState.MovingToStorage:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    waitTimer -= Time.deltaTime;
                    if (waitTimer <= 0)
                    {
                        currentState = BossState.MovingToKitchen;
                        agent.SetDestination(kitchenPoint.position);
                        waitTimer = waitTimeAtPoint;
                        Debug.Log("Boss moving to kitchen");
                    }
                }
                break;

            case BossState.MovingToKitchen:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    waitTimer -= Time.deltaTime;
                    if (waitTimer <= 0)
                    {
                        currentState = BossState.Idle;
                        waitTimer = waitTimeAtPoint;
                        Debug.Log("Boss finished at kitchen, checking orders");
                    }
                }
                break;

            case BossState.MovingToPrivateRoom:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    animator.SetBool("isResting", true); // Animation nghỉ
                    currentState = BossState.Idle;
                    Debug.Log("Boss resting in private room");
                }
                break;
        }
    }
}
