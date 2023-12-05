using TMPro;
using UnityEngine;

public class UIManager : MonoSingleton<UIManager>
{

    [SerializeField] private TextMeshProUGUI _ammoText;
    [SerializeField] private TextMeshProUGUI _scoreText;
    [SerializeField] private TextMeshProUGUI _enemyText;
    [SerializeField] private TextMeshProUGUI _remainingTimeText;

    //public bool IsOptionsMenuOpen { get; private set; }


    #region Initialisation
    private void OnEnable()
    {
        WeaponShooting.reloadWeapon += UpdateAmmoDisplayOnReload;
        WeaponShooting.shootWeapon += ReduceBulletCount;
        EnemyBase.OnEnemyDeath += UpdateScoreText;
    }
    private void OnDisable()
    {
        WeaponShooting.reloadWeapon -= UpdateAmmoDisplayOnReload;
        WeaponShooting.shootWeapon -= ReduceBulletCount;
    }

    private void Start()
    {
        _ammoText.text = Ammo.Instance.MaxAmmo.ToString();          // Set visual ammo display count to equal MaxAmmo
    }
    #endregion

    #region Events
    // Update the visual display in the bottom left corner of screen when Reloading
    private void UpdateAmmoDisplayOnReload()
    {
        _ammoText.text = Ammo.Instance.MaxAmmo.ToString();
        Debug.Log("Update Ammo Display to: " + Ammo.Instance.MaxAmmo);
    }

    // Update visual display in the bottom left corner of the screen when shooting
    private void ReduceBulletCount()
    {
        _ammoText.text = Ammo.Instance.CurrentAmmoCount.ToString();
    }

    // Update visual display in top left corner when an enemy dies
    private void UpdateScoreText(int points)
    {
        _scoreText.text = ScoreManager.Instance.TotalScore.ToString();
    }

    public void UpdateRemainingTextTime(double timer)
    {
        _remainingTimeText.text = timer.ToString();
    }
    #endregion
}
