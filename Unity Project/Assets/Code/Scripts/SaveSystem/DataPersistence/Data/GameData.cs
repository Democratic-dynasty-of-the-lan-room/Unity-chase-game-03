using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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



    }
}
