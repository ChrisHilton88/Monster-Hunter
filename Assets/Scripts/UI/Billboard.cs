using UnityEngine;

public class Billboard : MonoBehaviour
{
    Camera _cam;


    void Awake()
    {
        _cam = Camera.main;
    }

    void Update()
    {
        transform.forward = -_cam.transform.forward;        // Set to negative because our projects forward face is opposite (180)
    }
}
