using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Code.Scripts.SampleScene.Player;
using UnityEngine.SceneManagement;

public class SpeedDash : MonoBehaviour, IDataPersistence
{
    //[SerializeField] AbilityManager abilityManager;
    //[SerializeField] InventoryScript inventoryScript;

    Dash Script;

    AbilityManager abilityManager;

    public bool DashButton;

    public bool DashEquiped;

    public bool DashEquiped1;

    public bool DashOn;

    public Button yourButton;

    private void Awake()
    {
        DashButton = true;
        Debug.Log("awakeDashButton: " + DashButton);

        
        // Bad place because awake doesn't run every time this is set to active, only the first time.
        //for (int i = 0; i < inventoryScript.slots.Length; i++)
        //{
        //    if (abilityManager.AbilityActive[i] == true)
        //    {
                /*
                if (abilityManager.SaveInventoryName[i] == abilityManager.AbilityActive[i])
                {

                }
                */
        //    }
        //}
        

        //DashEquiped1 = false;
    }

    private void OnEnable()
    {
        abilityManager = FindAnyObjectByType<AbilityManager>();

        Script = FindAnyObjectByType<Dash>();

        // This needs to happen on Scene loaded on as script that isn't sometimes not enabled ideally.
        if (abilityManager.DashingAble == true)
        {
            Script.enabled = true;

            Debug.Log("SceneLoaded");
        }
        else
        {
            Debug.Log("DashingAble not true");
        }

        //SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        //SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /*
    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
  
    }
    */

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);

        /*
        Script = FindAnyObjectByType<Dash>();
        if (DashEquiped1 == true)
        {
            Script.enabled = true;

            Debug.Log("Script = true");
        }
        else
        {
            Script.enabled = false;

            Debug.Log("Script = false");
        }
        */
    }

    private void Update()
    {
   



        if (DashButton == true)
        {
            DashEquiped = true;

            Debug.Log("DashEquiped: " + DashEquiped);

            DashButton = false;
        }
        

        //Script = FindAnyObjectByType<Dash>();

        /*if (DashEquiped1 == true)
        {
            Script.enabled = true;

            Debug.Log("Script = true");
        }
        else
        {
            Script.enabled = false;
        }
        */
        if (Input.GetKeyDown(KeyCode.V))
        {
            Script = FindAnyObjectByType<Dash>();
            Debug.Log(Script.enabled);
            Debug.Log("Dash Equiped1" + DashEquiped1);
            DataPersistenceManager.instance.SaveGame();
        }
    }

    void TaskOnClick()
    {
        Script = FindAnyObjectByType<Dash>();
        abilityManager = FindAnyObjectByType<AbilityManager>();

        //Debug.Log("You have clicked the button!");

        if (Script.enabled == false)
        {
            Script.enabled = true;

            abilityManager.DashingAble = true;

            DashEquiped1 = true;

            //DataPersistenceManager.instance.SaveGame();

            //DashOn = false;

            Debug.Log("Dash: " + DashEquiped1);
        }
        else
        {
            //DashOn = true;

            abilityManager.DashingAble = false;

            Script.enabled = false;

            DashEquiped1 = false;

            Debug.Log("Dash Equipped off?: " + DashEquiped1);
        }

        Debug.Log("SaveGame SpeedDash");
    }

    public void LoadData(GameData data)
    {
        DashEquiped1 = data.DashEquiped;
    }

    public void SaveData(ref GameData data)
    {
        Script = FindAnyObjectByType<Dash>();
        data.DashEquiped = Script.enabled;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
   
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
     
    }
}
