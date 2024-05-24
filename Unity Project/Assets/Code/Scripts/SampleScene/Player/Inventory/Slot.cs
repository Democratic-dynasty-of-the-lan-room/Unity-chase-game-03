using UnityEngine;

public class Slot : MonoBehaviour
{
    public void DropItem()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }
}
