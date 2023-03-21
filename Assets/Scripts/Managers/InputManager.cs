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
        _playerInputActions.Player.Shoot.started += ShootStarted;
        _playerInputActions.Player.Shoot.canceled += ShootCanceled;
    }



    void Start()
    {
        _playerCameraController = _player.GetComponent<PlayerCameraController>();   
        _weaponShooting = _player.GetComponentInChildren<WeaponShooting>(); 
    }


    #region INPUT ACTION EVENTS
    // Shoot - Started Event
    void ShootStarted(InputAction.CallbackContext context)
    {
        // By using started, we should be able to prevent the use of holding down the button to fire.
        // The player should lift the button before being able to fire again.
        _weaponShooting.RaycastShoot();
    }

    // Shoot - Canceled Event
    void ShootCanceled(InputAction.CallbackContext context)
    {
        Debug.Log(context.phase);   
        // When the player releases the left mouse button is when they can fire again.
        // Can add a fire rate delay here to give it more authenticity of a sniper reload.
        // Once this event has been triggered, start the timer before they can shoot again.
    }

    // LookMovement Performed Event
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
