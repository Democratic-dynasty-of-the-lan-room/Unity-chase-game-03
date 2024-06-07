using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Dash : MonoBehaviour, IDataPersistence
{
    [SerializeField] PlayerMovment Player;

    Rigidbody rb;

    private IEnumerator coroutine;

    private bool CanDash;

    public float DashReload;

    private bool dash;

    public float ForceAmount;

    private Camera mainCamera;

    public bool DashEnabled;

    private bool GroundedDash;

    public float DivideUpwardsMovement;

    PlayerInput Move;

    //public bool EquipedDash;

    private void Awake()
    {       
        Move = new PlayerInput();

        // Take this out?
        this.enabled = false;
    }

    // Lol this a terrible way to do this. as if the reload wasn't done ot starts again when you open the menu.
    private void OnEnable()
    {
        if (CanDash == false)
        {
            coroutine = DashReloadTime(DashReload);
            StartCoroutine(coroutine);
        }

        //EquipedDash = true;

        //Debug.Log("SaveGameDash");
        //DataPersistenceManager.instance.SaveGame();

        Move.Enable();
    }
    private void OnDisable()
    {
        //EquipedDash = false;

        Move.Disable();
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

        if (this.enabled == true)
        {
            DashEnabled = true;
        }
    }

    // Update is called once per frame
    private void Update()
    {
        //bool PressDash = Move.GamePlay.Dash.triggered;

        if (Move.GamePlay.Dash.triggered && CanDash && GroundedDash)
        {
            CanDash = false;
            
            dash = true;

            GroundedDash = false;

            coroutine = DashReloadTime(DashReload);
            StartCoroutine(coroutine);

            print("Coroutine started: " + Time.time + " seconds");
        }

        if (Player.grounded)
        {
            GroundedDash = true;
        }
        else
        {
            //GroundedDash = false;
        }
    }

    private void FixedUpdate()
    {
        if (dash)
        {         
            Debug.Log("Dash!");

            Vector3 forceDirection = mainCamera.transform.forward;

            // limit upwards force.
            forceDirection.y = forceDirection.y / DivideUpwardsMovement;

            rb.AddForce(forceDirection * ForceAmount, ForceMode.Impulse);        

            dash = false;
        }
    }

    private IEnumerator DashReloadTime(float ReloadTime)
    {
        yield return new WaitForSeconds(ReloadTime);

        print("Coroutine ended: " + Time.time + " seconds");

        CanDash = true;       
    }

    public void LoadData(GameData data)
    {
        //EquipedDash = data.equipedDash;
    }

    public void SaveData(ref GameData data)
    {
        //data.equipedDash = EquipedDash;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
   
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {

    }
}
