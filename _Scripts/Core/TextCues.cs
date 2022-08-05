using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class TextCues: MonoBehaviour
{
    [SerializeField] private GameObject _levelNameTitle;
    [SerializeField] private AudioSource _cueSound;

    private void Start()
    {
        _levelNameTitle.SetActive(false);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player")
        {
            _levelNameTitle.SetActive(true);
            _cueSound.Play();
            StartCoroutine(DestroyAfterDelay());
        }
    }

    private IEnumerator DestroyAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        Destroy(_levelNameTitle);
        Destroy(gameObject);
    }
}
