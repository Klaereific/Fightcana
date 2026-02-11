using UnityEngine;

public class CheckInput : MonoBehaviour
{
    void Update()
    {
        float horizontal = Input.GetAxis("Horizontal"); // or the name of your axis
        float vertical = Input.GetAxis("Vertical"); // or the name of your axis
        Debug.Log($"Horizontal: {horizontal}, Vertical: {vertical}");
    }
}