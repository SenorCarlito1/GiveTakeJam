using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickUp : MonoBehaviour
{
    [SerializeField] private ToolStats tool;
    public HotBarObject hotBar;
    private MeshFilter model;
    private MeshRenderer mat;

    private bool inRange;

    private void Start()
    {
        //model = tool.model.GetComponent<MeshFilter>();
        //mat = tool.model.GetComponent<MeshRenderer>();
        //inRange = false;
    }

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

    void PickUp()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.attackScript.AddTool(tool);
                hotBar.AddItem(tool, 1);
                GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
                inRange = false;
                Destroy(gameObject);
            }
        }
    }
}
