using UnityEngine;

public class WallRunScript : MonoBehaviour
{
    [SerializeField] PlayerMovment playerMovement;

    [SerializeField] GameObject PlayerGameObject;

    Rigidbody PlayerRB;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        //playerMovement = PlayerGameObject.GetComponent<PlayerMovment>();

        PlayerRB = PlayerGameObject.GetComponent<Rigidbody>();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 9)
        {
            if (PlayerRB.linearVelocity.magnitude > playerMovement.WalkLimit)
            {

            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // TODO - Detect when you hit a wall that has the WhatIsGround Layer. Check velocity is high enough to wall run.
    
        // TODO - Using the Player movement script reference change stuff. 

        // Change gravity amount.
    }
}
