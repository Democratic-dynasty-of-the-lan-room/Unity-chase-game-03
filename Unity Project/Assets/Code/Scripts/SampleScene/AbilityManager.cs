using Code.Scripts.SampleScene.Player;
using JetBrains.Annotations;
using NUnit.Framework.Internal;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using static Unity.VisualScripting.Member;

public class AbilityManager : MonoBehaviour, IDataPersistence
{
    [SerializeField] Dash dash;

    SpeedDash script;

    [SerializeField] InventoryScript inventory;

    [SerializeField] GameObject ImageDash;
    [SerializeField] GameObject ImageSpeedBoost;
    [SerializeField] GameObject ImageWall;

    public bool SetDashInventory;

    //public List<GameObject> SaveInventory; OLD
    //public List<string> SaveInventoryName; OLD

    public string[] SaveInventoryName;
    public int[] SaveInventorySlotIndex;
    public bool[] AbilityActive;

    public List<int> InitializeSlotCount;

    public Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    public Transform parentTransform;

    private bool LoadInventoryFixedUpdate;

    public bool DashingAble;

    private void Awake()
    {
        prefabDictionary.Add("ImageDash", ImageDash);
        prefabDictionary.Add("ImageSpeedBoost", ImageSpeedBoost);
        prefabDictionary.Add("ImageWall", ImageWall);

        LoadInventoryFixedUpdate = false;
    }

    public void SaveInventoryQuestion()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Transform slot = inventory.slots[i].transform;

            if (slot.childCount > 0)
            {
                GameObject itemObj = slot.GetChild(0).gameObject;

                //Save name of Object to reference when instantiating it later.
                SaveInventoryName[i] = itemObj.gameObject.name;

                SaveInventorySlotIndex[i] = i;
            }
            else if (slot.childCount == 0)
            {
                SaveInventoryName[i] = "";
                //Debug.Log("SaveInventoryName = nothing");
            }
            else if (slot == null)
            {
                //Debug.Log("Slot = null");
            }
        }

        //IsAbilityActive();
    }

    public void LoadInventoryQuestion()
    {

        if (SaveInventoryName == null)
        {
            //SaveInventorySlotIndex = InitializeSlotCount;

            //Debug.Log("Initializing InventoryName");
        }
        //Debug.Log("RunLoadInventory");

        // Check if this is working correctly!
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            //Debug.Log("ForLoop");

            //Debug.Log("ForeachChild");
            if (inventory.slots[i].transform.childCount > 0)
            {
                //Debug.Log("DestroyChildren");
                Destroy(inventory.slots[i].transform.GetChild(0).gameObject);
            }
            else
            {
                //Debug.Log("There was no inventory to destroy");
            }
        }
       
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            // This is not quite ideal because it will remove any part of a name that has clone in it. Which means it won't work for objects that happen to have clone in the name already.
            string SlotName = SaveInventoryName[i].Replace("(Clone)", "");

            //Debug.Log(SlotName);
      
            if (SlotName != "")
            {
                //Debug.Log("SlotName isn't null");

                GameObject instance = Instantiate(Resources.Load("Inventory UI/" + SlotName, typeof(GameObject))) as GameObject;

                Transform slotTransform = inventory.slots[i].transform;

                
                if (instance != null)
                {
                    // Inventory Slot set to full
                    inventory.isFull[i] = true;

                    // sets the instance to be a parent of sloTransform
                    instance.transform.SetParent(slotTransform);

                    // sets the instance's position to be 0 0 0.
                    instance.transform.localPosition = new Vector3(0, 0, 0);

                    // I didn't set the scale before which mean't you couldn't see the button lol!
                    instance.transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    //Debug.LogError("Failed to load and instantiate the GameObject.");
                }
            }
            else
            {
                //Debug.Log("Slots to load are empty");
            }
        }

        //IsAbilityActive();
    }

    // This is an idea of how to save the the abilities state of being active or inactive.
    public void IsAbilityActive()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Transform slot = inventory.slots[i].transform;

            if (slot.childCount > 0)
            {
                GameObject itemObj = slot.GetChild(0).gameObject;

                if (itemObj.gameObject.activeSelf == true)
                {
                    AbilityActive[i] = true;
                    
                    //itemObj.gameObject.SetActive(true); Doesn't work because it's checking the game object not the script component.

                    Debug.Log("AbilityActive: " + AbilityActive[i]);
                }
                else
                {
                    AbilityActive[i] = false;

                    itemObj.gameObject.SetActive(false);

                    Debug.Log("AbilityActiveFalse?: " + AbilityActive[i]);
                }
            }
            else
            {
                Debug.Log("NoChildren AbilityActive");
            }
        }
    }

    private void FixedUpdate()
    {
        if (LoadInventoryFixedUpdate)
        {
            LoadInventoryQuestion();

            LoadInventoryFixedUpdate = false;
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    //Remove Later as well I think
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        LoadInventoryFixedUpdate = true;
    }

    private void Update()
    {
        /*if (Input.GetKeyDown(KeyCode.I))
        {
            //LoadInventoryQuestion("ImageDash", 0);
            //LoadInventoryQuestion("AnotherPrefab", 1);

            LoadInventoryQuestion();

            LoadInventoryFixedUpdate = true;
        }

        if (Input.GetKeyDown(KeyCode.H))
        {
            SaveInventoryQuestion();
        }
        */

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(DashingAble);
        }
    }

    public void LoadData(GameData data)
    {
        SaveInventoryName = data.ItemNameArray;
        SaveInventorySlotIndex = data.SlotIndexArray;

        AbilityActive = data.AbilityActivated;

        DashingAble = data.equipedDash;
    }

    public void SaveData(ref GameData data)
    {
        data.ItemNameArray = SaveInventoryName;
        data.SlotIndexArray = SaveInventorySlotIndex;

        data.InitSlotCount = InitializeSlotCount;

        data.AbilityActivated = AbilityActive;

        // Working but Ok?
        data.equipedDash = DashingAble;

        // Bad Place to Save the Inventory from?
        Debug.Log("ErrorHere?");
        SaveInventoryQuestion();
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
