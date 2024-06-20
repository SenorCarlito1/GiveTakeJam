using System.Collections;
using System.Collections.Generic;
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
    [SerializeField] private Material buildingMatNegative;

    private bool inBuildMode = false;

    private Camera _camera;

    public GameObject gameObjectToPosition;

    void Start()
    {
        _camera = GameManager.instance.currCamera;
    }

    // Update is called once per frame
    void Update()
    {
        _camera = GameManager.instance.currCamera;

        if (gameObjectToPosition == null || !IsRayHit(buildModLayerMask, out RaycastHit hitInfo))
        {
            return;
        }
        
        // Changes 
        if (Input.GetKeyDown(KeyCode.F6)) inBuildMode = true;

        if (inBuildMode && GameManager.instance.currCamera)
        {
            gameObjectToPosition.transform.position = hitInfo.point + new Vector3(0, 3, 0);
            if (Input.GetKey(KeyCode.R))
            {
                gameObjectToPosition.transform.Rotate(0, 1, 0);
            }
            if (Input.GetKeyDown(KeyCode.E))
            {

                Instantiate(gameObjectToPosition, hitInfo.point + new Vector3(0, 3, 0), gameObjectToPosition.transform.rotation);
                inBuildMode = false;
            }
        }




    }
    private void FixedUpdate()
    {
        // Show the raycast line in the scene window

        RaycastHit hit;
        if (Physics.Raycast(_camera.transform.position, _camera.transform.forward, out hit, Mathf.Infinity))
        {

            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * hit.distance, Color.yellow);
            Debug.Log("Did Hit");
        }
        else
        {
            Debug.DrawRay(transform.position, transform.TransformDirection(Vector3.forward) * 1000, Color.white);
            Debug.Log("Did not Hit");
        }
    }
    private bool IsRayHit(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, _camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, (int)layerMask);
    }
}
