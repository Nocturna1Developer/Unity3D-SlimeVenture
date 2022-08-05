using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryPickup : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Player")
        {
            Destroy(gameObject, 0.2f);
        }
    }
}
