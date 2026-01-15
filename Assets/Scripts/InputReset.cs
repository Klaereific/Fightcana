//using UnityEngine;
//using UnityEngine.InputSystem;
//
//public class StickDebugger : MonoBehaviour
//{
//    void Update()
//    {
//        var gp = Gamepad.current;
//        if (gp == null)
//        {
//            Debug.Log("No gamepad detected");
//            return;
//        }
//
//        Vector2 stick = gp.leftStick.ReadValue();
//        if (stick.sqrMagnitude > 0.01f)
//            Debug.Log($"Left stick value: {stick}");
//    }
//}
