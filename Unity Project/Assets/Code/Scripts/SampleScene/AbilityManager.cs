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

    //[SerializeField] SpeedDash speedDash;

    SpeedDash script;

    [SerializeField] InventoryScript inventory;

    [SerializeField] GameObject ImageDash;
    [SerializeField] GameObject ImageSpeedBoost;
    [SerializeField] GameObject ImageWall;

    //[SerializeField] InventoryScript inventoryScript;

    //[SerializeField] SpeedDash speedDash;

    public bool SetDashInventory;

    //public List<GameObject> SaveInventory; OLD
    //public List<string> SaveInventoryName;

    public string[] SaveInventoryName;
    public int[] SaveInventorySlotIndex;

    public List<int> InitializeSlotCount;

    //private int InitSlotAmount = 3;

    //public List<GameObject> SaveInventoryGameObject;

    //private Dictionary<string, GameObject> prefabDictionary;

    //public List<GameData> items = new List<GameData>();

    public Dictionary<string, GameObject> prefabDictionary = new Dictionary<string, GameObject>();

    public Transform parentTransform;

    private bool LoadInventoryFixedUpdate;

    private void Awake()
    {
        prefabDictionary.Add("ImageDash", ImageDash);
        prefabDictionary.Add("ImageSpeedBoost", ImageSpeedBoost);
        prefabDictionary.Add("ImageWall", ImageWall);

        LoadInventoryFixedUpdate = false;

        //SaveInventorySlotIndex.Add(3);
    }

    public void SaveInventoryQuestion()
    {
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Transform slot = inventory.slots[i].transform;
            //Transform slotPos = inventory.slots[i];

            if (slot.childCount > 0)
            {
                //print("ChildCount" + slot.childCount);

                GameObject itemObj = slot.GetChild(0).gameObject;

                //Save name of Object to reference when instantiating it later.
                SaveInventoryName[i] = itemObj.gameObject.name;

                //Debug.Log("ItemObject Name: " + itemObj.name);

                SaveInventorySlotIndex[i] = i;

                // This is unneccesarry and will propably be removed. I was just curious.
                //SaveInventoryGameObject[i] = itemObj.gameObject;

                //Instantiate(itemObj, slot.GetChild(i).transform);

                // Do I need to Get the parent so that I can Child the objects properly?

                //Transform SlotTest = slot[i];

                //SaveInventorySlotIndex = itemObj.IndexOf(i);

                //itemObj = SaveInventory[itemObj.GetInstanceID()];

                //Debug.Log("SaveInventory: " + SaveInventory);

                //Debug.Log(slot.gameObject.name);                  
                //Debug.Log("Slot");
                // we have children!
            }
            else if (slot.childCount == 0)
            {
                SaveInventoryName[i] = "";
                Debug.Log("SaveInventoryName = nothing");
            }
            else if (slot == null)
            {
                Debug.Log("Slot = null");
            }
        }
    }

    public void InstantiatePrefabByName(string prefabName, Transform parent)
    {
    
    }

    public void LoadInventoryQuestion()
    {

        if (SaveInventoryName == null)
        {
            //SaveInventorySlotIndex = InitializeSlotCount;

            Debug.Log("Initializing InventoryName");
        }
        //Debug.Log("RunLoadInventory");

        // Check if this is working correctly!
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            Debug.Log("ForLoop");

            //Debug.Log("ForeachChild");
            if (inventory.slots[i].transform.childCount > 0)
            {
                //print("ChildCount" + inventory.slots[i].transform.childCount);

                Debug.Log("DestroyChildren");
                Destroy(inventory.slots[i].transform.GetChild(0).gameObject);
                //Debug.Log("There is an error: Transform child out of bounds for Image speed boost?");
            }
            else
            {
                //print("ChildCount" + inventory.slots[i].transform.childCount);

                Debug.Log("There was no inventory to destroy");
            }
        }
       
        for (int i = 0; i < inventory.slots.Length; i++)
        {
            // This is not quite ideal because it will remove any part of a name that has clone in it. Which means it won't work for objects that happen to have clone in the name already.
            string SlotName = SaveInventoryName[i].Replace("(Clone)", "");

            Debug.Log(SlotName);
      
            if (SlotName != "")
            {
                Debug.Log("SlotName isn't null");

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
                    Debug.LogError("Failed to load and instantiate the GameObject.");
                }
            }
            else
            {
                Debug.Log("Slots to load are empty");
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
        //SceneManager.sceneUnloaded += OnSceneUnloaded;
    }

    //Remove Later as well I think
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        //SceneManager.sceneUnloaded -= OnSceneUnloaded;
    }

    public void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {

        /*// Instantiate inventory buttons. In the at the correct slot transform.
        if (SetDashInventory)
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                Instantiate(ItemButton, inventory.slots[i].transform, false);
            }
        }*/

        //LoadInventoryFixedUpdate = true;

        //Debug.Log("LoadInventory");
    }

    public void Dash()
    {
        if (dash.DashEnabled)
        {
            dash.enabled = true;

            if (script.DashEquiped)
            {
                //script.DashEquiped = true;
            }

            // TODO - save intventory slot
        }
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
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
    }

    /*public void LoadInventory()
    {
        script = FindAnyObjectByType<SpeedDash>();


        for (int i = 0; i < inventory.Handslots.Length; i++)
        {
            if (script.DashEquiped)
            {
                Instantiate(script.gameObject, inventory.Handslots[i].transform, false);
            }
        }    
    }*/

    public void LoadData(GameData data)
    {
        SaveInventoryName = data.ItemNameArray;
        SaveInventorySlotIndex = data.SlotIndexArray;
    }

    public void SaveData(ref GameData data)
    {
        data.ItemNameArray = SaveInventoryName;
        data.SlotIndexArray = SaveInventorySlotIndex;

        data.InitSlotCount = InitializeSlotCount;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
