using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Animations;

public class TestPlayerCharacter0MovementScript : MonoBehaviour
{
    Rigidbody rb;

    public float PlayerMovementSpeed;

    public float GroundDraging = 10;


    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
     

        if (Input.GetKey(KeyCode.W))
        { 
            rb.AddForce(Camera.main.transform.forward * PlayerMovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.S))
        {
            rb.AddForce(-Camera.main.transform.forward * PlayerMovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.D))
        {
            rb.AddForce(Camera.main.transform.right * PlayerMovementSpeed * Time.deltaTime);
        }
        if (Input.GetKey(KeyCode.A))
        {
            rb.AddForce(-Camera.main.transform.right * PlayerMovementSpeed * Time.deltaTime);
        }


    }
}
