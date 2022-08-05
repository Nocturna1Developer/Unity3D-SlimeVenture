using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryParticles : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject, 2f);
    }
}
