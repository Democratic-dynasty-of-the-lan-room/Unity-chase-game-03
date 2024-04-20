using System.Collections;
using System.Collections.Generic;
using System.Xml.Serialization;
using UnityEngine;


public class PlayerCam : MonoBehaviour, IDataPersistence
{
    public float sensX;
    public float sensY;

    public Transform orientation;

    float xRotation;
    float yRotation;

    private Vector2 currentMouseDelta;
    private Vector2 previousMouseDelta;

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = true;    
    }

    private void Update()
    {
        // get mouse input
        float mouseX = Input.GetAxisRaw("Mouse X") * sensX;
        float mouseY = Input.GetAxisRaw("Mouse Y") * sensY;

        currentMouseDelta = new Vector2(mouseX, mouseY);
        Vector2 averageDelta = (currentMouseDelta + previousMouseDelta) / 2.0f;
        previousMouseDelta = currentMouseDelta;

        // Apply mouse movement
        Vector3 rotation = transform.localEulerAngles;
        rotation.x -= averageDelta.y * sensY;
        rotation.y += averageDelta.x * sensX;
        transform.localEulerAngles = rotation;


        rotation.x = Mathf.Clamp(rotation.x, -90f, 90f);
        /*yRotation += mouseX;

        xRotation -= mouseY;
       
        

        // rotate cam and orientation
        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0);
        */
        orientation.rotation = Quaternion.Euler(0, rotation.y, 0);   
    }

    // Why isn't this working?
    public void LoadData(GameData data)
    {     
        this.transform.rotation = data.PlayerRotation;
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerRotation = this.transform.rotation;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {

    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {

    }
}


