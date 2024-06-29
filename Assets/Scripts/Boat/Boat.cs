using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boat : MonoBehaviour
{
    [SerializeField] private Transform[] requiredPartSlots; // Use Transform to store slots for easy parenting

    private InventoryObject playerInventory;
    private HotBarObject playerHotBar;

    // List to keep track of instantiated parts
    [SerializeField] private List<GameObject> instantiatedParts = new List<GameObject>();

    private void Start()
    {
        // Reference the inventory object from the GameManager
        playerInventory = GameManager.instance.inventoryObject;
        playerHotBar = GameManager.instance.hotBarObject;
        // Initialize visualization in the editor
        UpdateSlotVisualization();
    }

    private void Update()
    {
        // Attach parts when player presses 'F'
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F key pressed. Attempting to attach parts...");

            // Attempt to attach any parts labeled as ItemType.Part
            AttachPartsFromInventory();

            // Check if all parts are attached
            if (AreAllPartsAttached())
            {
                Debug.Log("Boat assembly complete!");
                GameManager.instance.Win();
            }

            // Update visualization after attaching parts
            UpdateSlotVisualization();
        }
    }

    // Attach parts from the player's inventory
    private void AttachPartsFromInventory()
    {
        for (int i = playerHotBar.Container.Count - 1; i >= 0; i--)
        {
            HotBarObject.HotbarSlot slot = playerHotBar.Container[i];
            if (slot.item.type == ItemType.Part) // Assuming ItemType.Part is used for boat parts
            {
                Debug.Log($"Found part {slot.item.name} in inventory. Attempting to attach...");
                GameObject partPrefab = slot.item.itemPrefab;
                if (partPrefab != null && AttachPart(partPrefab))
                {
                    Debug.Log($"{slot.item.name} successfully attached. Removing from inventory...");
                    RemovePartFromInventory(slot);
                }
                else
                {
                    Debug.Log($"Failed to attach {slot.item.name}. No empty slots or prefab is null.");
                }
            }
        }
    }

    // Attach a part to the boat
    private bool AttachPart(GameObject partPrefab)
    {
        for (int i = 0; i < requiredPartSlots.Length; i++)
        {
            if (requiredPartSlots[i].childCount == 0) // Check if slot is empty
            {
                GameObject part = Instantiate(partPrefab, requiredPartSlots[i]);
                part.transform.localPosition = Vector3.zero;
                part.SetActive(true);
                instantiatedParts.Add(part);
                return true;
            }
        }
        return false;
    }

    // Check if all required parts are attached
    private bool AreAllPartsAttached()
    {
        return instantiatedParts.Count == requiredPartSlots.Length;
    }

    // Remove part from the player's inventory
    private void RemovePartFromInventory(HotBarObject.HotbarSlot slot)
    {
        slot.amount--;
        if (slot.amount <= 0)
        {
            playerHotBar.Container.Remove(slot);
        }
    }

    // Update the inspector to show current parts in the boat
    private void UpdateSlotVisualization()
    {
        for (int i = 0; i < requiredPartSlots.Length; i++)
        {
            Transform slot = requiredPartSlots[i];
            if (slot.childCount > 0)
            {
                Debug.Log($"Slot {i + 1}: {slot.GetChild(0).name} is attached.");
            }
            else
            {
                Debug.Log($"Slot {i + 1}: No part attached.");
            }
        }
    }
}
