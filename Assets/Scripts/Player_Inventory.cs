using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public List<Item> items;

    public void AddItem(Item item)
    {
        if(items.Contains(item))
        {
            return;
        }

        UI_Manager.instance.SetItems(item, items.Count);
        items.Add(item);

    }

}
