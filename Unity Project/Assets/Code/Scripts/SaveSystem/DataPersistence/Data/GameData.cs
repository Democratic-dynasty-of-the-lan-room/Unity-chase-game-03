using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int JumpCount;
    public int DeathCount;
    public Vector3 playerPosition;
    public Quaternion PlayerRotation;
    public Quaternion PlayerOrientation;

    public Vector3 BlockPosition;
    public Quaternion BlockRotation;

    //the values defined in this constructor will be the default values
    // the game starts with when there's no data to load
    public GameData()
    {
        this.JumpCount = 0;
        this.DeathCount = 0;
        playerPosition = Vector3.zero;
        // not sure what this does
        PlayerRotation = Quaternion.identity;
        PlayerOrientation  = Quaternion.identity;

        BlockPosition = Vector3.left;
        BlockRotation = Quaternion.identity;
    }
}
