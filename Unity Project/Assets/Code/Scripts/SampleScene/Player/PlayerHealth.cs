using Code.Scripts.SampleScene.MenuScripts;
using System.Xml;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public int Health;

    [Header("Warning this doesn't work Completely yet. Im not sure how to set the default GameData to SetHealthAmount")]
    public int SetHealthAmount;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject PauseMenu;

    [SerializeField] GameObject InventoryScript;

    public GameObject RestartMenu;

    public GameData gameData;

    public bool OverideDeath;

    [Header("Check what health is with H")]
    public bool ShowHealthWithH;

    private void Awake()
    {
        //Time.timeScale = 1.0f;
    }

    private void FixedUpdate()
    {     
        if (Health <= 0 && !OverideDeath)
        {
            //Debug.Log("Health in death" + Health);

            // We probably want this. Just need to find a good place to set it to 1o again. That works with other scripts.
            //Time.timeScale = 0f;

            RestartMenu.SetActive(true);
            // setting the player to false so that you can't move after the end
            
            Player.SetActive(false);
            //Enabling this
            this.enabled = false;

            //Showscursor so that you can click restart
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            PauseMenu.SetActive(false);
            InventoryScript.SetActive(false);
        }  
        
        if (Input.GetKeyDown(KeyCode.H) && ShowHealthWithH)
        {
            Debug.Log("Health = " + Health);
        }
    }


    public void LoadData(GameData data)
    {
        Health = data.PlayerHealth;
    }

    public void SaveData(ref GameData data)
    {
        data.PlayerHealth = Health;
    }

    public void RestartLoadData(CheckPointData CheckPointLoadData)
    {
        
    }

    public void RestartSaveData(ref CheckPointData CheckPointSaveData)
    {
       
    }
}
