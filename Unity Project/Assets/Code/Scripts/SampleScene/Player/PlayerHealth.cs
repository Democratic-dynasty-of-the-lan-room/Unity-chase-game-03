using Code.Scripts.SampleScene.MenuScripts;
using UnityEngine;
using UnityEngine.AI;

public class PlayerHealth : MonoBehaviour, IDataPersistence
{
    public int Health;

    [SerializeField] GameObject Player;

    [SerializeField] GameObject PauseMenu;

    [SerializeField] GameObject InventoryScript;

    public GameObject RestartMenu;

    public bool OverideDeath;

    private void Update()
    {     
        if (Health <= 0 && !OverideDeath)
        {
            Health = 100;
         
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
