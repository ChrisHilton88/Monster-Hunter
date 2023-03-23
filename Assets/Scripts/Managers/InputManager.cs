using GameDevHQ.FileBase.Plugins.FPS_Character_Controller;
using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    [SerializeField] private GameObject _managers;

    PlayerInputActions _playerInputActions;
    FPS_Controller _fpsController;
    PlayerCameraController _playerCameraController;
    WeaponShooting _weaponShooting; 
    GameManager _gameManager;



    // Subscribe to events
    void OnEnable()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.LookMovement.performed += LookMovementPerformed;
        _playerInputActions.Player.PlayerMovement.performed += PlayerMovementPerformed;
        _playerInputActions.Player.PlayerMovement.canceled += PlayerMovementCanceled;
        _playerInputActions.Player.Shoot.performed += ShootPerformed;
        _playerInputActions.Player.Shoot.canceled += ShootCanceled;
        _playerInputActions.Player.Crouch.performed += CrouchPerformed;
        _playerInputActions.Player.Crouch.canceled += CrouchCanceled;
        _playerInputActions.Player.Cursor.performed += CursorPerformed;
    }



    void Start()
    {
        _playerCameraController = _player.GetComponent<PlayerCameraController>();   
        _weaponShooting = _player.GetComponentInChildren<WeaponShooting>(); 
        _gameManager = _managers.GetComponentInChildren<GameManager>(); 
        _fpsController = _player.GetComponentInChildren<FPS_Controller>();  
    }


    #region INPUT ACTION EVENTS
    


    // LookMovement - Performed Event
    void LookMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookMovement = context.ReadValue<Vector2>();                    // Cache context callback values
        _playerCameraController.CameraController(lookMovement);                 // Pass in those values to CameraController() method.
    }

    // PlayerMovement - Started Event
    void PlayerMovementPerformed(InputAction.CallbackContext context)
    {
        _fpsController.CanMove = true;
        _fpsController.PlayerMovement(context.ReadValue<Vector2>());    
    }

    // PlayerMovement - Canceled Event
    void PlayerMovementCanceled(InputAction.CallbackContext context)
    {
        _fpsController.CanMove = false;
    }

    // Shoot - Started Event
    void ShootPerformed(InputAction.CallbackContext context)
    {
        if (_weaponShooting.CanShoot)                           // If CanShoot is true, shoot bullet.
            _weaponShooting.ShootBullet();
        else
            return;                                             // Else, do nothing.
    }

    // Shoot - Canceled Event
    void ShootCanceled(InputAction.CallbackContext context)
    {
        _weaponShooting.ShootDelayTimer();                                      // Start fire rate delay
    }

    // Crouch - Performed Event
    void CrouchPerformed(InputAction.CallbackContext context)
    {

    }

    // Crouch - Canceled Event
    void CrouchCanceled(InputAction.CallbackContext context)
    {
        
    }

    // Cursor - Performed Event
    void CursorPerformed(InputAction.CallbackContext context)
    {
        _gameManager.SetCursorStateToNone();
    }


    #endregion

    // Unsubscribe to events.
    void OnDisable()
    {
        _playerInputActions.Player.Disable();
        _playerInputActions.Player.LookMovement.performed -= LookMovementPerformed;
        _playerInputActions.Player.Shoot.performed -= ShootPerformed;
        _playerInputActions.Player.Shoot.canceled -= ShootCanceled;
        _playerInputActions.Player.Crouch.performed -= CrouchPerformed;
        _playerInputActions.Player.Crouch.canceled -= CrouchCanceled;
    }
}
