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

    [Header("RestartLevel")]

    public Vector3 RestartPosition;

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

    // Runs when you click Restart/Succumb
    public void RestartLevel()
    {
        // Enemy reset position

        // player reset position how I did it now is bad
        playerPosition = Vector3.zero;

        // reset objects in the level if you collected them in this level and reset, but don't reset an object if you are visiting this place again
    }
}
