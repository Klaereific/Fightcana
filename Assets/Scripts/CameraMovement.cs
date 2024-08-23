using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player1; // Reference to the first player
    public Transform player2; // Reference to the second player

    public float minZoom = 5f; // Minimum camera zoom level
    public float maxZoom = 10f; // Maximum camera zoom level
    public float zoomLimiter = 10f; // Zoom factor to control the zoom speed

    public Vector3 offset; // Offset to adjust the camera position

    private Camera cam; // Reference to the camera

    void Start()
    {
        cam = Camera.main; // Get the main camera
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        Move(); // Update the camera position
        Zoom(); // Update the camera zoom
    }

    void Move()
    {
        // Calculate the midpoint between the two players
        float midpoint_x = (player1.position.x + player2.position.x) / 2f;

        // Set the camera's position to the midpoint plus any desired offset
        Vector3 newPos = new Vector3();
        newPos.x = midpoint_x;
        transform.position = newPos + offset;
    }

    void Zoom()
    {
        // Calculate the distance between the two players
        float distance = Mathf.Sqrt(Mathf.Pow(player1.position.x - player2.position.x,2));

        float zoomFactor = (distance - minZoom * 2) / (maxZoom * 2 - minZoom * 2);

        // Determine the appropriate zoom level based on the distance
        float zoom = Mathf.Lerp(minZoom, maxZoom, zoomFactor);
        //Debug.Log(distance);
        // Set the camera's orthographic size (zoom level)
        cam.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
    }
}
