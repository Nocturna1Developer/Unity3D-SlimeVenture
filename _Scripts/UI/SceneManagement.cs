//using TMPro;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class SceneManagement : MonoBehaviour
{
    [SerializeField] private RectTransform _faderImage;

    private void Start()
    {
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 1, 0);
        LeanTween.alpha(_faderImage, 0, 1f).setOnComplete(() => {
            _faderImage.gameObject.SetActive(false);
        });
    }

    public void LoadNextLevel()
    {
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            StartCoroutine(LoadNextlevelAfterDelay(1f));
        });
    }

    public void LoadMainMenu()
    { 
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            SceneManager.LoadScene(0);
        });
    }

    public void LoadLevelOne()
    { 
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            SceneManager.LoadScene(1);
        });
    }

    public void LoadLevelTwo()
    { 
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            SceneManager.LoadScene(2);
        });
    }

    public void LoadLevelThree()
    { 
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            SceneManager.LoadScene(3);
        });
    }

    public void RestartLevel()
    { 
        _faderImage.gameObject.SetActive(true);
        LeanTween.alpha(_faderImage, 0, 0);
        LeanTween.alpha(_faderImage, 1, 2f).setOnComplete(() => {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        });
    }

    public void QuitApplication()
    {
        Application.Quit();
    }

    public IEnumerator LoadNextlevelAfterDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}