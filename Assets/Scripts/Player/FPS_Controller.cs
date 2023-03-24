using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.VFX;

namespace GameDevHQ.FileBase.Plugins.FPS_Character_Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class FPS_Controller : MonoBehaviour
    {
        [Header("Controller Info")]
        [SerializeField]
        [Tooltip("How fast can the player walk?")]
        private float _walkSpeed = 3.0f;            // How fast the character is walking.
        [SerializeField]
        [Tooltip("How fast can the player run?")]
        private float _runSpeed = 7.0f;             // How fast the character is running.
        [SerializeField]
        [Tooltip("Set the player's gravity multiplier")]
        private float _gravity = 1.0f;              // How much gravity to apply.
        [SerializeField]
        [Tooltip("How high can the player jump?")]
        private float _jumpHeight = 15.0f;          // How high can the character jump
        [SerializeField]
        [Tooltip("Returns true if the player is running")]
        private bool _isRunning = false;            // Bool to display if we are running.
        [SerializeField]
        [Tooltip("Returns true if the player is crouching")]
        private bool _crouching = false;            // Bool to display if we are crouched or not.

        private CharacterController _controller;    // Reference variable to the 'CharacterController' component.
        private float _yVelocity = 0.0f;            // Cache player's 'Y' velocity.


        [Header("Headbob Settings")]
        [SerializeField]
        [Tooltip("Smooth out the transition from moving to not moving")]
        private float _smooth = 20.0f;              // Smooth out the transition from moving to not moving.
        [SerializeField]
        [Tooltip("How quickly the player head bobs")]
        private float _walkFrequency = 4.8f;        // How quickly the player head bobs while walking.
        [SerializeField]
        [Tooltip("How quickly the player head bobs")]
        private float _runFrequency = 7.8f;         // How quickly the player head bobs while running.
        [SerializeField]
        [Tooltip("How dramatic the headbob is")]
        [Range(0.0f, 0.2f)]
        private float _heightOffset = 0.05f;        // How dramatic the bobbing is.
        private float _timer = Mathf.PI / 2;        // This is where Sin = 1 -- used to simulate walking forward. 

        private PlayerCameraController _playerCam;
        [SerializeField] private Camera _fpsCamera;

        private Vector2 _movement;
        private Vector3 _crouchingPos = new Vector3(0, -0.3f, 0);
        private Vector3 _standingPos = new Vector3(0, 0.3f, 0);



        void Start()
        {
            _controller = GetComponent<CharacterController>();      
            _playerCam = GetComponent<PlayerCameraController>();
        }

        void Update()
        {
            FPSController(_movement);


            //HeadBobbing(); 
        }

        void FPSController(Vector2 movement)
        {
            //float h = Input.GetAxis("Horizontal"); //horizontal inputs (a, d, leftarrow, rightarrow)
            //float v = Input.GetAxis("Vertical"); //veritical inputs (w, s, uparrow, downarrow)

            if(!_isRunning)
            {
                Vector3 direction = new Vector3(movement.x, 0, movement.y);                                   //direction to move
                Vector3 velocity = direction * _walkSpeed;                                                    //velocity is the direction and speed we travel
                _controller.Move(velocity * Time.deltaTime);    // Move the controller 'X' meters per second.
            }
            else
            {
                Vector3 direction = new Vector3(movement.x, 0, movement.y);                                   //direction to move
                Vector3 velocity = direction * _runSpeed;                                                     //velocity is the direction and speed we travel
                _controller.Move(velocity * Time.deltaTime);    // Move the controller 'X' meters per second.
            }


            // Jumping
            //if (_controller.isGrounded == true)             // Check if we're grounded.
            //{
            //    if (Input.GetKeyDown(KeyCode.Space))
            //    {
            //        _yVelocity = _jumpHeight;               // Assign the cache velocity to our jump height.
            //    }
            //}
            //else                                            // We're not grounded.
            //{
            //    _yVelocity -= _gravity;                     // Subtract gravity from our yVelocity. 
            //}

            //velocity.y = _yVelocity;                        // Assign the cached value of our 'Y' velocity.

            //velocity = transform.TransformDirection(velocity);
        }

        public void PlayerMovement(Vector2 context)
        {
            _movement = context;
        }

        public void Crouching(float context)
        {
            if (_isRunning == false && context == 1)
            {
                _crouching = true;
                _controller.height = 1f;
                _controller.Move(_crouchingPos);
                Debug.Log("Cont height: " + _controller.height);
            }
            else
            {
                _crouching = false;
                _controller.height = 2f;
                _controller.Move(_standingPos);
                Debug.Log("Cont height: " + _controller.height);
            }

            // Old Input System
            //if (Input.GetKeyDown(KeyCode.C) && _isRunning == false)
            //{
            //    if (_crouching == true)
            //    {
            //        _crouching = false;
            //        _controller.height = 2.0f;
            //    }
            //    else
            //    {
            //        _crouching = true;
            //        _controller.height = 1.0f;
            //    }
            //}
        }

        public void Running(float context)
        {
            if (_crouching == false && context == 1)
            {
                _isRunning = true;
            }
            else
            {
                _isRunning = false;
            }
        }

        //    void HeadBobbing()
        //    {
        //        float h = Input.GetAxis("Horizontal");          // Horizontal input.
        //        float v = Input.GetAxis("Vertical");            // Veritical inputs.

        //        if (h != 0 || v != 0)                           // Are we moving?
        //        {

        //            if (Input.GetKey(KeyCode.LeftShift))        // Check if running.
        //            {
        //                _timer += _runFrequency * Time.deltaTime;       // Increment timer for our sin/cos waves when running.
        //            }
        //            else
        //            {
        //                _timer += _walkFrequency * Time.deltaTime;      // Increment timer for our sin/cos waves when walking.
        //            }

        //            Vector3 headPosition = new Vector3          // Calculate the head position in our walk cycle.
        //                (
        //                    _playerCam._initialCameraPos.x + Mathf.Cos(_timer) * _heightOffset,        // X value.
        //                    _playerCam._initialCameraPos.y + Mathf.Sin(_timer) * _heightOffset,        // Y value.
        //                    0       // Z value.
        //                );

        //            _fpsCamera.transform.localPosition = headPosition;      // Assign the head position.

        //            if (_timer > Mathf.PI * 2)      // Reset the timer when we complete a full walk cycle on the unit circle.
        //            {
        //                _timer = 0;                 // Completed walk cycle. Reset. 
        //            }
        //        }
        //        else
        //        {
        //            _timer = Mathf.PI / 2;          // Reset timer back to 1 for initial walk cycle.

        //            Vector3 resetHead = new Vector3         // Calculate reset head position back to initial camera position.
        //                (
        //                Mathf.Lerp(_fpsCamera.transform.localPosition.x, _playerCam._initialCameraPos.x, _smooth * Time.deltaTime),        // X vlaue.
        //                Mathf.Lerp(_fpsCamera.transform.localPosition.y, _playerCam._initialCameraPos.y, _smooth * Time.deltaTime),        // Y value.
        //                0       // Z value.
        //                );

        //            _fpsCamera.transform.localPosition = resetHead;         // Assign the head position back to the initial camera position.
        //        }
        //    }
        //}
    }
}

