using NUnit.Framework.Internal;
using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class PickUpScript : MonoBehaviour, IDataPersistence
    {
        private InventoryScript inventory;

        [SerializeField] private string selectableTag = "Selectable";
        [SerializeField] private string HandTag = "Handtag";

        [SerializeField] GameObject PressEToPickUp;

        [SerializeField] SpeedDash speedDash;

        //public GameObject[] SaveInventory;

        public float rayLength;

        PlayerInput playerInput;

        private void Awake()
        {
            playerInput = new PlayerInput();
        }

        private void OnEnable()
        {
            playerInput.Enable();
        }
        private void OnDisable()
        {
            playerInput.Disable();
        }

        // Start is called before the first frame update
        private void Start()
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();

            PressEToPickUp.SetActive(false);
        }



        //Update is called once per frame
        //Checking if Player is picking up or viewing an object that can be picked up using raycast and checking slots
        void Update()
        {
        
            var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, rayLength))
            {

                var selection = hit.transform;
                if (selection.CompareTag(selectableTag))
                {
                
                    PressEToPickUp.SetActive(true);
                
                    if (playerInput.GamePlay.Interact.triggered)
                    {                       
                        PickUpObjects pickUpObjects = hit.collider.gameObject.GetComponent<PickUpObjects>();

                        if (pickUpObjects != null)
                        {
                            //call function from pickupobjects script
                            pickUpObjects.Instanciates();
                            PressEToPickUp.SetActive(false);

                            // Lol what does this do
                            //SaveInventory[inventory.slots.Length] = pickUpObjects.itemButton;
                        }                  
                    }                         
                } //For things held in hand, probably a bad way to do this
                else if (selection.CompareTag(HandTag))
                {
                    PressEToPickUp.SetActive(true);

                    if (playerInput.GamePlay.Interact.triggered)
                    {

                    
                        PickUpObjects pickUpObjects = hit.collider.gameObject.GetComponent<PickUpObjects>();

                        if (pickUpObjects != null)
                        {
                            //Debug.Log("PickUpScript Hand before");

                            //call function from pickupobjects script
                            pickUpObjects.Hand();
                            PressEToPickUp.SetActive(false);

                            //Debug.Log("PickUpScript Hand after");
                        }
                    }
                }
                else
                {
                    PressEToPickUp.SetActive(false);
                }
            }
            else
            {
                PressEToPickUp.SetActive(false);
            }           
        }

        public void LoadData(GameData data)
        {
           
        }

        public void SaveData(ref GameData data)
        {
           
        }

        public void RestartLoadData(CheckPointData CheckPointLoadData)
        {
          
        }

        public void RestartSaveData(ref CheckPointData CheckPointSaveData)
        {
         
        }

        /*public void loadTheInventoryquestion() 
        {
            for (int i = 0; i < inventory.slots.Length; i++)
            {
                //pickUpObjects.Instantiates();
            }
        } */
    }
}
