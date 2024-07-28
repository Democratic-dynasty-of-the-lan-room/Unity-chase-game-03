using Code.Scripts.SampleScene.MenuScripts;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    private IEnumerator coroutine;

    private IEnumerator afterHitCoroutine;

    public bool BypassRegen;

    public float RegenHealthWaitTime;

    public int RegenHealthAmount;

    [Tooltip("This is the amount of Time to wait before starting to regen after being damaged")]
    public float AfterHitWaitTime;

    private bool CanRegen = true;

    private bool CanHitWaitTime = true;

    private bool CanStartRegen = true;



    public int Health;

    public static PlayerHealth Instance { get; private set; }

    //public int Health = Instance.Health;

    [Header("Warning this doesn't work Completely yet. Im not sure how to set the default GameData to SetHealthAmount")]
    public int SetHealthAmount;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject PauseMenu;

    [SerializeField] GameObject InventoryScript;

    public GameObject RestartMenu;

    public GameData gameData;

    public bool OverideDeath;

    private int previousHealth;

    private bool CanPreviousHealth;

    [Header("Check what health is with H")]
    public bool ShowHealthWithH;

    private void Awake()
    {
        //Time.timeScale = 1.0f;
        
        // Make sure there is only one instance to keep the singleton pattern
        if (Instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
    }

    private void Start()
    {        
        CanPreviousHealth = true;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.H) && ShowHealthWithH)
        {
            Debug.Log("Health = " + Health);
        }
    }

    private void FixedUpdate()
    {    
        if (CanPreviousHealth)
        {
            previousHealth = Health;

            //Debug.Log("Health in FixedUpdate: " + Health);

            CanPreviousHealth = false;
        }

        if (Health <= 0 && !OverideDeath)
        {
            //Debug.Log("Health in death" + Health);

            // We probably want this. Just need to find a good place to set it to 1o again. That works with other scripts.
            //Time.timeScale = 0f;

            RestartMenu.SetActive(true);
            // setting the player to false so that you can't move after the end

            Player.SetActive(false);
            //Player.GetComponent<PlayerMovment>().enabled = false;
            //Enabling this
            this.enabled = false;

            //Showscursor so that you can click restart
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PauseMenu.SetActive(false);
            InventoryScript.SetActive(false);
        }  

        // If you where hit Take Time before being able to regen Health.
        if (Health < previousHealth && CanHitWaitTime && Health >= 1)
        {
            coroutine = RegenWait(RegenHealthWaitTime);
            StopCoroutine(coroutine);


            afterHitCoroutine = AfterHitCoroutine(AfterHitWaitTime);
            StartCoroutine(afterHitCoroutine);


            previousHealth = Health;

            CanStartRegen = false;

            CanHitWaitTime = false;

            //Debug.Log("Health Went Down");
        }
        else
        {
            previousHealth = Health;
        }

        if (Health < SetHealthAmount && !BypassRegen && CanRegen && CanStartRegen)
        {
            if (Health >= 1)
            {
                coroutine = RegenWait(RegenHealthWaitTime);
                StartCoroutine(coroutine);
            }     

            CanRegen = false;

            //Debug.Log("Able To Regenerate Health Now");
        }
    }


    private IEnumerator RegenWait(float RegenWaitTime)
    {
        if (Health < SetHealthAmount)
        {
            //print("Coroutine Beginning Time: " + Time.time + " seconds");

            yield return new WaitForSeconds(RegenWaitTime);
            //print("Coroutine ended: " + Time.time + " seconds");

            //Health = SetHealthAmount;

         
            if (Health + RegenHealthAmount < SetHealthAmount)
            {
                Health = Health + RegenHealthAmount;
            }
            else if (Health + RegenHealthAmount > SetHealthAmount)
            {
                Health = SetHealthAmount;
            }

            CanRegen = true;

            //Debug.Log("Health Regen: " + Health);
        }
    }

    private IEnumerator AfterHitCoroutine(float AfterHitWaitTime)
    {
        //print("AfterHit Beginning Time: " + Time.time + " seconds");

        yield return new WaitForSeconds(AfterHitWaitTime);
        //print("AfterHit ended: " + Time.time + " seconds");

        coroutine = RegenWait(RegenHealthWaitTime);
        StartCoroutine(coroutine);

        // add's first health amount after AfterHitCoroutine has finished.
        //Health = Health + RegenHealthAmount;

        if (Health + RegenHealthAmount < SetHealthAmount)
        {
            Health = Health + RegenHealthAmount;
        }
        else if (Health + RegenHealthAmount > SetHealthAmount)
        {
            Health = SetHealthAmount;
        }

        CanStartRegen = true;

        CanHitWaitTime = true;

        //Debug.Log("After Hit Wait Time: " + Health);
    }


    public void LoadData(GameData data)
    {
        Health = data.PlayerHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerHealth = Health;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
