using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;
using FirstGearGames.SmoothCameraShaker;
using Quaternion = UnityEngine.Quaternion;

public class SwingController : MonoBehaviour
{
    [Header("Core Properties")]
    [SerializeField] private float      _moveSpeed  = 7f;
    [SerializeField] private float      _maxSpeed   = 10f;
    [SerializeField] private float      _jumpPower  = 6f;
    [SerializeField] private float      _fallMultiplier       = 2.5f;
    [SerializeField] private float      _lowJumpMultiplier    = 2f;
    [SerializeField] private float      _distToGroundTreshold = 0.1f;
    [SerializeField] private GameObject _smokeTrail;
    private Rigidbody _playerRigidbody;
    private GameObject anchor;
    private HingeJoint joint;
    private HingeJoint anchorJoint;
    private float _distToGround;

    [Header("Camera Properties")]
    [SerializeField] private ShakeData _offRoadData = null;
    private ShakerInstance             _offRoadInstance;

    [Header("Swing Properties")]
    [SerializeField] private float _indicatorOffset = 0.5f;
    [SerializeField] private float _indicatorPressedOffset = 0.4f;
    [SerializeField] private float _maxTetherDistance;
    private GameObject[] _gos;
    private GameObject   _indicatorGameObject;
    private GameObject   indicatorSphere;
    private Transform    _tetherTransform;
    private Vector3      closestTether;

    [Header("Audio Properties")]
    [SerializeField] private AudioSource _connectSound;
    [SerializeField] private AudioSource _releaseSound;
    [SerializeField] private AudioSource _jumpSound; 
    private bool soundplayed;

    private LineRenderer lr;

    private void Start()
    {
        // Lock rotation of _playerRigidbody 
        _playerRigidbody = GetComponent<Rigidbody>();
        _playerRigidbody.centerOfMass = Vector3.zero;
        _playerRigidbody.inertiaTensorRotation = Quaternion.identity;

        // Tether indicator 
        _gos = GameObject.FindGameObjectsWithTag("Tether Points");
        anchor = GameObject.Find("Anchor");
        _tetherTransform = FindClosestTetherPoint(_gos).transform; // Get the closest tether transform
        _indicatorGameObject = GameObject.Find("Indicator"); // Find Game Object "Indicator"
        indicatorSphere = GameObject.Find("IndicatorSphere"); // Find Game Object "Indicator"
        _indicatorGameObject.transform.position = new Vector3(_tetherTransform.position.x, _tetherTransform.position.y + _indicatorOffset, 0);

        soundplayed = false;
        lr = GetComponent<LineRenderer>();

        _smokeTrail.SetActive(false);
    }

    private void Update()
    {
        // If we are not pressing the mouse button or the A key or the D key then make the player
        // fall faster
        WeightyJumpModifier();
        // if(!Input.GetMouseButton(0) && !Input.GetKey(KeyCode.D) && !Input.GetKey(KeyCode.A))
        // {
        //     WeightyJumpModifier();
        // }

        // Hold
        if (Input.GetMouseButton(0))
        { 
            DoSwingAction();
            if (_offRoadInstance != null)
            {
                _offRoadInstance.FadeOut();
                _offRoadInstance = null;
            }
            else
            {
                _offRoadInstance = CameraShakerHandler.Shake(_offRoadData);
            }
        }
        // Release
        else
        {
            DoFallingAction();
        }

        // Update rope positions
        lr.SetPosition(0, transform.position);
        lr.SetPosition(1, anchor.transform.position);
    }

    private void FixedUpdate()
    {
        PlayerMovement();
        PlayerJump();
    }

    private void PlayerMovement()
    {
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        // Normalizes the speed so player doesn't constantly acclerate
        Vector3 movement = new Vector3(moveHorizontal, 0.0f, moveVertical);
        
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.A))
        {
            _smokeTrail.SetActive(true);
        }
        else
        {
            _smokeTrail.SetActive(false);
        }

        if(_playerRigidbody.velocity.magnitude > _maxSpeed)
        {
            _playerRigidbody.velocity = _playerRigidbody.velocity.normalized * _maxSpeed;
        }
        _playerRigidbody.AddForce(movement * _moveSpeed);
    }

    private bool IsGrounded()
    {
        //Debug.Log("Player is now grounded, you can jump");
        return Physics.Raycast(transform.position, -Vector3.up, _distToGround + _distToGroundTreshold);
    }

    private void PlayerJump()
    {
        if ((Input.GetKey(KeyCode.Space) || Input.GetKey(KeyCode.W)) && IsGrounded())
        {
            _playerRigidbody.AddForce(0, _jumpPower, 0, ForceMode.Impulse);
            _jumpSound.Play();
        }
    }

    private void WeightyJumpModifier()
    {
        // If the player is falling make them fall faster for that weighty feel
        if(_playerRigidbody.velocity.y < 0)
        {
            _playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (_fallMultiplier - 1) * Time.deltaTime;
        }
        else if(_playerRigidbody.velocity.y > 0 && !Input.GetButton("Jump"))
        {
            _playerRigidbody.velocity += Vector3.up * Physics.gravity.y * (_lowJumpMultiplier - 1) * Time.deltaTime;
        }
    }

    private void DoSwingAction()
    {
        // Calculate angle between player and tether point and rotate the player around it
        var dir = (_tetherTransform.position - transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) *Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);

        // Fire Rope (Extra check, every thing here runs only once per tethering action) 
        if (Input.GetMouseButtonDown(0))
        {
            // Get the vector to the closest Tether point as long as there are points
            closestTether = FindClosestTetherPoint(_gos).transform.position - transform.position;

            // Move pressed indicator position to _tetherTransform pos
            _indicatorGameObject.transform.position = new Vector3(_tetherTransform.position.x, _tetherTransform.position.y + _indicatorPressedOffset , 0);
            indicatorSphere.transform.position = new Vector3(_tetherTransform.position.x, _tetherTransform.position.y, 0);

            // Shoot a ray out towards that position       
            LayerMask ignorePlayer = ~(1 << LayerMask.NameToLayer("Player"));
            Physics.Raycast(transform.position, closestTether, out RaycastHit hit, _maxTetherDistance, ignorePlayer);
            if (hit.collider)
            {
                if (hit.collider.tag == "Tether Points")
                {
                    // Move the anchor to the correct position
                    anchor.transform.position = new Vector3(hit.point.x, hit.point.y, 0);
                    // Zero out any rotation of anchor
                    anchor.transform.rotation = Quaternion.identity;

                    // Create HingeJoints
                    joint = gameObject.AddComponent<HingeJoint>();
                    joint.axis = Vector3.forward;
                    joint.anchor = Vector3.zero;
                    joint.connectedBody = anchor.GetComponent<Rigidbody>();

                    // Create anchor HingeJoint
                    anchorJoint = anchor.AddComponent<HingeJoint>();
                    anchorJoint.axis = Vector3.forward;
                    anchorJoint.anchor = Vector3.zero;
                    lr.enabled = true; // show rope

                    // Play connect sound
                    if (!soundplayed)
                    {
                        _connectSound.Play();
                        soundplayed = true;
                    }
                }
            }
        }
    }

    private void DoFallingAction()
    {
        // Keep updating position of closest while flying as long as we find tether points
        if (FindClosestTetherPoint(_gos) != null)
        {
            _tetherTransform = FindClosestTetherPoint(_gos).transform;
        }

        // Move indicator to the closest tether point
        _indicatorGameObject.transform.position = new Vector3(_tetherTransform.position.x, _tetherTransform.position.y + _indicatorOffset, 0f);
        // Move sphere away from screen
        indicatorSphere.transform.position = new Vector3(0f, -1.0f, 0f);

        // Update player rotation while flying
        Vector3 direction = _playerRigidbody.velocity.normalized;
        float rotationZ = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if (_playerRigidbody.velocity != Vector3.zero)
            transform.rotation = Quaternion.Euler(0f, 0f, rotationZ - (360.0f));
        
        // Called only once
        if(Input.GetMouseButtonUp(0))
        {
            _releaseSound.Play();
            soundplayed = false;
            Destroy(joint);
            Destroy(anchorJoint);
            lr.enabled = false;
        }
    }

    GameObject FindClosestTetherPoint(GameObject[] _gos)
    {
        GameObject closest = null;
        float distance = Mathf.Infinity;

        // Player position
        Vector3 position = transform.position;
        foreach (GameObject go in _gos)
        {
            // Get vector from Tether point to Player
            Vector3 diff = go.transform.position - position;
            // Get distance value of this vector
            float curDistance = diff.sqrMagnitude;
            if (curDistance < distance)
            {
                closest = go;
                distance = curDistance;
            }
        }
        return closest;
    }
}