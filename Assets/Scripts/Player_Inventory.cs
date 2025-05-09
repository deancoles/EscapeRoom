using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Inventory : MonoBehaviour
{
    public List<Item> items;        // List storing all items currently held by the player.

    // Adds an item to the player’s inventory if it’s not already present and updates the UI.
    public void AddItem(Item item)
    {
        if(items.Contains(item))
        {
            return;
        }

        UI_Manager.instance.SetItems(item, items.Count);
        items.Add(item);
    }

    // Removes an item from inventory and updates the inventory UI.
    public void RemoveItem(Item item)
    {
        if (item == null || !items.Contains(item))
        {
            return;
        }

        items.Remove(item);
        UI_Manager.instance.RefreshItems(items);    // Assuming UI needs updating after removal
    }

}
