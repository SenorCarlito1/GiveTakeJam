using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cameraControl : MonoBehaviour
{
    [SerializeField] private int sensHor;
    [SerializeField] private int sensVert;

    [SerializeField] private int lockVermin;
    [SerializeField] private int lockVermax;

    [SerializeField] private bool invertY;

    private float xRoation;

    private Vector3 posOrig;

    private void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        posOrig = transform.position;
    }

    private void Update()
    {
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sensVert;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sensHor;

        if (invertY)
        {
            xRoation += mouseY;
        }
        else
        {
            xRoation -= mouseY;
        }

        xRoation = Mathf.Clamp(xRoation, lockVermin, lockVermax);

        transform.localRotation = Quaternion.Euler(xRoation, 0, 0);

        transform.parent.Rotate(Vector3.up * mouseX);
    }
}
