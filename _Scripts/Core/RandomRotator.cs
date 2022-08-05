using UnityEngine;
using System.Collections;

public class RandomRotator : MonoBehaviour
{
    [Header("Core Properies")]
    [SerializeField] private float _rotateAmount;
    private Rigidbody _rigidbody;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }
    
    private void Start()
    {
        _rigidbody.angularVelocity = Random.insideUnitSphere * _rotateAmount;
    }
}