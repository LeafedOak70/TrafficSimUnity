using UnityEngine;

public class FollowCamera : MonoBehaviour
{
    public Camera targetCamera;
    public float canvasDistance = 10f; // Distance from the camera
    public float canvasWidth = 10f; // Adjust as needed

    private RectTransform canvasRect;

    private void Start()
    {
        if (targetCamera == null)
        {
            Debug.LogError("Target camera not assigned!");
            return;
        }

        // Get the RectTransform component of the Canvas
        canvasRect = GetComponent<RectTransform>();

        // Set the initial Canvas size and position
        UpdateCanvas();
    }

    private void Update()
    {
        if (targetCamera == null)
        {
            Debug.LogError("Target camera not assigned!");
            return;
        }

        // Update the Canvas size and position based on the camera's changes
        UpdateCanvas();
    }

    private void UpdateCanvas()
    {
        // Set Canvas position in front of the camera
        transform.position = targetCamera.transform.position + targetCamera.transform.forward * canvasDistance;

        // Set Canvas rotation to match the camera's rotation
        transform.rotation = targetCamera.transform.rotation;

        // Set Canvas size based on the camera's aspect ratio
        float aspectRatio = targetCamera.aspect;
        float canvasHeight = canvasWidth / aspectRatio;

        // Set Canvas size
        canvasRect.sizeDelta = new Vector2(canvasWidth, canvasHeight);
    }
}
