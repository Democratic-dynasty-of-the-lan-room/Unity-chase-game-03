using Code.Scripts.SampleScene.Player;
using NUnit.Framework;
using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class PickUpObjects : MonoBehaviour
    {
        public GameObject itemButton;

        // maybe have a bool for each instantiated ItemButton.

        // I had it referenved wifh findGameObject with tag. But it wasn't finding it after the update. PlayerGameobject ref
        public InventoryScript inventory;

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
