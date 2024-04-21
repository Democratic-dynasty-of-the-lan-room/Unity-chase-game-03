using Code.Scripts.SampleScene.Player;
using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class PickUpObjects : MonoBehaviour
    {
        public GameObject itemButton;

        // I had it referenved wifh findGameObject with tag. But it wasn't finding it after the update. PlayerGameobject ref
        public InventoryScript inventory;

        // Start is called before the first frame update
        void Start()
        {
            //inventory = GameObject.FindGameObjectWithTag("Player").GetComponent<InventoryScript>();
        }

        // Update is called once per frame
        void Update()
        {
        
        }

        //function to be called in pickupscript. Instanciates button in inventory and prefab?
        public void Instanciates()
        {

            Debug.Log("Instantiates");

            for (int i = 0; i < inventory.slots.Length; i++)
            {

                Debug.Log("intentory Slots");

                if (inventory.isFull[i] == false)
                {
                    Debug.Log("Instantiate intentory before");


                    Instantiate(itemButton, inventory.slots[i].transform, false);
                    Destroy(this.gameObject);

                    Debug.Log("Instantiate intentory after");

                    inventory.isFull[i] = true;         

                    break;
                }
            }
        }

        //Function called for any object with handtag that is picked up
        public void Hand()
        {
            //Debug.Log("Hand");

            for (int i = 0; i < inventory.Handslots.Length; i++)
            {
                //Debug.Log("Hand slots");


                if (inventory.HandisFull[i] == false)
                {
                    //Debug.Log("Instantiate Hand before");

                    Instantiate(itemButton, inventory.Handslots[i].transform, false);

                    //Debug.Log("Instantiate Hand mid");

                    Destroy(gameObject);

                    //Debug.Log("Instantiate Hand after");

                    inventory.HandisFull[i] = true;

                    break;
                }
            }
        }

    }
}
