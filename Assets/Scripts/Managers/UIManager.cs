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
        _ammoText.text = Ammo.Instance.MaxAmmo.ToString();
    }

    // Update the visual display
    void UpdateAmmoDisplay()
    {
        _ammoText.text = Ammo.Instance.MaxAmmo.ToString();
        Debug.Log("Update Ammo Display to: " + Ammo.Instance.MaxAmmo);
    }

    void ShotWeapon()
    {
        _ammoText.text = Ammo.Instance.CurrentAmmoCount.ToString();  
    }

    void OnDisable()
    {
        WeaponShooting.reloadWeapon -= UpdateAmmoDisplay; 
        WeaponShooting.shootWeapon -= ShotWeapon;   
    }
}
