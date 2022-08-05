using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class HomingMissle : MonoBehaviour
{
    [Header("Core Properties")]
    [SerializeField] private Transform _rocketTarget;
    [SerializeField] private Rigidbody _rocketRigidbody;
    [SerializeField] private float     _turnSpeed = 1f;
    [SerializeField] private float     _rocketFlySpeed = 10f;
    private bool _alreadyHitPlayerOnce;

    [Header("Audio Properties")]
    [SerializeField] private AudioSource _explosionSound;
    [SerializeField] private AudioSource _idleSound;

    [Header("Particles Properties")]
    [SerializeField] private GameObject _explosionEffect;
 
    private Transform _rocketLocalTrans;

    private void Start()
    {  
        _rocketTarget = GameObject.Find("SlimePlayer3D").transform;
        _rocketLocalTrans = GetComponent<Transform>();
        _alreadyHitPlayerOnce = true;
    }
 
    private void FixedUpdate()
    {
        FollowPlayer();
    }

    private void FollowPlayer()
    {
        _rocketRigidbody.velocity = _rocketLocalTrans.forward * _rocketFlySpeed;
 
        // Now Turn the Rocket towards the Target
        var _rocketTargetRot = Quaternion.LookRotation(_rocketTarget.position - _rocketLocalTrans.position);
 
        _rocketRigidbody.MoveRotation(Quaternion.RotateTowards(_rocketLocalTrans.rotation, _rocketTargetRot, _turnSpeed));
    }
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            Rigidbody playerRigidBody = other.gameObject.GetComponent<Rigidbody>();
            if (playerRigidBody)
            {
                _alreadyHitPlayerOnce = false;
                _explosionSound.Play();
                _idleSound.Stop();
                Instantiate(_explosionEffect, transform.position, transform.rotation);
                playerRigidBody.AddForceAtPosition(-Vector3.forward * 5f, playerRigidBody.position);
            }
        }
    }
}