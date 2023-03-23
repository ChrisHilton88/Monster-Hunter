using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    [SerializeField] private GameObject _player;
    PlayerInputActions _playerInputActions;
    PlayerCameraController _playerCameraController;
    WeaponShooting _weaponShooting; 



    // Subscribe to events
    void OnEnable()
    {
        _playerInputActions = new PlayerInputActions();
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.LookMovement.performed += LookMovementPerformed;
        _playerInputActions.Player.Shoot.performed += ShootPerformed;
        _playerInputActions.Player.Shoot.canceled += ShootCanceled;
    }



    void Start()
    {
        _playerCameraController = _player.GetComponent<PlayerCameraController>();   
        _weaponShooting = _player.GetComponentInChildren<WeaponShooting>(); 
    }


    #region INPUT ACTION EVENTS
    
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

    // LookMovement - Performed Event
    void LookMovementPerformed(InputAction.CallbackContext context)
    {
        Vector2 lookMovement = context.ReadValue<Vector2>();                    // Cache context callback values
        _playerCameraController.CameraController(lookMovement);                 // Pass in those values to CameraController() method.
    }


    #endregion

    // Unsubscribe to events.
    void OnDisable()
    {
        
    }
}
