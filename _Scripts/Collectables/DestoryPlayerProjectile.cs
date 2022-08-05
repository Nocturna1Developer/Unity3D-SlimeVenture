using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestoryPlayerProjectile : MonoBehaviour
{
    
    private float _destroyTime = 5f;

    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds());
    }

    // private void LateUpdate()
    // {
    //     transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    // }
 
    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)  
    {
        switch (collision.gameObject.tag)
        {  
            case "Enemy":
                Destroy(gameObject);
                break;
        }
    }
}
