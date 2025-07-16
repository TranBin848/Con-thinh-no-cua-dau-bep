using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "MenuList", menuName = "ScriptableObjects/MenuList")]
public class MenuList : ScriptableObject
{
    public List<string> dishes = new List<string>();
}
