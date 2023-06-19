using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoSingleton<UIManager>
{
    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private TextMeshProUGUI _remainingTimeText;

    private bool _isReloading;
    public bool IsReloading { get; set; }

    private int _minAmmo = 0, _maxAmmo = 10, _currentAmmoCount;
    public int AmmoCount
    {
        get { return _currentAmmoCount; }
        set { _currentAmmoCount = value; }  
    }

    public bool IsOptionsMenuOpen { get; set; } 

    void Start()
    {
        _currentAmmoCount = _maxAmmo;
        _ammoText.text = _maxAmmo.ToString();
    }

    public void UpdateAmmoCount(int count)
    {
        if (_isReloading)                           // If Reloading, run this
        {
            _currentAmmoCount = count;
            IsReloading = false;
        }
        else
        {                                           // Else, run this
            AmmoCount -= count;
            _ammoText.text = _currentAmmoCount.ToString();
        }
    }
}
