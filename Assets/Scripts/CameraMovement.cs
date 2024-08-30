using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player1; // Reference to the first player
    public Transform player2; // Reference to the second player

    public float minCamDist = 5f; // Minimum camera zoom level
    public float maxCamDist = 10f; // Maximum camera zoom level
    public float zoomLimiter = 10f; // Zoom factor to control the zoom speed
    public float pad = 0.5f;
    public float CamDist;
    public float distance;
    public float VerticalPoV;
    public float initialZposition;
    public float initialHeight;
    public Vector3 offset; // Offset to adjust the camera position

    private Camera cam; // Reference to the camera

    

    void Start()
    {
        cam = Camera.main; // Get the main camera

        VerticalPoV = 2 * Mathf.Atan(Mathf.Tan(cam.fieldOfView * Mathf.Deg2Rad / 2) / cam.aspect) * Mathf.Rad2Deg;
        initialZposition = offset.z;
        initialHeight = Mathf.Abs(initialZposition) * Mathf.Tan(VerticalPoV * Mathf.Deg2Rad / 2);
    }

    void LateUpdate()
    {
        if (player1 == null || player2 == null)
            return;

        Move(); // Update the camera position
        // Zoom(); // Update the camera zoom
    }

    void Move()
    {
        // Calculate the midpoint between the two players
        float midpoint_x = (player1.position.x + player2.position.x) / 2f;

        // Set the camera's position to the midpoint plus any desired offset
        Vector3 newPos = new Vector3();
        newPos.x = midpoint_x;

        distance = Mathf.Sqrt(Mathf.Pow(player1.position.x - player2.position.x, 2));

        float TanHalfFOV = Mathf.Tan(Mathf.Deg2Rad*(cam.fieldOfView / 2));

        CamDist = ((distance + pad) / 2) / TanHalfFOV;

        offset.z = - Mathf.Clamp(CamDist, minCamDist, maxCamDist);

        offset.y = initialHeight * (offset.z / initialZposition);

        transform.position = newPos + offset;
    }
    /*
    void Zoom()
    {
        // Calculate the distance between the two players
        float distance = Mathf.Sqrt(Mathf.Pow(player1.position.x - player2.position.x,2));
    
        offset.z = ((distance+pad)/2)/Mathf.Tan(cam.fieldOfView/2)

        // Determine the appropriate zoom level based on the distance
        float zoom = Mathf.Lerp(min_FoV, max_FoV, FoV);
        //Debug.Log(distance);
        // Set the camera's orthographic size (zoom level)
        //cam.orthographicSize = Mathf.Clamp(zoom, minZoom, maxZoom);
        cam.fieldOfView = Mathf.Clamp(FoV, min_FoV, max_FoV);
    }
    */
}
