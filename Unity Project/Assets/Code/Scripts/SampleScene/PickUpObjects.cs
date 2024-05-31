using Code.Scripts.SampleScene.Player;
using NUnit.Framework;
using NUnit.Framework.Internal;
using Unity.VisualScripting;
using UnityEngine;

namespace Code.Scripts.SampleScene
{
    public class PickUpObjects : MonoBehaviour, IDataPersistence
    {
        public GameObject itemButton;

        public Transform test;
        public string testName;
        //public GameObject TestGameObject;

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

                    //test = inventory.Handslots[i].transform;
                    //testName = itemButton.name;
                    //TestGameObject = itemButton.GameObject;

                    //Debug.Log("Test: " + test + "Item Name: " + testName);

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

        public void TestInstantiates()
        {
            //Instantiate(itemButton, inventory.slots[test].transform, false);
        }

        public void LoadData(GameData data)
        {
          
        }

        public void SaveData(ref GameData data)
        {
            testName = data.TestStringSave;
        }

        public void RestartLoadData(CheckPointData CheckPointLoadData)
        {
       
        }

        public void RestartSaveData(ref CheckPointData CheckPointSaveData)
        {
     
        }
    }
}
