using UnityEngine;

namespace GameDevHQ.FileBase.Plugins.FPS_Character_Controller
{
    [RequireComponent(typeof(CharacterController))]
    public class FPS_Controller : MonoBehaviour
    {
        [Header("Headbob Settings")]
        [SerializeField]
        [Tooltip("Smooth out the transition from moving to not moving")]
        private float _smooth = 20.0f;              // Smooth out the transition from moving to not moving.
        [SerializeField]
        [Tooltip("How quickly the player head bobs")]
        private float _walkFrequency = 4.8f;        // How quickly the player head bobs while walking.
        [SerializeField]
        [Tooltip("How dramatic the headbob is")]
        [Range(0.0f, 0.2f)]
        private float _heightOffset = 0.05f;        // How dramatic the bobbing is.
        private float _timer = Mathf.PI / 2;        // This is where Sin = 1 -- used to simulate walking forward. 


        private int minFOV = 30, maxFOV = 55;       // Zoom In/Out min/max 

        private float _walkSpeed = 3.0f;            
        private float _zoomMultiplier = 0.5f;

        private bool _isZooming = false;

        private Vector2 _movement;

        private CharacterController _controller;    // Reference variable to the 'CharacterController' component.

        [SerializeField] private Camera _fpsCamera;
        [SerializeField] private PlayerCameraController _playerCam;



        #region Initialisation
        private void Start()
        {
            _controller = GetComponent<CharacterController>();
            _playerCam = GetComponent<PlayerCameraController>();
        }
        #endregion

        #region Methods
        private void Update()
        {
            FPSController(_movement);
            HeadBobbing();
        }

        private void FPSController(Vector2 movement)
        {
            // Old Input System
            //float h = Input.GetAxis("Horizontal"); //horizontal inputs (a, d, left arrow, right arrow)
            //float v = Input.GetAxis("Vertical"); //veritical inputs (w, s, up arrow, down arrow)

            Vector3 direction = new Vector3(movement.x, 0, movement.y);
            Vector3 velocity = direction * _walkSpeed;

            velocity = transform.TransformDirection(velocity);

            _controller.Move(velocity * Time.deltaTime);
        }

        public void PlayerMovement(Vector2 context)
        {
            _movement = context;
        }

        // TODO: LERP Zoom In/Out and lock selection until finished reaching POV size
        public void SniperZoom(float context)
        {
            // Zoom in
            if (context == 1 && _isZooming == false)
            {
                while (_fpsCamera.fieldOfView > minFOV)
                {
                    _fpsCamera.fieldOfView -= _zoomMultiplier;

                    if (_fpsCamera.fieldOfView <= minFOV)
                    {
                        _fpsCamera.fieldOfView = minFOV;

                    }

                    _isZooming = true;
                }
            }
            // Zoom out
            else
            {
                while (_fpsCamera.fieldOfView < maxFOV)
                {
                    _fpsCamera.fieldOfView += _zoomMultiplier;

                    if (_fpsCamera.fieldOfView >= maxFOV)
                    {
                        _fpsCamera.fieldOfView = maxFOV;
                    }

                    _isZooming = false;
                }
            }
        }

        private void HeadBobbing()
        {
            // Old Input System
            //float h = Input.GetAxis("Horizontal");          // Horizontal input.
            //float v = Input.GetAxis("Vertical");            // Veritical inputs.

            if (_movement.x != 0 || _movement.y != 0)                                              // Are we moving?
            {
                _timer += _walkFrequency * Time.deltaTime;                                     // Increment timer for our sin/cos waves when walking.

                Vector3 headPosition = new Vector3                                                 // Calculate the head position in our walk cycle.
                    (
                        _playerCam._initialCameraPos.x + Mathf.Cos(_timer) * _heightOffset,        // X value.
                        _playerCam._initialCameraPos.y + Mathf.Sin(_timer) * _heightOffset,        // Y value.
                        0                                                                          // Z value.
                    );

                _fpsCamera.transform.localPosition = headPosition;                                 // Assign the head position.

                if (_timer > Mathf.PI * 2)                                                         // Reset the timer when we complete a full walk cycle on the unit circle.
                {
                    _timer = 0;                                                                    // Completed walk cycle. Reset. 
                }
            }
            else
            {
                _timer = Mathf.PI / 2;                                                             // Reset timer back to 1 for initial walk cycle.

                Vector3 resetHead = new Vector3                                                    // Calculate reset head position back to initial camera position.
                    (
                    Mathf.Lerp(_fpsCamera.transform.localPosition.x, _playerCam._initialCameraPos.x, _smooth * Time.deltaTime),        // X vlaue.
                    Mathf.Lerp(_fpsCamera.transform.localPosition.y, _playerCam._initialCameraPos.y, _smooth * Time.deltaTime),        // Y value.
                    0                                                                                                                  // Z value.
                    );

                _fpsCamera.transform.localPosition = resetHead;                                    // Assign the head position back to the initial camera position.
            }

            // Old Input System
            //void HeadBobbing()
            //{

            //    if (_movement.x != 0 || _movement.y != 0)                                                         // Are we moving?
            //    {

            //        if (Input.GetKey(KeyCode.LeftShift))        // Check if running.
            //        {
            //            _timer += _runFrequency * Time.deltaTime;       // Increment timer for our sin/cos waves when running.
            //        }
            //        else
            //        {
            //            _timer += _walkFrequency * Time.deltaTime;      // Increment timer for our sin/cos waves when walking.
            //        }

            //        Vector3 headPosition = new Vector3          // Calculate the head position in our walk cycle.
            //            (
            //                _playerCam._initialCameraPos.x + Mathf.Cos(_timer) * _heightOffset,        // X value.
            //                _playerCam._initialCameraPos.y + Mathf.Sin(_timer) * _heightOffset,        // Y value.
            //                0       // Z value.
            //            );

            //        _fpsCamera.transform.localPosition = headPosition;      // Assign the head position.

            //        if (_timer > Mathf.PI * 2)      // Reset the timer when we complete a full walk cycle on the unit circle.
            //        {
            //            _timer = 0;                 // Completed walk cycle. Reset. 
            //        }
            //    }
            //    else
            //    {
            //        _timer = Mathf.PI / 2;          // Reset timer back to 1 for initial walk cycle.

            //        Vector3 resetHead = new Vector3         // Calculate reset head position back to initial camera position.
            //            (
            //            Mathf.Lerp(_fpsCamera.transform.localPosition.x, _playerCam._initialCameraPos.x, _smooth * Time.deltaTime),        // X vlaue.
            //            Mathf.Lerp(_fpsCamera.transform.localPosition.y, _playerCam._initialCameraPos.y, _smooth * Time.deltaTime),        // Y value.
            //            0       // Z value.
            //            );

            //        _fpsCamera.transform.localPosition = resetHead;         // Assign the head position back to the initial camera position.
            //    }
        }
        #endregion
    }
}

