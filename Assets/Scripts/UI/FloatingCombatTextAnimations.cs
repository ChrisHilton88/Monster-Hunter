using TMPro;
using UnityEngine;

// Responsible for all the animations that occur on the floating combat text
public class FloatingCombatTextAnimations : MonoBehaviour
{
    public AnimationCurve animHeight;
    public AnimationCurve animScale;
    public AnimationCurve animAlphaColor;

    private TextMeshProUGUI _tmpU;

    private float _time = 0;

    private Vector3 _origin;


    private void OnEnable()
    {
        _time = 0;
    }

    private void Start()
    {
        _tmpU = GetComponentInChildren<TextMeshProUGUI>();
    }

    private void Update()
    {
        Color currentColor = _tmpU.color;
        currentColor.a = animAlphaColor.Evaluate(_time);
        _tmpU.color = currentColor;

        transform.localScale = NewScale(_time);
        transform.position = NewPosition(_time);
        _time += Time.deltaTime;

        if(_time > 1)
        {
            gameObject.SetActive(false);
        }
    }

    public void UpdateOrigin(Vector3 pos)
    {
        _origin = pos;
    }

    private Vector3 NewScale(float time)
    {
        Vector3 textScale = new Vector3(1 * animScale.Evaluate(time), 1 * animScale.Evaluate(time), 1);
        return textScale;
    }

    private Vector3 NewPosition(float time)
    {
        Vector3 textPos = _origin + new Vector3(0, 1 * animHeight.Evaluate(time), 0);
        return textPos;
    }
}
