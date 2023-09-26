using TMPro;
using UnityEngine;

public class FloatingCombatTextPopUp : MonoSingleton<FloatingCombatTextPopUp>
{
    [SerializeField] GameObject _damageTextPrefab;
    [SerializeField] GameObject _criticalTextPrefab;

    private float _minRange = -0.3f, _maxRange = 0.3f;      // min and max range for vector position


    // Create Damage PopUp Text
    public void InstantiateDamagePopUp(Vector3 pos, int damage)
    {
        GameObject newPopUp;

        if(damage > 70)
        {
            newPopUp = Instantiate(_criticalTextPrefab, pos + RandomVectorPos(), Quaternion.identity);
        }
        else
        {
            newPopUp = Instantiate(_damageTextPrefab, pos + RandomVectorPos(), Quaternion.identity);
        }

        TextMeshProUGUI temp = newPopUp.transform.GetComponentInChildren<TextMeshProUGUI>();
        temp.text = damage.ToString();
    }


    // Generate a random Vector3
    Vector3 RandomVectorPos()
    {
        Vector3 newVector = new Vector3(Random.Range(_minRange, _maxRange), Random.Range(_minRange, _maxRange), 0);
        return newVector;
    }

}


// Billboarding - Always having the canvas face the camera