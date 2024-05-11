using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class Dash : MonoBehaviour
{
    Rigidbody rb;

    //[SerializeField] GameObject playerMovement;

    private IEnumerator coroutine;

    private bool CanDash;

    public float DashReload;

    private bool dash;

    public float ForceAmount;

    private Camera mainCamera;

    private void Awake()
    {
        this.enabled = false;
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        rb = GetComponent<Rigidbody>();

        mainCamera = Camera.main;
        if (mainCamera == null)
        {
            Debug.LogError("Main camera not found!");
        }

        CanDash = true;

        dash = false;
    }

    // Lol this a terrible way to do this. as if the reload wasn't done ot starts again when you open the menu.
    private void OnEnable()
    {
        if (CanDash == false)
        {
            coroutine = DashReloadTime(DashReload);
            StartCoroutine(coroutine);
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C) && CanDash)
        {
            CanDash = false;
           
            dash = true;

            coroutine = DashReloadTime(DashReload);
            StartCoroutine(coroutine);

            print("Coroutine started");

            print("Coroutine started: " + Time.time + " seconds");
        }
    }

    private void FixedUpdate()
    {
        if (dash)
        {         
            Debug.Log("Dash!");

            //rb.AddForce(new Vector3(0, mainCamera.transform.rotation.y, 0) * ForceAmount, ForceMode.Impulse);

            rb.AddForce(mainCamera.transform.forward * ForceAmount, ForceMode.Impulse);

            dash = false;
        }
    }

    private IEnumerator DashReloadTime(float ReloadTime)
    {
        yield return new WaitForSeconds(ReloadTime);

        print("Coroutine ended: " + Time.time + " seconds");

        CanDash = true;       
    }
}
