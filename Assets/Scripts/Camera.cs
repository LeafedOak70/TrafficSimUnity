using UnityEngine;

public class CameraDrag : MonoBehaviour
{
    private Vector3 dragOrigin;
    private float zoomSpeed = 4.0f;
    private float minZoom = 2.0f;
    private float maxZoom = 30.0f;

    void Update()
    {
        HandleInput();
    }

    void HandleInput()
    {
        Camera camera = GetComponent<Camera>();
        if (Input.GetMouseButtonDown(0))
        {
            dragOrigin = camera.ScreenToWorldPoint(Input.mousePosition);
        }

        if (Input.GetMouseButton(0))
        {
            Vector3 difference = dragOrigin - camera.ScreenToWorldPoint(Input.mousePosition);
            transform.position += difference;
        }

        float scroll = Input.GetAxis("Mouse ScrollWheel");
        camera.orthographicSize = Mathf.Clamp(camera.orthographicSize - scroll * zoomSpeed, minZoom, maxZoom);
    }
}