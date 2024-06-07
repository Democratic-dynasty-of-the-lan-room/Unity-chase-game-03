using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Unity.VisualScripting;
using Code.Scripts.SampleScene.Player;
using UnityEngine.SceneManagement;
using System.Runtime.CompilerServices;

public class ImageDashScript : MonoBehaviour
{
    Dash Script;

    AbilityManager abilityManager;

    public bool DashButton;

    public bool DashEquiped;

    public bool DashEquiped1;

    public bool DashOn;

    public Button yourButton;

    private void OnEnable()
    {     
        abilityManager = FindAnyObjectByType<AbilityManager>();

        Script = FindAnyObjectByType<Dash>();

        //DashOn1 = true;
        EnableDash();
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    private void EnableDash()
    {
        Script = FindAnyObjectByType<Dash>();
        abilityManager = FindAnyObjectByType<AbilityManager>();
        for (int i = 0; i < abilityManager.SaveInventorySlotIndex.Length; i++)
        {
            //Debug.Log("EnableDash ForLoop");
            if (abilityManager.SaveInventoryName[i] == "ImageDash(Clone)")
            {
                //Debug.Log("EnableDash NameOfItem");
                if (abilityManager.AbilityActive[i] == true)
                {
                    Script.enabled = true;

                    //Debug.Log("EnableDash true");
                }
                else
                {
                    Script.enabled = false;

                    //Debug.Log("EnableDash false");
                }
            }
        }
    }

    void TaskOnClick()
    {
        // There should be a way to not need to save all save data for this.
        // Without this abilityManager.SaveInventoryName[i] is not set to a name yet.
        DataPersistenceManager.instance.SaveGame();

        // Checking if There is Dash in the inventory and then setting i active/inactive bool list to true.
        Script = FindAnyObjectByType<Dash>();
        abilityManager = FindAnyObjectByType<AbilityManager>();
        if (Script.enabled == false)
        {
            for (int i = 0; i < abilityManager.SaveInventorySlotIndex.Length; i++)
            {
                if (abilityManager.SaveInventoryName[i] == "ImageDash(Clone)")
                {
                    abilityManager.AbilityActive[i] = true;

                    Script.enabled = true;

                    //Debug.Log("DashScript enabled");

                    EnableDash();

                    //Debug.Log("AbilityActiveTrue: " + abilityManager.AbilityActive[i]);
                }
                else
                {
                    //Debug.Log("Not Correct Item: " + abilityManager.SaveInventoryName[i]);
                }
            }
        }
        else
        {
            for (int i = 0; i < abilityManager.SaveInventorySlotIndex.Length; i++)
            {
                if (abilityManager.SaveInventoryName[i] == "ImageDash(Clone)")
                {
                    abilityManager.AbilityActive[i] = false;

                    Script.enabled = false;
                   
                    //Debug.Log("AbilityActiveFalse: " +  abilityManager.AbilityActive[i]);
                }
                else
                {
                    //Debug.Log("Not Correct Item: " + abilityManager.SaveInventoryName[i]);
                }
            }
        }
    }
}
