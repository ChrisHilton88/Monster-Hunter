using System;
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
        WeaponShooting.OnReloadWeapon += UpdateAmmoDisplayOnReload;
        WeaponShooting.OnShootWeapon += ReduceBulletCount;
        RoundTimerManager.OnRoundStart += UpdateEnemyCount;
    }
    private void OnDisable()
    {
        WeaponShooting.OnReloadWeapon -= UpdateAmmoDisplayOnReload;
        WeaponShooting.OnShootWeapon -= ReduceBulletCount;
        RoundTimerManager.OnRoundStart -= UpdateEnemyCount;
    }

    private void Start()
    {
        _ammoText.text = Ammo.Instance.MaxAmmo.ToString();          // Set visual ammo display count to equal MaxAmmo
        //_enemyText.text = SpawnManager.Instance.WaveList[0].enemyList.Count.ToString();         // Set enemy count equal to first wave count 
        UpdateEnemyCount();
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

    // Update visual display in top left corner when an enemy dies - Called from ScoreManager after internal value is updated. 
    // Had issue with conflicting operation of events as it was incorrectly updating the ScoreText, it was always 1 step behind the real internal value.
    public void UpdateScoreText()
    {
        _scoreText.text = ScoreManager.Instance.TotalScore.ToString();
    }

    // Responsible for updating the enemy UI count in the top right screen
    public void UpdateEnemyCount()
    {
        _enemyText.text = SpawnManager.globalInternalEnemyCount.ToString();     // Taking the static internal value value
    }

    // Responsible for updating the remaining time UI text
    public void UpdateRemainingTextTime(double timer)
    {
        _remainingTimeText.text = timer.ToString();
    }
    #endregion
}
