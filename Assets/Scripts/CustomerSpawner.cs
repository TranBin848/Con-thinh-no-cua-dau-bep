using System.Collections.Generic;
using UnityEngine;
using System.Collections.Generic;

public class CustomerSpawner : MonoBehaviour
{
    public GameObject customerPrefab;
    public Transform spawnPoint;
    public MenuList menuList;

    public List<Chair> chairs = new List<Chair>();

    public float spawnInterval = 5f;

    void Start()
    {
        InvokeRepeating("SpawnCustomer", 0f, spawnInterval);
    }

    void SpawnCustomer()
    {
        Chair targetChair = FindEmptyChair();
        if (targetChair != null)
        {
            GameObject customer = Instantiate(customerPrefab, spawnPoint.position, Quaternion.identity);
            Customer customerScript = customer.GetComponent<Customer>();

            customerScript.menuList = menuList;
            customerScript.chairTarget = targetChair.transform;
            customerScript.chairScript = targetChair;

            targetChair.isOccupied = true;
        }
        else
        {
            Debug.Log("Hết ghế trống!");
        }
    }

    Chair FindEmptyChair()
    {
        foreach (Chair chair in chairs)
        {
            if (!chair.isOccupied)
                return chair;
        }
        return null;
    }
}
