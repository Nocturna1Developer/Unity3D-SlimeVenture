using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using CodeMonkey.HealthSystemCM;
using System.Collections.Generic;

public class PlayEnemyVoiceLine : MonoBehaviour
{
    [SerializeField] private AudioClip   _voiceLineClip;
    [SerializeField] private AudioSource _voiceLineSource;  

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {       
            case "Player":
                _voiceLineSource.PlayOneShot(_voiceLineClip);
                break;
        }
    }
}
