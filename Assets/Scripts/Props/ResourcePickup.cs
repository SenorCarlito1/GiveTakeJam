using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourcePickup : MonoBehaviour
{
    [SerializeField] private ResourceObject resource;
    public HotBarObject hotBar;

    private bool inRange;


    // Update is called once per frame
    private void Update()
    {
        PickUp();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = true;
            hotBar = other.GetComponentInChildren<PlayerHotBar>().hotBar;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
            hotBar = null;
        }
    }
    private void PickUp()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                hotBar.AddItem(resource, 1);
                GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
                inRange = false;
                Destroy(gameObject);
            }
        }
    }
}
