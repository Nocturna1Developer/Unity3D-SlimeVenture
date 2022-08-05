using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DestoryEnemyProjectile : MonoBehaviour
{
    private float _destroyTime = 7f;
    [SerializeField] private Renderer _spikeRenderer;

    private void Start()
    {
        StartCoroutine(DestroyAfterSeconds());
        _spikeRenderer.enabled = true;
    }
 
    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider collision)  
    {
        switch (collision.gameObject.tag)
        {
            case "Player":
                _spikeRenderer.enabled = false;
                Destroy(gameObject, 1f);
                break;
        }
    }
}
