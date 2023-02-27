using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevHQ.FileBase.Plugins.FPS_Character_Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class FPS_Controller : MonoBehaviour
    {
        [Header("Controller Info")]
        [SerializeField][Tooltip("How fast can the player walk?")]
        private float _walkSpeed = 3.0f;            // How fast the character is walking.
        [SerializeField][Tooltip("How fast can the player run?")]
        private float _runSpeed = 7.0f;             // How fast the character is running.
        [SerializeField][Tooltip("Set the player's gravity multiplier")] 
        private float _gravity = 1.0f;              // How much gravity to apply.
        [SerializeField][Tooltip("How high can the player jump?")]
        private float _jumpHeight = 15.0f;          // How high can the character jump
        [SerializeField][Tooltip("Returns true (tick) if the player is running")]
        private bool _isRunning = false;            // Bool to display if we are running.
        [SerializeField][Tooltip("Returns true (tick) if the player is crouching")]
        private bool _crouching = false;            // Bool to display if we are crouched or not.

        private CharacterController _controller;    // Reference variable to the 'CharacterController' component.
        private float _yVelocity = 0.0f;            // Cache player's 'Y' velocity.
        

        [Header("Headbob Settings")]       
        [SerializeField][Tooltip("Smooth out the transition from moving to not moving")]
        private float _smooth = 20.0f;              // Smooth out the transition from moving to not moving.
        [SerializeField][Tooltip("How quickly the player head bobs")]
        private float _walkFrequency = 4.8f;        // How quickly the player head bobs while walking.
        [SerializeField][Tooltip("How quickly the player head bobs")]
        private float _runFrequency = 7.8f;         // How quickly the player head bobs while running.
        [SerializeField][Tooltip("How dramatic the headbob is")][Range(0.0f, 0.2f)]
        private float _heightOffset = 0.05f;        // How dramatic the bobbing is.
        private float _timer = Mathf.PI / 2;        // This is where Sin = 1 -- used to simulate walking forward. 
        private Vector3 _initialCameraPos;          // Local position where we reset the camera when it's not bobbing.

        [Header("Camera Settings")]
        [SerializeField][Tooltip("Control the look sensitivty of the camera")]
        private float _lookSensitivity = 5.0f;      // Mouse sensitivity 

        private Camera _fpsCamera;



        private void Start()
        {
            _controller = GetComponent<CharacterController>();      // Assign the reference variable to the component.
            _fpsCamera = GetComponentInChildren<Camera>();
            _initialCameraPos = _fpsCamera.transform.localPosition;
            Cursor.lockState = CursorLockMode.Locked;
        }

        private void Update()
        {
            // Update to NIS
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                Cursor.lockState = CursorLockMode.None;
            }

            FPSController();
            CameraController();
            HeadBobbing(); 
        }

        void FPSController()
        {
            float h = Input.GetAxis("Horizontal"); //horizontal inputs (a, d, leftarrow, rightarrow)
            float v = Input.GetAxis("Vertical"); //veritical inputs (w, s, uparrow, downarrow)

            Vector3 direction = new Vector3(h, 0, v); //direction to move
            Vector3 velocity = direction * _walkSpeed; //velocity is the direction and speed we travel

            // Update Crouching to NIS
            if (Input.GetKeyDown(KeyCode.C) && _isRunning == false)
            {

                if (_crouching == true)
                {
                    _crouching = false;
                    _controller.height = 2.0f;
                }
                else
                {
                    _crouching = true;
                    _controller.height = 1.0f;
                }
                
            }

            // Running
            // Update Running to NIS
            if (Input.GetKey(KeyCode.LeftShift) && _crouching == false)
            {
                velocity = direction * _runSpeed;           // Use the run velocity.
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }

            // Jumping
            // Update Jump to NIS
            if (_controller.isGrounded == true)             // Check if we're grounded.
            {
                if (Input.GetKeyDown(KeyCode.Space))        
                {
                    _yVelocity = _jumpHeight;               // Assign the cache velocity to our jump height.
                }
            }
            else                                            // We're not grounded.
            {
                _yVelocity -= _gravity;                     // Subtract gravity from our yVelocity. 
            }

            velocity.y = _yVelocity;                        // Assign the cached value of our 'Y' velocity.

            velocity = transform.TransformDirection(velocity);

            _controller.Move(velocity * Time.deltaTime);    // Move the controller 'X' meters per second.
        }

        // Camera Movement
        // Update to NIS
        void CameraController()
        {
            float mouseX = Input.GetAxis("Mouse X");        // Get mouse movement on the X axis.
            float mouseY = Input.GetAxis("Mouse Y");        // Get mouse movement on the Y axis.

            Vector3 rot = transform.localEulerAngles;       // Store current rotation.
            rot.y += mouseX * _lookSensitivity;             // Add our mouseX movement to the Y axis.
            transform.localRotation = Quaternion.AngleAxis(rot.y, Vector3.up);      // Rotate along the Y axis by movement amount.

            Vector3 camRot = _fpsCamera.transform.localEulerAngles;                 // Store the current rotation.
            camRot.x += -mouseY * _lookSensitivity;         // Add the mouseY movement to the X axis.
            _fpsCamera.transform.localRotation = Quaternion.AngleAxis(camRot.x, Vector3.right);         // Rotate along the X axis by movement amount.
        }

        // Head Bobbing
        // Update to NIS
        void HeadBobbing()
        {
            float h = Input.GetAxis("Horizontal");          // Horizontal input.
            float v = Input.GetAxis("Vertical");            // Veritical inputs.

            if (h != 0 || v != 0)                           // Are we moving?
            {
               
                if (Input.GetKey(KeyCode.LeftShift))        // Check if running.
                {
                    _timer += _runFrequency * Time.deltaTime;       // Increment timer for our sin/cos waves when running.
                }
                else
                {
                    _timer += _walkFrequency * Time.deltaTime;      // Increment timer for our sin/cos waves when walking.
                }

                Vector3 headPosition = new Vector3          // Calculate the head position in our walk cycle.
                    (
                        _initialCameraPos.x + Mathf.Cos(_timer) * _heightOffset,        // X value.
                        _initialCameraPos.y + Mathf.Sin(_timer) * _heightOffset,        // Y value.
                        0       // Z value.
                    );

                _fpsCamera.transform.localPosition = headPosition;      // Assign the head position.

                if (_timer > Mathf.PI * 2)      // Reset the timer when we complete a full walk cycle on the unit circle.
                {
                    _timer = 0;                 // Completed walk cycle. Reset. 
                }
            }
            else
            {
                _timer = Mathf.PI / 2;          // Reset timer back to 1 for initial walk cycle.

                Vector3 resetHead = new Vector3         // Calculate reset head position back to initial camera position.
                    (
                    Mathf.Lerp(_fpsCamera.transform.localPosition.x, _initialCameraPos.x, _smooth * Time.deltaTime),        // X vlaue.
                    Mathf.Lerp(_fpsCamera.transform.localPosition.y, _initialCameraPos.y, _smooth * Time.deltaTime),        // Y value.
                    0       // Z value.
                    );

                _fpsCamera.transform.localPosition = resetHead;         // Assign the head position back to the initial camera position.
            }
        }
    }

    // OLD INPUT SYSTEM CODE
    #region OLD INPUT SYSTEM CODE


    #endregion
}

