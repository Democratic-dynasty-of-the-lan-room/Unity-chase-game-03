using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class ParasiteShooter : MonoBehaviour
{
    [SerializeField] GameObject MovementPlayer;

    [SerializeField] GameObject BulletPrefabOBj;

    public float ViewRange;

    public float ReloadTime;

    private IEnumerator coroutine;

    public bool CanRunShoot;

    void Start()
    {
        CanRunShoot = true;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (Vector3.Distance(MovementPlayer.transform.position, this.transform.position) < ViewRange)
        {
            transform.LookAt(MovementPlayer.transform.position);

            coroutine = Shoot(ReloadTime);

            if (CanRunShoot)
            {
                StartCoroutine(coroutine);

                CanRunShoot = false;
            }
        }
    }

    private IEnumerator Shoot(float ReloadTime)
    {
        yield return new WaitForSeconds(ReloadTime);

        //print("ShootTime" + Time.time);

        Instantiate(BulletPrefabOBj, this.transform.position + new Vector3(transform.forward.x, transform.forward.y, transform.forward.z), Quaternion.LookRotation(this.transform.forward));

        CanRunShoot = true;
    }


    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, ViewRange);
    }
}
