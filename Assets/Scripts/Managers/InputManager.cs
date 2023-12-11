using GameDevHQ.FileBase.Plugins.FPS_Character_Controller;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;

    PlayerInputActions _playerInputActions;
    FPS_Controller _fpsController;
    PlayerCameraController _playerCameraController;
    WeaponShooting _weaponShooting; 
    GameManager _gameManager;



    #region Initialisation
    private void OnEnable()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.LookMovement.performed += LookMovementPerformed;
        _playerInputActions.Player.PlayerMovement.performed += PlayerMovementPerformed;
        _playerInputActions.Player.PlayerMovement.canceled += PlayerMovementCanceled;
        _playerInputActions.Player.Shoot.performed += ShootPerformed;
        _playerInputActions.Player.Reload.performed += ReloadPerformed;
        _playerInputActions.Player.Reload.canceled += ReloadCanceled;
        _playerInputActions.Player.SniperZoom.performed += SniperZoomPerformed;
        _playerInputActions.Player.SniperZoom.canceled += SniperZoomCanceled;
        _playerInputActions.Player.Cursor.performed += CursorPerformed;
        //_playerInputActions.Player.OptionsMenu.performed += OptionsMenuPerformed;
    }

    private void OnDisable()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.Player.LookMovement.performed -= LookMovementPerformed;
        _playerInputActions.Player.PlayerMovement.performed -= PlayerMovementPerformed;
        _playerInputActions.Player.PlayerMovement.canceled -= PlayerMovementCanceled;
        _playerInputActions.Player.Shoot.performed -= ShootPerformed;
        _playerInputActions.Player.SniperZoom.performed -= SniperZoomPerformed;
        _playerInputActions.Player.SniperZoom.canceled -= SniperZoomCanceled;
        _playerInputActions.Player.Cursor.performed -= CursorPerformed;
        //_playerInputActions.Player.OptionsMenu.performed -= OptionsMenuCanceled;
    }

    private void Start()
    {
        _playerCameraController = _player.GetComponent<PlayerCameraController>();
        _weaponShooting = _player.GetComponentInChildren<WeaponShooting>();
        _fpsController = _player.GetComponentInChildren<FPS_Controller>();
    }
    #endregion#

    #region INPUT ACTION EVENTS
    private void LookMovementPerformed(InputAction.CallbackContext context)
    {
        _playerCameraController.CameraController(context.ReadValue<Vector2>());                 
    }

    private void PlayerMovementPerformed(InputAction.CallbackContext context)
    {
        _fpsController.PlayerMovement(context.ReadValue<Vector2>());    
    }

    private void PlayerMovementCanceled(InputAction.CallbackContext context)
    {
        _fpsController.PlayerMovement(Vector2.zero);
    }

    private void ShootPerformed(InputAction.CallbackContext context)
    {
        if(Ammo.Instance.CanShoot)
            WeaponShooting.OnShootWeapon?.Invoke();
    }

    private void ReloadPerformed(InputAction.CallbackContext context)
    {
        if (Ammo.Instance.CanReload)
            WeaponShooting.OnReloadWeapon?.Invoke();
    }

    private void ReloadCanceled(InputAction.CallbackContext context)
    {
        // TODO: If Player presses 'R' key again while reloading is active, cancel reload
        
    }

    private void SniperZoomPerformed(InputAction.CallbackContext context)
    {
        // Start zooming in process
        _fpsController.SniperZoom(context.ReadValue<float>());  
    }

    private void SniperZoomCanceled(InputAction.CallbackContext context)
    {
        // Stop zooming out process and go back to normal
        _fpsController.SniperZoom(context.ReadValue<float>());
    }

    private void CursorPerformed(InputAction.CallbackContext context)
    {
        _gameManager.SetCursorStateToNone();
    }

    //void OptionsMenuPerformed(InputAction.CallbackContext context)
    //{
    //    if (UIManager.Instance.IsOptionsMenuOpen)
    //    {
    //        // Close menu
    //    }
    //    else
    //    {
    //        // Open menu
    //    }
    //    Debug.Log("Opened the Options Menu");
    //}
    #endregion
}
