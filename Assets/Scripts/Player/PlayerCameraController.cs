using UnityEngine;

public class PlayerCameraController : MonoBehaviour
{
    [Header("Camera Settings")]
    [SerializeField]
    [Tooltip("Control the look sensitivty of the camera")]
    private float _lookSensitivity = 5.0f;      // Mouse sensitivity 
    
    public Vector3 _initialCameraPos;          // Local position where we reset the camera when it's not bobbing.

    [SerializeField] private Camera _fpsCamera;



    void Start()
    {
        _initialCameraPos = _fpsCamera.transform.localPosition;
    }

    void Update()
    {
        CameraController();
    }

    // Update to NIS
    void CameraController()
    {
        float mouseX = Input.GetAxis("Mouse X");        // Get mouse movement on the X axis.
        float mouseY = Input.GetAxis("Mouse Y");        // Get mouse movement on the Y axis.

        Vector3 rot = transform.localEulerAngles;       // Store current rotation.
        rot.y += mouseX * _lookSensitivity;             // Add our mouseX movement to the Y axis.
        transform.localRotation = Quaternion.AngleAxis(rot.y, Vector3.up);      // Rotate along the Y axis by movement amount.

        Vector3 camRot = _fpsCamera.transform.localEulerAngles;                 // Store the current rotation.
        camRot.x += -mouseY * _lookSensitivity;         // Add the mouseY movement to the X axis.
        _fpsCamera.transform.localRotation = Quaternion.AngleAxis(camRot.x, Vector3.right);         // Rotate along the X axis by movement amount.
    }
}
