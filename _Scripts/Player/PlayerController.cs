using TMPro;
using System;
using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using CodeMonkey.HealthSystemCM;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using FirstGearGames.SmoothCameraShaker;

public class PlayerController : MonoBehaviour, IGetHealthSystem 
{
    [Header("Core Properties")]
    [SerializeField] private float      _jumpPower      = 5.0f;
    [SerializeField] private float      _launchVelocity = 10f;
    [SerializeField] private GameObject _slimeBody;

    private float        _distToGround;
    private GameMaster   _gameMaster;
    private Rigidbody    _ballRigidbody;
    private HealthSystem healthSystem;
    

    [Header("Weapon Properties")]
    [SerializeField] private float _ammoCount = 10f;
    [SerializeField] private float _fireRate = 1f;
    [SerializeField] private GameObject _projectile;
    [SerializeField] private Transform  _shootPoint;
    private float _nextFireTime;
    private float _minAmmoCount = 0f;

    [Header("Particle Properties")]
    [SerializeField] private GameObject     _bloodEffect;
    [SerializeField] private GameObject     _deathEffect;
    [SerializeField] private GameObject     _ammoCollectedEffect;
    [SerializeField] private GameObject     _coinCollectedEffect;
    [SerializeField] private GameObject     _healEffect;
    [SerializeField] private GameObject     _poisonEffect;
    [SerializeField] private GameObject     _checkpointEffect;
    [SerializeField] private GameObject     _dustEffect;
    [SerializeField] private ParticleSystem _muzzleFlash;

    [Header("Camera Properties")]
    [SerializeField] private ShakeData _slimeJumpShakeData = null;
    [SerializeField] private ShakeData _shootShakeData = null;
    private ShakerInstance             _offRoadInstance;
    private Vector3                    _playerLook;

    [Header("Audio Properties")]
    [SerializeField] private AudioSource _bounceSound; 
    [SerializeField] private AudioSource _healSound; 
    [SerializeField] private AudioSource _deathSound;  
    [SerializeField] private AudioSource _hurtSound;  
    [SerializeField] private AudioSource _coinSound;  
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _ammoPickupSound;
    [SerializeField] private AudioSource _ammoEmptySound;
    [SerializeField] private AudioSource _checkpointSound; 
    [SerializeField] private AudioSource _levelClearSound; 

    // [Header("HUD Properties")] 
    // [SerializeField] private RectTransform   _faderImage;
    // [SerializeField] private GameObject      _levelClearCanvas;
    // [SerializeField] private TextMeshProUGUI _ammoCountUI;

    private void Awake()
    {

        healthSystem = new HealthSystem(150);
        healthSystem.OnDead += HealthSystem_OnDead;

        //_levelClearCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.Confined;
    }

    private void Start()
    {
        // Sets player spawn location
        _gameMaster = GameObject.FindGameObjectWithTag("GameMaster").GetComponent<GameMaster>();
        transform.position = _gameMaster.lastCheckpointPosition;
        _ballRigidbody = this.GetComponent<Rigidbody>();
        _distToGround = GetComponent<Collider>().bounds.extents.y;
    }

    private void FixedUpdate()
    {
        if (Input.GetMouseButton(1) && _ammoCount > _minAmmoCount)
        {
            Shoot();
        }
        else if(Input.GetMouseButton(1) && _ammoCount == 0)
        {
            _ammoEmptySound.Play();
        }
        else
        {
            if(_muzzleFlash.isPlaying) 
            {
                _muzzleFlash.Stop();
            }
        }
    }

    private void LateUpdate()
    {
        // Locks Slime in z axis
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, transform.localEulerAngles.y, 0);
    }

    private void Shoot()
    {
        if(Time.time > _nextFireTime)
        {
            _nextFireTime = Time.time + _fireRate;
            _shootSound.Play();
            if(!_muzzleFlash.isPlaying) _muzzleFlash.Play();
        
            Rigidbody slimeBall = Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation).GetComponent<Rigidbody>();
            slimeBall.AddForce(_launchVelocity, 0, 0, ForceMode.Impulse);

            _ammoCount--;
            //_ammoCountUI.text = "Slime Ammo: " + _ammoCount.ToString();
            _offRoadInstance = CameraShakerHandler.Shake(_shootShakeData);
        }
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }

    private void HealthSystem_OnDead(object sender, System.EventArgs e) 
    {
        _deathSound.Play();
        _ammoCount = 20f;
        _slimeBody.SetActive(false);
        Instantiate(_deathEffect, transform.position, transform.rotation);
        _ballRigidbody.isKinematic = true;
        StartCoroutine(RestartLevelAfterDelay(1f));
    }

    private void OnCollisionEnter(Collision collision)  
    {
        switch (collision.gameObject.tag)
        {
            case "Dead":
                _deathSound.Play();
                _ammoCount = 20f;
                _slimeBody.SetActive(false);
                _ballRigidbody.isKinematic = true;
                Instantiate(_deathEffect, transform.position, transform.rotation);
                StartCoroutine(RestartLevelAfterDelay(1f));
                break;

            case "EnemyHead":
                _bounceSound.Play();
                _ballRigidbody.AddForce(0, 5f, 0, ForceMode.Impulse);
                break;

            default:
                break;
        }
    }

    private void OnCollisionExit(Collision collision)  
    {
        CameraShakerHandler.StopAll();
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "UnsafePlatform":
                _bounceSound.Play();
                healthSystem.Damage(5);
                _ballRigidbody.AddForce(0, 5f, 0, ForceMode.Impulse);
                
                if (_offRoadInstance != null)
                {
                    _offRoadInstance.FadeOut();
                    _offRoadInstance = null;
                }
                else
                {
                    _offRoadInstance = CameraShakerHandler.Shake(_slimeJumpShakeData);
                }
                break;
            
            case "SafePlatform":
                _bounceSound.Play();
                Instantiate(_dustEffect, transform.position, transform.rotation);

                if (_offRoadInstance != null)
                {
                    _offRoadInstance.FadeOut();
                    _offRoadInstance = null;
                }
                else
                {
                    _offRoadInstance = CameraShakerHandler.Shake(_slimeJumpShakeData);
                }
                break;
            
            case "Enemy":
                _bounceSound.Play();
                _hurtSound.Play();
                _ballRigidbody.AddForce(-15f, 0, 0, ForceMode.Impulse);
                healthSystem.Damage(5);
                break;

            case "Heal":
                _healSound.Play();
                healthSystem.Heal(20);
                Instantiate(_healEffect, transform.position, transform.rotation);
                break;

            case "Damage":
                _hurtSound.Play();
                Instantiate(_bloodEffect, transform.position, transform.rotation);
                healthSystem.Damage(10);

                if (_offRoadInstance != null)
                {
                    _offRoadInstance.FadeOut();
                    _offRoadInstance = null;
                }
                else
                {
                    _offRoadInstance = CameraShakerHandler.Shake(_slimeJumpShakeData);
                }

                break;

            case "Checkpoint":
                _checkpointSound.Play();
                Instantiate(_checkpointEffect, transform.position, transform.rotation);
                break;

            case "Coin":
                _coinSound.Play();
                SumScore.Add(15);
                Instantiate(_coinCollectedEffect, transform.position, transform.rotation);
                break;

            case "AmmoPickup":
                _ammoPickupSound.Play();
                _ammoCount = _ammoCount + 10f;
                Instantiate(_ammoCollectedEffect, transform.position, transform.rotation);
                break;

            case "LevelClear":
                Instantiate(_checkpointEffect, transform.position, transform.rotation);
                //_levelClearCanvas.SetActive(true);
                _ballRigidbody.isKinematic = true;
                _levelClearSound.Play();
                StartCoroutine(LoadNextlevelAfterDelay(1f));
                break;
            
            default:
                break;
        }
    }

    private IEnumerator RestartLevelAfterDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    public IEnumerator LoadNextlevelAfterDelay(float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
}