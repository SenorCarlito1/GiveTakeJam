using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemPickupScript : MonoBehaviour
{
    public InventoryObject inventory;
    public ItemObject item;
    private MeshFilter meshFilter;  
    private Material material;

    private bool inRange;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        PickUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            inventory = other.GetComponentInChildren<PlayerInventory>().inventory;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            inventory = null;
        }
    }

    void PickUp()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                inventory.AddItem(item, 1);
                inRange = false;
                Destroy(gameObject);
            }
        }
    }
}
