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
        _playerInputActions.Player.Jump.performed += JumpPerformed;
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
    


    void LookMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookMovement = context.ReadValue<Vector2>();                    // Cache context callback values
        _playerCameraController.CameraController(lookMovement);                 // Pass in those values to CameraController() method.
    }

    void PlayerMovementPerformed(InputAction.CallbackContext context)
    {
        _fpsController.PlayerMovement(context.ReadValue<Vector2>());    
    }

    void PlayerMovementCanceled(InputAction.CallbackContext context)
    {
        _fpsController.PlayerMovement(Vector2.zero);
    }

    void JumpPerformed(InputAction.CallbackContext context)
    {
        
    }

    void ShootPerformed(InputAction.CallbackContext context)
    {
        if (_weaponShooting.CanShoot)                           // If CanShoot is true, shoot bullet.
            _weaponShooting.ShootBullet();
        else
            return;                                             // Else, do nothing.
    }

    void ShootCanceled(InputAction.CallbackContext context)
    {
        _weaponShooting.ShootDelayTimer();                                      // Start fire rate delay
    }

    void CrouchPerformed(InputAction.CallbackContext context)
    {
        _fpsController.Crouching(context.ReadValue<float>());
        Debug.Log("Crouching");
    }

    void CrouchCanceled(InputAction.CallbackContext context)
    {
        _fpsController.Crouching(context.ReadValue<float>());
        Debug.Log("Not Crouching!");
    }

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
