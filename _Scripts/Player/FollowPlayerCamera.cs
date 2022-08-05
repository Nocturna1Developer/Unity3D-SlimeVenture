using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class FollowPlayerCamera : MonoBehaviour
{
    [Range(1f,40f)] public float _laziness = 10f;

    [SerializeField] private Transform target;
    [SerializeField] private Vector3 generalOffset;
    [SerializeField] private GameObject _speedLines;
    [SerializeField] private Rigidbody _playerRigidbody;

    [SerializeField] private bool lookAtTarget = true;
    [SerializeField] private bool takeOffsetFromInitialPos = true;

    private Vector3 _whereCameraShouldBe;
    private GameMaster  _gameMaster;

    private void Awake()
    {
        if (takeOffsetFromInitialPos && target != null) 
        {
            generalOffset = transform.position - target.position;
        }
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        transform.position = _gameMaster.lastCheckpointPositionOfCamera;
        _speedLines.SetActive(false);
    }

    void FixedUpdate()
    {
        AimAtPlayer();
        if(_playerRigidbody.velocity.magnitude > 10f)
        {
            _speedLines.SetActive(true);
        }
        else
        {
            _speedLines.SetActive(false);
        }
    }

    private void AimAtPlayer()
    {
        if (target != null)
        {
            _whereCameraShouldBe = target.position + generalOffset;
            transform.position = Vector3.Lerp(transform.position, _whereCameraShouldBe, 1 / _laziness);

            if (lookAtTarget) 
            {
                transform.LookAt(target);
            }
        }
    }
}
