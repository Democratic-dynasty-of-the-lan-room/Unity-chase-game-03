using UnityEngine;
using System.Collections;

public class Enemy : MonoBehaviour
{

    public float AttackDistance;
    [Range(0,360)]
    public float Angle;

    public Vector3 DirFromAngle(float angleInDegrees)
    {
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad),0,Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
}
