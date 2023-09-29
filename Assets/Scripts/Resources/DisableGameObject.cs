using UnityEngine;

public class DisableGameObject : MonoBehaviour
{
    // Start is called before the first frame update

    void OnEnable()
    {
        Invoke("DisableThisGameObject", 2f);
    }

    void DisableThisGameObject()
    {
        gameObject.SetActive(false);
    }
}
