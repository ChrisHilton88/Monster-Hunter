using TMPro;
using UnityEngine;

public class FloatingCombatTextPopUp : MonoSingleton<FloatingCombatTextPopUp>
{
    private float _minRange = -0.3f, _maxRange = 0.3f;      // min and max range for vector position


    // Create Damage PopUp Text
    public void InstantiateDamagePopUp(Vector3 pos, int damage)
    {
        GameObject newPopUp;

        if(damage > 70)
        {
            GameObject obj = FloatingCombatTextObjectPooling.Instance.RequestCriticalPrefab(pos);
            obj.transform.position += RandomVectorPos();
            newPopUp = obj;
        }
        else
        {
            GameObject obj = FloatingCombatTextObjectPooling.Instance.RequestNormalPrefab(pos);
            obj.transform.position += RandomVectorPos();
            newPopUp = obj;
        }

        TextMeshProUGUI temp = newPopUp.transform.GetComponentInChildren<TextMeshProUGUI>();
        temp.text = damage.ToString();
        Debug.Log(pos);
    }

    // Generate a random Vector3
    Vector3 RandomVectorPos()
    {
        Vector3 newVector = new Vector3(Random.Range(-1f, 1f), Random.Range(_minRange, _maxRange), 0);
        return newVector;
    }
}


// Billboarding - Always having the canvas face the camera