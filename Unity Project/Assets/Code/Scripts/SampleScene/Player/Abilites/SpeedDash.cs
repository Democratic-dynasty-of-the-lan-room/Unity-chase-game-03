using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Code.Scripts.SampleScene.Player;
using UnityEngine.SceneManagement;

public class SpeedDash : MonoBehaviour
{
    Dash Script;

    AbilityManager abilityManager;

    public bool DashButton;

    public bool DashEquiped;

    public bool DashEquiped1;

    public bool DashOn;

    public Button yourButton;

    private void Awake()
    {

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

            // Is this needed?
            Script.enabled = false;
        }
    }

    private void OnDisable()
    {
     
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void Update()
    {
         
    }

    void TaskOnClick()
    {
        Script = FindAnyObjectByType<Dash>();
        abilityManager = FindAnyObjectByType<AbilityManager>();

        if (Script.enabled == false)
        {
            Script.enabled = true;

            abilityManager.DashingAble = true;

            // I don't wan't to Save the game here, but it does work. Otherwise SaveInventoryName is nothing When I check what the name is here.
            DataPersistenceManager.instance.SaveGame();

            // Maybe if there are no names then it just uses bool 1? Though this could lead to errors.
            for (int i = 0; i < abilityManager.SaveInventorySlotIndex.Length; i++)
            {
                if (abilityManager.SaveInventoryName[i] == "ImageDash(Clone)")
                {
                    abilityManager.AbilityActive[i] = true;

                    Debug.Log("AbilityActiveTrue: " + abilityManager.AbilityActive[i]);
                }
                else
                {
                    Debug.Log("Not Correct Item: " + abilityManager.SaveInventoryName[i]);
                }
            }
        }
        else
        {
            Script.enabled = false;

            abilityManager.DashingAble = false;


            for (int i = 0; i < abilityManager.SaveInventorySlotIndex.Length; i++)
            {
                if (abilityManager.SaveInventoryName[i] == "ImageDash(Clone)")
                {
                    abilityManager.AbilityActive[i] = false;

                    Debug.Log("AbilityActiveFalse: " +  abilityManager.AbilityActive[i]);
                }
                else
                {
                    Debug.Log("Not Correct Item: " + abilityManager.SaveInventoryName[i]);
                }
            }
        }

        Debug.Log("SaveGame SpeedDash");
    }
}
