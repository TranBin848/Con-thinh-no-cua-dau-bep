using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class Customer : MonoBehaviour, IInteractable
{
    public string customerID { get; private set; }

    public float moveSpeed = 2f;
    public Transform chairTarget; // ghế mà khách sẽ ngồi
    public Chair chairScript;
    public MenuList menuList;
    public GameObject[] speechBubblePrefabs; // Các prefab khung chat
    public float orderTimeLimit = 10f; // Thời gian chờ nhận món
    public float speechBubbleOffsetY = 1.5f;
    private GameObject currentSpeechBubble; // Khung chat hiện tại
    private float orderTimer;


    private string orderedDish;

    private bool isMoving = false;
    private bool isSitting = false;
    private bool hasOrdered = false;

    NavMeshAgent agent;
    private Animator animator;

    
    void Start()
    {
        customerID ??= GlobalHelper.GenerateUniqueId(gameObject); // Tạo ID duy nhất nếu chưa có
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        agent.updateRotation = false; // Tắt cập nhật xoay của NavMeshAgent
        agent.updateUpAxis = false; // Tắt cập nhật trục Y của NavMeshAgent
        if (chairTarget != null)
        {
            agent.SetDestination(chairTarget.position); // Đặt điểm đến là ghế
            //StartCoroutine(MoveToChair());
        }
        
    }

    private void Update()
    {
        // Cập nhật animation khi di chuyển
        Vector3 velocity = agent.velocity;
        if (velocity.magnitude > 0.1f)
        {
            animator.SetBool("isMoving", true);
        }
        else
        {
            animator.SetBool("isMoving", false);
        }
        animator.SetFloat("moveX", velocity.normalized.x);
        animator.SetFloat("moveY", velocity.normalized.y);

        // Kiểm tra khi đến ghế
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance && !isSitting)
        {
            SitDown();
        }

        // Đếm thời gian chờ nhận món
        if (hasOrdered && orderTimer > 0)
        {
            orderTimer -= Time.deltaTime;
            if (orderTimer <= 0)
            {
                OnOrderTimeout();
            }
        }
    }

    IEnumerator MoveToChair()
    {
        isMoving = true;
        while (Vector2.Distance(transform.position, chairTarget.position) > 0.05f)
        {
            transform.position = Vector2.MoveTowards(transform.position, chairTarget.position, moveSpeed * Time.deltaTime);
            yield return null;
        }
        transform.position = chairTarget.position; // snap vị trí

        isMoving = false;
        OrderFood();
    }

    void OrderFood()
    {
        hasOrdered = true;
        orderTimer = orderTimeLimit;

        // Chọn món ngẫu nhiên
        int index = Random.Range(0, menuList.dishes.Count);
        orderedDish = menuList.dishes[index];

        // Hiển thị khung chat
        int prefabIndex = Random.Range(0, speechBubblePrefabs.Length);
        currentSpeechBubble = Instantiate(speechBubblePrefabs[prefabIndex], transform.position + new Vector3(0, speechBubbleOffsetY, 0), Quaternion.identity);
        currentSpeechBubble.transform.SetParent(transform); // Gắn khung chat vào khách để di chuyển cùng
        Debug.Log(gameObject.name + " gọi món: " + orderedDish);
    }

    public void InteractWithOrder()
    {
        Debug.Log("Check");
        if (hasOrdered && orderTimer > 0)
        {
            Debug.Log("Người chơi nhận đơn: " + orderedDish);
            // Xóa khung chat
            if (currentSpeechBubble != null)
            {
                Destroy(currentSpeechBubble);
            }
            hasOrdered = false;
            // TODO: Thêm logic để xử lý đơn hàng (như thêm vào danh sách nhiệm vụ người chơi)
        }
    }


    void OnOrderTimeout()
    {
        if (hasOrdered)
        {
            Debug.Log(gameObject.name + " hết thời gian chờ, khách phàn nàn!");
            hasOrdered = false;
            if (currentSpeechBubble != null)
            {
                Destroy(currentSpeechBubble);
            }
            // TODO: Giảm danh tiếng quán (cần tích hợp với hệ thống danh tiếng)
        }
    }

    void SitDown()
    {
        if (chairScript != null)
        {
            isSitting = true;
            animator.SetBool("isSitting", true);
            animator.SetInteger("sitDirection", chairScript.sitDirection);
            agent.isStopped = true;
            agent.ResetPath();
            Debug.Log(gameObject.name + " đã ngồi vào ghế");
            OrderFood();
        }
    }

    public bool canInteract()
    {
        return isSitting; // Chỉ có thể tương tác khi đã ngồi
    }
    public void Interact()
    {
        Debug.Log(canInteract() ? "Có thể tương tác với đơn hàng" : "Không thể tương tác với đơn hàng");
        if (canInteract())
        {
            InteractWithOrder();
        }
    }
}
