using UnityEngine;
using System.Collections;
using System.Collections.Generic;
 
public class PlayerProjectile : MonoBehaviour
{
    [Header("Audio Properties")]
    [SerializeField] private AudioSource _hitMarkerSound;
    [SerializeField] private AudioSource _idleSound;
 
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Enemy")
        {
            _hitMarkerSound.Play();
            _idleSound.Stop();
        }
    }
}