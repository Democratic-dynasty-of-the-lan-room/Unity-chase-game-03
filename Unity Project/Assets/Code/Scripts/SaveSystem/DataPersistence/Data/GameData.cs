using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering.LookDev;

[System.Serializable]
public class GameData
{
    //public int HealthAmount;

    public int JumpCount;
    public int DeathCount;
    public int PosToSpawn;
    public int SceneNumber;
    public int PlayerHealth;

    public bool CanSpawn;

    public Vector3 playerPosition;
    //public Vector3 playerRotation;
    //public Quaternion PlayerRotation;
    //public Quaternion PlayerOrientation;

    public Vector3 BlockPosition;
    public Quaternion BlockRotation;

    public float YRotation;
    public float XRotation;

    // Inventory
    public bool[] InventorySlots;
    public List<GameObject> InInventory;

    // Abilities Marked with A, and Inventory IN:
    //public bool ADash;
    //public bool INDash;
    //public string TestStringSave;
    //public bool ADashInventory;

    //public List<string> ItemName;
    //public List<int> SlotIndex;

    public int[] SlotIndexArray = new int[3];
    public string[] ItemNameArray;
    public bool[] AbilityActivated;

    //public List<int> InitSlotCount;

    //public bool DashEquiped;

    public bool equipedDash;

    //public List<GameObject> InventoryGameObjects;

    //the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.JumpCount = 0;
        this.DeathCount = 0;
        this.PosToSpawn = 0;
        this.SceneNumber = 1;
        this.PlayerHealth = 100;

        this.CanSpawn = true;

        playerPosition = Vector3.zero;
        //playerRotation = Vector3.zero;
        // not sure what this does
        //PlayerRotation = Quaternion.identity;
        //PlayerOrientation  = Quaternion.identity;

        BlockPosition = Vector3.left;
        BlockRotation = Quaternion.identity;

        YRotation = 0;
        XRotation = 0;

        InInventory = null;

        // Abilites
        //ADash = false;
        //INDash = false;
        //TestStringSave = string.Empty;
        //ADashInventory = false;

        ItemNameArray = new string[3];
        AbilityActivated = new bool[3];

        //slots = new SlotIndex[3];

        //SlotIndex = InitSlotCount;
        //SlotIndex = InitSlotCount;

        //DashEquiped = false;
        equipedDash = false;
    }
}