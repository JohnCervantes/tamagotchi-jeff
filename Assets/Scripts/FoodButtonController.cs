using UnityEngine;
using UnityEngine.UI;

public class FoodButtonController : MonoBehaviour
{
    public GameObject boneGO; // Drag your DraggableBone GameObject here
    public Transform spawnPosition; // Where the bone appears (optional)

    void Start()
    {
        // Get the Button component and add click listener
        Button button = GetComponent<Button>();
        button.onClick.AddListener(OnFoodButtonClicked);
    }

    void OnFoodButtonClicked()
    {
        Debug.Log("Food button clicked!");

        // Show the bone
        boneGO.SetActive(true);

        // Position it near the button or at spawn position
        if (spawnPosition != null)
        {
            boneGO.transform.position = spawnPosition.position;
        }
        else
        {
            // Default: spawn near the button (in world space)
            Vector3 buttonWorldPos = Camera.main.ScreenToWorldPoint(transform.position);
            buttonWorldPos.z = 0; // Make sure it's at z=0 for 2D
            boneGO.transform.position = buttonWorldPos;
        }
    }
}