using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CheckPointData
{
    public Vector3 RestartplayerPosition;
    //public Quaternion RestartPlayerRotation;
    //public Quaternion RestartPlayerOrientation;

    // Restart Data
    public CheckPointData()
    {
        RestartplayerPosition = new Vector3(0,3,0);
        
        //RestartPlayerRotation = Quaternion.identity;

        //RestartPlayerOrientation = Quaternion.identity;
    }
}
