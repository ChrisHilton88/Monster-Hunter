using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    //[SerializeField] private TextMeshProUGUI _remainingTimeText;

    //public bool IsOptionsMenuOpen { get; private set; }



    void OnEnable()
    {
        WeaponShooting.reloadWeapon += UpdateAmmoDisplay;
        WeaponShooting.shootWeapon += ShotWeapon;
    }

    void Start()
    {
        _ammoText.text = AmmoManager.Instance.MaxAmmo.ToString();
    }

    // Update the visual display
    void UpdateAmmoDisplay()
    {
        _ammoText.text = AmmoManager.Instance.MaxAmmo.ToString();
        Debug.Log("Update Ammo Display to: " + AmmoManager.Instance.MaxAmmo);
    }

    void ShotWeapon()
    {
        _ammoText.text = AmmoManager.Instance.CurrentAmmoCount.ToString();  
    }

    void OnDisable()
    {
        WeaponShooting.reloadWeapon -= UpdateAmmoDisplay; 
        WeaponShooting.shootWeapon -= ShotWeapon;   
    }
}
