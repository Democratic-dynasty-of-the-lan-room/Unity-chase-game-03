using UnityEngine;

namespace Code.Scripts.SampleScene.Player
{
    public class MoveCamera : MonoBehaviour
    {
        public Transform cameraPosition;

        public void Update()
        {
            transform.position = cameraPosition.position;
        }
    }
}