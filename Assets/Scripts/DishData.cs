using UnityEngine;

[CreateAssetMenu(fileName = "DishData", menuName = "ScriptableObjects/DishData")]
public class DishData : ScriptableObject
{
    public int dishId;
    public Sprite dishSprite;
    public GameObject fishIngredient;
    public GameObject vegetableIngredient;
}
