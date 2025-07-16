using UnityEngine;
using System.Collections;
using UnityEngine.AI;
public class Customer : MonoBehaviour
{
    public float moveSpeed = 2f;
    public Transform chairTarget; // ghế mà khách sẽ ngồi
    public MenuList menuList;

    private bool isMoving = false;

    public NavMeshAgent agent;
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.updateRotation = false; // Tắt cập nhật xoay của NavMeshAgent
        agent.updateUpAxis = false; // Tắt cập nhật trục Y của NavMeshAgent
        if (chairTarget != null)
        {
            agent.SetDestination(chairTarget.position); // Đặt điểm đến là ghế
            //StartCoroutine(MoveToChair());
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
}
