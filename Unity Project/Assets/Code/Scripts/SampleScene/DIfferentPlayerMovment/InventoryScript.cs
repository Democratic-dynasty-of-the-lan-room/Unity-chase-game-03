using System.Collections.Generic;
using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class InventoryScript : MonoBehaviour
    {
        public bool[] isFull;
        public GameObject[] slots;

        public bool[] HandisFull;
        public GameObject[] Handslots;

        public List<InventoryScript> scripts;

        public static bool InventoryIsOpen = false;

        [SerializeField] GameObject Inventory;
        [SerializeField] GameObject pauseMenu;

        // Start is called before the first frame update
        void Start()
        {
            this.enabled = true;
        }

        // Update is called once per frame
        void Update()
        {
            //Opens and closes inventory
            if (Input.GetKeyDown(KeyCode.Tab))
            {
                if (InventoryIsOpen)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        void Resume()
        {

            Inventory.SetActive(false);

            // Hides and lock cursor
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;

            pauseMenu.SetActive(true);

            Time.timeScale = 1f;
            InventoryIsOpen = false;


        }

        void Pause()
        {

            Inventory.SetActive(true);

            pauseMenu.SetActive(false);

            // Shows and unlocks cursor
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            Time.timeScale = 0f;
            InventoryIsOpen = true;
        }
    }
}
