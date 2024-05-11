using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class SpeedDash : MonoBehaviour
{
    //private GameObject Player;

    Dash Script;


    public Button yourButton;

    private void Awake()
    {
        //Player = GameObject.FindWithTag("Player");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        Button btn = yourButton.GetComponent<Button>();
        btn.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        Script = FindAnyObjectByType<Dash>();

        //Script = Player.GetComponentInChildren<Dash>();

        Debug.Log("You have clicked the button!");

        if (Script.enabled == false)
        {
            Script.enabled = true;
        }
        else
        {
            Script.enabled = false;
        }
    }
}
