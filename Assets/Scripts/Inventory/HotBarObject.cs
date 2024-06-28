using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Hotbar", menuName = "Hotabar System/Hotbar")]
public class HotBarObject : ScriptableObject
{

    public List<HotbarSlot> Container = new List<HotbarSlot>();
    public InventoryObject Inventory;
    public void AddItem(ItemObject item, int amount)
    {
        bool hasItem = false;
        for (int i = 0; i < Container.Count; i++)
        {
            if (Container[i].item.type != ItemType.Tool)
            {
               if (Container[i].item == item)
               {
                  Container[i].AddAmount(amount);
                  hasItem = true;
                  break;
               }
            }
            
        }
        if (!hasItem)
        {
            if(Container.Count == 10)
            {
                Inventory.AddItem(item, amount);
            }
            else
            {
                Container.Add(new HotbarSlot(item, amount));
            }
        }
    }






    [System.Serializable]
    public class HotbarSlot
    {
        public ItemObject item;
        public int amount;
        public HotbarSlot(ItemObject _item, int _amount)
        {
            item = _item;
            amount = _amount;
        }
        public void AddAmount(int value)
        {
            amount += value;
        }
    }
}
