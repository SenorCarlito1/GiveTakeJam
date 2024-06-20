using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class BuildingPlacement : MonoBehaviour
{
    [SerializeField] private float rayDistance;
    [SerializeField] private LayerMask buildModLayerMask;
    [SerializeField] private int defaultLayerInt;
    [SerializeField] private Transform rayOrigin;
    [SerializeField] private Material buildingMatPositive;
    [SerializeField] private Material buildingMatNegative;

    private bool inBuildMode = false;

    private Camera _camera;

    public GameObject gameObjectToPosition;
    void Start()
    {
        _camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObjectToPosition == null || !IsRayHit(buildModLayerMask, out RaycastHit hitInfo))
        {
            return;
        }

        if (Input.GetKeyDown(KeyCode.F6)) inBuildMode = true;

        
        if (inBuildMode)
        {
            gameObjectToPosition.transform.position = hitInfo.point + new Vector3(0, 3, 0);

            if (Input.GetKeyDown(KeyCode.E))
            {
                // transform.position + (transform.forward * 10) + new Vector3(0, 2, 0)
                Instantiate(gameObjectToPosition, hitInfo.point + new Vector3(0, 3, 0), Quaternion.identity);
                inBuildMode = false;
            }
        }
        



    }

    private bool IsRayHit(LayerMask layerMask, out RaycastHit hitInfo)
    {
        var ray = new Ray(rayOrigin.position, _camera.transform.forward * rayDistance);
        return Physics.Raycast(ray, out hitInfo, rayDistance, (int)layerMask);
    }
}
