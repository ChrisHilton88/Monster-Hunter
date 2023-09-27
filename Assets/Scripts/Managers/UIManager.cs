using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    //[SerializeField] private TextMeshProUGUI _remainingTimeText;

    public bool IsOptionsMenuOpen { get; set; }



    void OnEnable()
    {
        InputManager.reloadWeapon += UpdateAmmoDisplay;
    }

    void Start()
    {
        _ammoText.text = AmmoManager.Instance.MaxAmmo.ToString();
    }

    // Update the visual display
    void UpdateAmmoDisplay()
    {

        Debug.Log("Update Ammo Display to: " + AmmoManager.Instance.MaxAmmo);
    }

    void OnDisable()
    {
        InputManager.reloadWeapon -= UpdateAmmoDisplay; 
    }
}
