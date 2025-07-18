using UnityEngine;

public class Chef : MonoBehaviour
{
    public Transform storagePoint;
    public Transform kitchenPoint;
    public Transform privateRoomPoint;
    public float waitTimeAtPoint = 3f;
    private UnityEngine.AI.NavMeshAgent agent;
    private Animator animator;
    private enum BossState { Idle, MovingToStorage, MovingToKitchen, CollectingFish, CollectingVegetable, MovingToPrivateRoom }
    private BossState currentState = BossState.Idle;
    public float collectFishTimer = 2f;
    public float collectVegetableTimer = 2f;
    public float cookingTimer = 5f;
    private OrderCard currentOrder; // OrderCard đang xử lý
    void Start()
    {
        agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false;
        agent.updateUpAxis = false;
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
                    currentOrder = OrderManager.Instance.activeOrders[0];
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
                    currentState = BossState.CollectingFish;
                    agent.SetDestination(currentOrder.dishData.fishIngredient.transform.position);
                    Debug.Log($"Boss moving to fish: {currentOrder.dishData.fishIngredient.name}");
                }
                break;

            case BossState.CollectingFish:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    collectFishTimer -= Time.deltaTime;
                    if (collectFishTimer <= 0)
                    {
                        animator.SetTrigger("CollectIngredients");
                        currentState = BossState.CollectingVegetable;
                        agent.SetDestination(currentOrder.dishData.vegetableIngredient.transform.position);
                        collectFishTimer = waitTimeAtPoint;
                        Debug.Log($"Boss collecting fish: {currentOrder.dishData.fishIngredient.name}");
                    }
                }
                break;

            case BossState.CollectingVegetable:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    collectVegetableTimer -= Time.deltaTime;
                    if (collectVegetableTimer <= 0)
                    {
                        animator.SetTrigger("CollectIngredients");
                        currentState = BossState.MovingToKitchen;
                        agent.SetDestination(kitchenPoint.position);
                        collectVegetableTimer = waitTimeAtPoint;
                        Debug.Log($"Boss collecting vegetable: {currentOrder.dishData.vegetableIngredient.name}");
                    }
                }
                break;

            case BossState.MovingToKitchen:
                if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
                {
                    cookingTimer -= Time.deltaTime;
                    if (cookingTimer <= 0)
                    {
                        currentState = BossState.Idle;
                        cookingTimer = 5f;
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
