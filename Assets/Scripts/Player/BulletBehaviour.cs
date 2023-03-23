using UnityEngine;

public class BulletBehaviour : MonoBehaviour
{
    private Vector3 _bulletScale = new Vector3(200, 200, 200);
    Rigidbody _rb;

    private float _speed = 100f;



    void Start()
    {
        transform.localScale = _bulletScale;
        _rb = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        // This needs to be updated to reflect the local rotation of the bullet and not the World.
        _rb.AddForce(Vector3.forward * _speed, ForceMode.Impulse);
    }

    void OnCollisionEnter(Collision other)
    {
        if (other.collider.CompareTag("Floor"))
        {
            _rb.gameObject.SetActive(false);
        }
    }
}
