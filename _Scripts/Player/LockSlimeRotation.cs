using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeMonkey.HealthSystemCM;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using FirstGearGames.SmoothCameraShaker;

public class LockSlimeRotation : MonoBehaviour
{
    private void LateUpdate()
    {
        // Locks Slime in z axis
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, 130, 0);
    }
}