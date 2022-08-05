using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using FirstGearGames.SmoothCameraShaker;
using FirstGearGames.SmoothCameraShaker.Demo;

public class GrappleGun : MonoBehaviour
{
    [Header("Core Properties")]
    [SerializeField] private LineRenderer _lineRenderer;
    [SerializeField] private LayerMask _whatIsGrappleable;
    [SerializeField] private ShakeData _offRoadData = null;
    [SerializeField] private Transform _camera;
    [SerializeField] private Transform _player;
    [SerializeField] private Rigidbody _ballRigidbody;

    public Transform _gunTip;
    private ShakerInstance _offRoadInstance;

    [Header("Rope Properties")]
    [SerializeField] private float _springAmount;
    [SerializeField] private float _damperAmount;
    [SerializeField] private float _massScaleAmount;
    [SerializeField] private float _ropeLengthThresholdMin = 0.01f;
    [SerializeField] private float _ropeLengthThresholdMax = 0.05f;

    private float        _maxDistance = 100f;
    private SpringJoint  _springJoint;
    private Vector3      _grapplePoint;

    [Header("Audio Properties")]
    [SerializeField] private AudioSource _connectSound; 
    [SerializeField] private AudioSource _releaseSound;  

    private void Awake()
    {
        _lineRenderer = GetComponent<LineRenderer>();
    }

    private void Update() 
    {
        if (Input.GetMouseButtonDown(0))
        {
            StartGrapple();
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _ballRigidbody.AddForce(0, 5f, 0, ForceMode.Impulse);
            }
            //_connectSound.Play();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            StopGrapple();
            _releaseSound.Play();
        }
    }

    private void LateUpdate()
    {
        DrawRope();
    }


    private void StartGrapple()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit, _maxDistance, _whatIsGrappleable)) 
        {
            _connectSound.Play();
            _grapplePoint = hit.point;
            _springJoint = _player.gameObject.AddComponent<SpringJoint>();
            _springJoint.autoConfigureConnectedAnchor = false;
            _springJoint.connectedAnchor = _grapplePoint;

            float distanceFromPoint = Vector3.Distance(_player.position, _grapplePoint);

            // The distance grapple will try to keep from grapple point. 
            //_springJoint.maxDistance = distanceFromPoint * _ropeLengthThresholdMax;
            //_springJoint.minDistance = distanceFromPoint * _ropeLengthThresholdMin;
            _springJoint.maxDistance = distanceFromPoint;
            _springJoint.minDistance = distanceFromPoint;

            _springJoint.spring = _springAmount;
            _springJoint.damper = _damperAmount;
            _springJoint.massScale = _massScaleAmount;

            _lineRenderer.positionCount = 2;
            currentGrapplePosition = _gunTip.position;

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
    }

    private void StopGrapple()
    {
        _lineRenderer.positionCount = 0;
        Destroy(_springJoint);
    }

    private Vector3 currentGrapplePosition;
    
    private void DrawRope()
    {
        //If not grappling, don't draw rope
        if (!_springJoint) return;

        currentGrapplePosition = Vector3.Lerp(currentGrapplePosition, _grapplePoint, Time.deltaTime * 8f);
        
        _lineRenderer.SetPosition(0, _gunTip.position);
        _lineRenderer.SetPosition(1, currentGrapplePosition);
    }

    public bool IsGrappling() 
    {
        return _springJoint != null;
    }

    public Vector3 GetGrapplePoint() 
    {
        return _grapplePoint;
    }
}