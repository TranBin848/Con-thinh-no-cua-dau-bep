using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class Customer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform chairTarget; // ghế mà khách sẽ ngồi
    public Chair chairScript;
    public MenuList menuList;

    private bool isMoving = false;

    NavMeshAgent agent;
    private Animator animator;
    void Start()
    {
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
        // Check if arrived at chair
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            SitDown();
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
        // Gọi món ngẫu nhiên
        int index = Random.Range(0, menuList.dishes.Count);
        string orderedDish = menuList.dishes[index];

        Debug.Log(gameObject.name + " gọi món: " + orderedDish);
        // TODO: hiển thị UI order lên khách
    }

    void SitDown()
    {
        if (chairScript != null)
        {
            // Giả sử chairScript có thuộc tính sitDirection (-1 hoặc +1)
            animator.SetBool("isSitting", true);
            animator.SetInteger("sitDirection", chairScript.sitDirection);

            agent.isStopped = true;
            agent.ResetPath();

            Debug.Log("Khách đã ngồi vào ghế");
        }
    }
}
