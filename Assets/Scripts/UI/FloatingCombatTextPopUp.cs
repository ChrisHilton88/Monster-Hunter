using TMPro;
using UnityEngine;

public class FloatingCombatTextPopUp : MonoSingleton<FloatingCombatTextPopUp>
{
    private float _minRange = -0.25f, _maxRange = 0.25f;      // min and max range for vector position


    // Create Damage PopUp Text
    public void ActivateDamagePopUp(Vector3 pos, int damage)        // Position of damage text pop up, damage dealt
    {
        GameObject obj;

        if(damage > 70)     // Critical hit
        {
            obj = FloatingCombatTextObjectPooling.Instance.RequestCriticalPrefab(pos, RandomVectorPos());
        }
        else                // Normal hit
        {
            obj = FloatingCombatTextObjectPooling.Instance.RequestNormalPrefab(pos, RandomVectorPos());
        }

        TextMeshProUGUI temp = obj.transform.GetComponentInChildren<TextMeshProUGUI>();
        temp.text = damage.ToString();
    }

    // Gives slight randomness to spawn pos
    Vector3 RandomVectorPos()
    {
        Vector3 newVector = new Vector3(Random.Range(-1f, 1f), Random.Range(_minRange, _maxRange), 0);
        return newVector;
    }
}


// Billboarding - Always having the canvas face the camera