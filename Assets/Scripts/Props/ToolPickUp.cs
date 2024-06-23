using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToolPickUp : MonoBehaviour
{
    [SerializeField] private ToolStats tool;
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
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inRange = false;
        }
    }

    void PickUp()
    {
        if (inRange)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                GameManager.instance.attackScript.AddTool(tool);
                inRange = false;
                Destroy(gameObject);
            }
        }
    }
}
