using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class PickUpScript : MonoBehaviour
    {
        private InventoryScript inventory;

        [SerializeField] private string selectableTag = "Selectable";
        [SerializeField] private string HandTag = "Handtag";

        [SerializeField] GameObject PressEToPickUp;

        public float rayLength;

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
                
                    if (Input.GetKeyDown(KeyCode.E))
                    {
                        PickUpObjects pickUpObjects = hit.collider.gameObject.GetComponent<PickUpObjects>();

                        if (pickUpObjects != null)
                        {                      
                            //call function from pickupobjects script
                            pickUpObjects.Instanciates();
                            PressEToPickUp.SetActive(false);
                        }                  
                    }                         
                } //For things held in hand, probably a bad way to do this
                else if (selection.CompareTag(HandTag))
                {
                    PressEToPickUp.SetActive(true);

                    if (Input.GetKeyDown(KeyCode.E))
                    {

                    
                        PickUpObjects pickUpObjects = hit.collider.gameObject.GetComponent<PickUpObjects>();

                        if (pickUpObjects != null)
                        {
                            //call function from pickupobjects script
                            pickUpObjects.Hand();
                            PressEToPickUp.SetActive(false);
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
    }
}
