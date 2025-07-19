using UnityEngine;
using System.Collections.Generic;
    
public class OrderManager : MonoBehaviour
{
    public static OrderManager Instance;
    public GameObject orderCardPrefab;
    public Transform orderPanel;
    public List<OrderCard> activeOrders = new List<OrderCard>();
   
    private void Awake()
    {
        Instance = this;
    }

    public void AddOrder(DishData dish, float orderTime, Customer customer)
    {
        // Tạo card mới
        GameObject card = Instantiate(orderCardPrefab, orderPanel);
        OrderCard cardScript = card.GetComponent<OrderCard>();
        cardScript.Setup(dish, orderTime, customer);
        activeOrders.Add(cardScript);
    }

    public void RemoveOrder(OrderCard card)
    {
        activeOrders.Remove(card);
        Destroy(card.gameObject);
    }
}
