using TMPro;
using UnityEngine;

public class FloatingCombatTextPopUp : MonoSingleton<FloatingCombatTextPopUp>
{
    [SerializeField] GameObject _damageTextPrefab;

    private float _minValue = -0.3f, _maxValue = 0.3f;


    // Create Damage PopUp Text
    public void InstantiateDamagePopUp(Vector3 pos, int damage)
    {
        GameObject newPopUp = Instantiate(_damageTextPrefab, pos + RandomVector(), Quaternion.identity);     // Instantiate a new _damageTextPrfab GameObject and get a reference to it
        TextMeshProUGUI temp = newPopUp.transform.GetComponentInChildren<TextMeshProUGUI>();      // Create a temporary TMPUGUI class to hold a reference to the component
        temp.text = damage.ToString();       // Assign the .text field from the component to the string value passed in through the parameter
    }

    // Generate a random Vector3
    Vector3 RandomVector()
    {
        Vector3 newVector = new Vector3(Random.Range(_minValue, _maxValue), Random.Range(_minValue, _maxValue), 0);
        return newVector;
    }
}


// Billboarding - Always having the canvas face the camera