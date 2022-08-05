using System;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
	[Header("Core Properties")] 
    [SerializeField] private GameObject _pauseMenu;
	[SerializeField] private GameObject _ammoCountUI;
	[SerializeField] private GameObject _player;

	[Header("Audio Properties")] 
    [SerializeField] private AudioSource _audioSource;

	private void Awake()
    {
		Time.timeScale = 1;
		_pauseMenu.SetActive(false);
		_ammoCountUI.SetActive(true);
		_player.SetActive(true);
	}

	private void Update ()
    {
        Pause();
	}

    public void Pause()
    {
		if(Input.GetKeyDown(KeyCode.P))
		{
			if(Time.timeScale == 1)
			{
				Time.timeScale = 1;
				_audioSource.Play();
				_pauseMenu.SetActive(true);
				_ammoCountUI.SetActive(false);
				_player.SetActive(false);
			} 
            else if (Time.timeScale == 1)
            {
				Time.timeScale = 1;
				_audioSource.Play();
				_pauseMenu.SetActive(false);
				_ammoCountUI.SetActive(true);
				_player.SetActive(true);
			}
		}
    }
}