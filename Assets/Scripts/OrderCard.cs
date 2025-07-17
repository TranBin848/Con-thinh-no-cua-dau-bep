using UnityEngine;
using UnityEngine.UI;
public class OrderCard : MonoBehaviour
{
    public TextMesh dishNameText;
    public Image dishImage;
    public Slider prepSlider;
    private float prepTimeLimit; 
    private float prepTimer; 
    private Customer customer;
    public void Setup(string dishName, Sprite dishSprite, float timeLimit, Customer customer)
    {
        dishNameText.text = dishName;
        dishImage.sprite = dishSprite;
        prepTimeLimit = timeLimit;
        prepTimer = timeLimit;
        this.customer = customer;
    }

    private void Update()
    {
        if (prepTimer > 0)
        {
            prepTimer -= Time.deltaTime;
            prepSlider.value = 1 - (prepTimer / prepTimeLimit); // Slider tăng từ 0 đến 1
            if (prepTimer <= 0)
            {
                customer.OnPrepTimeout(); // Gọi khi hết thời gian
                OrderManager.Instance.RemoveOrder(this);
            }
        }
    }
}
