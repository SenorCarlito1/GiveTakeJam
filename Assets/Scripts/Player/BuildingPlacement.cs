using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private int defaultLayerInt;
    [SerializeField] private LayerMask buildModLayerMask;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatOriginal;
    [SerializeField] private Material buildingMatNegative;
   // public GameObject[] buildingList = new GameObject[2];

    private bool inBuildMode = false;

    private int selectedBuilding = 0;

    private Camera _camera;

    [SerializeField] private Buidling gameObjectToPosition;
    Vector3 origPos;

    private MeshCollider[] meshColliders;
    private MeshRenderer[] meshRenders;


    void Start()
    {
        _camera = GameManager.instance.currCamera;
        origPos = gameObjectToPosition.transform.position;
        
    }

    // Update is called once per frame
    void Update()
    {
        _camera = GameManager.instance.currCamera;

        if (gameObjectToPosition == null || !IsRayHit(buildModLayerMask, out RaycastHit hitInfo))
        {
            return;
        }

        // Enables build mode
        if (Input.GetKeyDown(KeyCode.F6) && GameManager.instance.woodCount >= 1) 
            inBuildMode = true;


        SwitchBuilding();

        if (inBuildMode && GameManager.instance.currCamera)
        {

           
            gameObjectToPosition.transform.position = hitInfo.point + new Vector3(0, 3, 0);
            gameObjectToPosition.UpdateMaterial(buildingMatPositive);
            HandleInput(hitInfo);
            if(inBuildMode == false)
            {
                gameObjectToPosition.transform.position = origPos;
            }
            
        }
        
    }

    private void HandleInput(RaycastHit hitInfo)
    {
        if (Input.GetKey(KeyCode.R))
        {
            gameObjectToPosition.transform.Rotate(0, 100 * Time.deltaTime, 0);
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            
            for (int i = 0; i < GameManager.instance.hotBarObject.Container.Count; i++) 
            {
                if (GameManager.instance.hotBarObject.Container[i].item.type == ItemType.Resource)
                {
                    GameManager.instance.woodCount -= 8;
                    GameManager.instance.hotBarObject.Container[i].amount -= 8;

                    if (GameManager.instance.hotBarObject.Container[i].amount == 0)
                    {
                        GameManager.instance.hotBarObject.Container.Remove(GameManager.instance.hotBarObject.Container[i]);
                        
                    }
                   
                }
            }
            
            SpawnBuilding(hitInfo);
            
            // Jymeer 
            for (int i = 0; i < GameManager.instance.hotBarMenu.transform.childCount; i++)
            {
                Destroy(GameManager.instance.hotBarMenu.transform.transform.GetChild(i).gameObject);
            }
            GameManager.instance.hotBarMenu.GetComponentInChildren<DisplayHotBar>().CreateDisplay();
        }
    }

    private void ChangeMeshProperties()
    {
        for (int i = 0; i < meshColliders.Length; i++)
        {
            meshColliders[i].enabled = false;
        }
        for (int i = 0; i < meshRenders.Length; i++)
        {
            meshRenders[i].material = buildingMatPositive;
        }
    }

    private void SwitchBuilding()
    {
        //if (Input.mouseScrollDelta.y > 0 && selectedBuilding < buildingList.Length - 1 && !inBuildMode)
        //{
        //    selectedBuilding++;
        //    ChangeBuilding();
        //    Debug.Log("UP current Index is " + selectedBuilding);

        //}
        //else if (Input.mouseScrollDelta.y < 0 && selectedBuilding > 0 && !inBuildMode)
        //{
        //    selectedBuilding--;
        //    ChangeBuilding();
        //    Debug.Log("DOWN current Index is " + selectedBuilding);
        //}

    }

    private void ChangeBuilding()
    {
        //gameObjectToPosition = buildingList[selectedBuilding].gameObject;
        //meshColliders = buildingList[selectedBuilding].GetComponentsInChildren<MeshCollider>();
        //meshRenders = buildingList[selectedBuilding].GetComponentsInChildren<MeshRenderer>();
    }

    private void SpawnBuilding(RaycastHit hitInfo)
    {
    //    for (int i = 0; i < meshRenders.Length; i++)
    //    {
    //        meshRenders[i].material = buildingMatOriginal;
    //    }
 
        Buidling building = Instantiate(gameObjectToPosition, hitInfo.point + new Vector3(0, 3, 0), gameObjectToPosition.transform.rotation);
       // building.UpdateMaterial(buildingMatOriginal);
       building.PlaceBuilding();     
        
        inBuildMode = false;
    }
    private void FixedUpdate()
    {
        // Show the raycast line in the scene window

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            //Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            //Debug.Log("Did not Hit");
        }
    }
    private bool IsRayHit(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, _camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, (int)layerMask);
    }
}
