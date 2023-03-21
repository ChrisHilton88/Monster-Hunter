using UnityEngine;

public class WeaponShooting : MonoBehaviour
{
    // Shoot raycast to mouse position.
    // Build a raycast from the end of the weapon barrel (raycast origin)
    // Make the color red so we can see it
    // Limit it's distance, don't use infinity.

    Vector3 _reticulePos = new Vector3(0.5f, 0.5f, 0);

    private bool _canShoot;
    public bool CanShoot
    {
        get { return _canShoot; }
        set { _canShoot = value; }
    }

    void Update()
    {
        Debug.DrawLine(transform.position, transform.forward, Color.red);
    }


    public void RaycastShoot()
    {
        // Change this so the origin point comes from the gun
        Ray rayOrigin = Camera.main.ViewportPointToRay(_reticulePos);
        RaycastHit hitInfo;


        // Shoot bullet


        if(Physics.Raycast(rayOrigin, out hitInfo))
        {

            // Add more to this.
            //GameObject hitObject = null;

            // Create a switch statement for all the different possible collision tags 

        }
    }
}
