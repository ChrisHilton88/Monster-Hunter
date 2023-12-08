using UnityEngine;

// Responsible for rotating an icon while it is active
public class SpinIcons : MonoBehaviour
{
    private float _speed = 40f;

    void Update()
    {
        transform.Rotate(-Vector3.up * _speed * Time.deltaTime, Space.World);
    }
}
