using UnityEngine;
using UnityEngine.InputSystem;

public class InputManager : MonoBehaviour
{
    PlayerInputActions _playerInputActions;


    void Awake()
    {
        _playerInputActions = new PlayerInputActions();

    }

    // Subscribe to events
    void OnEnable()
    {
        _playerInputActions.Player.Enable();
        _playerInputActions.Player.Shoot.performed += ShootPerformed;
    }



    void Start()
    {
        
    }

    void Update()
    {
        
    }

    #region INPUT ACTION EVENTS
    void ShootPerformed(InputAction.CallbackContext context)
    {
        Debug.Log("Shot");
    }


    #endregion

    // Unsubscribe to events.
    void OnDisable()
    {
        
    }
}
