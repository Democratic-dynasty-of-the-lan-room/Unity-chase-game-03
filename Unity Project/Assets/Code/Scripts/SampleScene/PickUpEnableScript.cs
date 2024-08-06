using Unity.VisualScripting;
using UnityEngine;

public class PickUpEnableScript : MonoBehaviour
{
    [SerializeField] GameObject EToPressLever;

    public float rayLength = 3;

    PlayerInput playerInput;

    private void Awake()
    {
        playerInput = new PlayerInput();

        EToPressLever.SetActive(false);
    }
    private void OnEnable()
    {
        playerInput.Enable();
    }

    private void OnDisable()
    {
        playerInput.Disable();
    }

    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, rayLength))
        {
            var HitTransform = hit.transform;
            if (HitTransform.CompareTag("Selectable"))
            {
                EToPressLever.SetActive(true);

                if (playerInput.GamePlay.Interact.triggered)
                {
                    hit.collider.transform.GetChild(0).gameObject.SetActive(true);
                    //Debug.Log("AddHealth. Destroy object");
                }
            }
            else
            {
                EToPressLever.SetActive(false);
            }
        }
    }
}
