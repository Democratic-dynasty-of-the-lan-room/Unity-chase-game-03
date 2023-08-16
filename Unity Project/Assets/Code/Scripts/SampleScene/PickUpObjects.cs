using Code.Scripts.SampleScene.Player;
using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class PickUpObjects : MonoBehaviour
    {

        public GameObject itemButton;

        private InventoryScript inventory;   

        // Start is called before the first frame update
        void Start()
        {
            inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //function to be called in pickupscript. Instanciates button in inventory and prefab?
        public void Instanciates()
        {
        
            for (int i = 0; i < inventory.slots.Length; i++)
            {
            
                if (inventory.isFull[i] == false)
                {
                
                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(this.gameObject);

                    inventory.isFull[i] = true;         

                    break;
                }
            }
        }

        //Function called for any object with handtag that is picked up
        public void Hand()
        {
            for (int i = 0; i < inventory.Handslots.Length; i++)
            {
   
                if (inventory.HandisFull[i] == false)
                { 

                    Instantiate(itemButton, inventory.Handslots[i].transform, false);
                    Destroy(gameObject);
   
                    inventory.HandisFull[i] = true;

                    break;
                }
            }
        }

    }
}
