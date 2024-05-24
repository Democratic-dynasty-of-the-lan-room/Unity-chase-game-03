using UnityEngine;
using UnityEngine.UI;
using System.Collections;
//using System.Diagnostics.CodeAnalysis;

public class SpeedDash : MonoBehaviour, IDataPersistence
{
    //private GameObject Player;

    Dash Script;

    public bool DashButton;

    public bool DashEquiped;

    public Button yourButton;

    private void Awake()
    {
        //Player = GameObject.FindWithTag("Player");

        // Maybe Save the DashButton here
        // And then load it in ability manager
        // Then instantiate this from the ability manager
        // Find out how to Save and load the right position as well
        // Maybe make a simple save this .transorm.position on this script
        // And then instantiate and load it to this.transform.position.
        // Though maybe you can set it to the transform of the inventory slot it was in. by parenting it to the right inventory slot. So Save which slot it needs to be parented to?
        DashButton = true;
        Debug.Log("awakeDashButton: " + DashButton);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
        //Debug.Log("Fixed Update");

        if (DashButton == true)
        {
            DashEquiped = true;

            Debug.Log("DashEquiped: " + DashEquiped);

            DashButton = false;
        }
    }

    void TaskOnClick()
    {
        Script = FindAnyObjectByType<Dash>();

        //Script = Player.GetComponentInChildren<Dash>();

        Debug.Log("You have clicked the button!");

        if (Script.enabled == false)
        {
            Script.enabled = true;
        }
        else
        {
            Script.enabled = false;
        }
    }

    public void LoadData(GameData data)
    {
    
    }

    public void SaveData(ref GameData data)
    {
        Debug.Log("DashButton: " + DashButton);

        data.ADashInventory = DashEquiped;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
   
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
     
    }
}
