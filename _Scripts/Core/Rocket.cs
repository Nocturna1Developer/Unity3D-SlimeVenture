// using UnityEngine;
// using UnityEngine.UI;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine.SceneManagement;

// public class Rocket : MonoBehaviour
// {
//     private float maxHealth = 100f;
//     [SerializeField] float currentHealth = 100f;
//     [SerializeField] private Slider healthbarSlider;

//     [SerializeField] float rcsThrust = 300f;
//     [SerializeField] float mainThrust = 50f;
//     [SerializeField] float levelLoadDelay = 2f;

//     [SerializeField] AudioClip mainEngine;
//     [SerializeField] AudioClip success;
//     [SerializeField] AudioClip death;

//     [SerializeField] ParticleSystem mainEngineParticles;
//     [SerializeField] ParticleSystem successParticles;
//     [SerializeField] ParticleSystem deathParticles;

//     Rigidbody rigidBody;
//     AudioSource audioSource;

//     bool isTransitioning = false;
//     bool collisionsDisabled = false;

//     void Start()
//     {
//         currentHealth = maxHealth;
//         rigidBody = GetComponent<Rigidbody>();
//         audioSource = GetComponent<AudioSource>();
//     }

//     void FixedUpdate()
//     {
//         if(!isTransitioning)
//         {
//             RespondToThrustInput();
//             RespondToRotateInput();
//             successParticles.Play();
//         }
//     }

//     private void LoadNextLevel()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
//     }

//     private void LoadCurrentLevel()
//     {
//         SceneManager.LoadScene(SceneManager.GetActiveScene().name);
//     }
    
//     // Reports when there has been a collision and delays the level restarting when you die
//     void OnCollisionEnter(Collision collision)
//     {
//         SetPlayerHealthbarUI();

//         if (isTransitioning || collisionsDisabled) { return; }

//         switch (collision.gameObject.tag)
//         {
//             case "Friendly":
//                 break;

//             case "Finish":
//                 StartSuccessSequence();
//                 break;

//             case "Fuel":
//                 fuelSlider.value -= 10;
//                 //EndFuelSequence();
//                 break;

//             case "Dead":
//                 healthbarSlider.value -= 30;
//                 currentHealth -= 30;
//                 StartDeathSequence();
//                 break;

//             default:
//                 break;
//         }
//     }

//     // Plays the jingle sound when you win and stops all other sounds
//     private void StartSuccessSequence()
//     {
//         currentHealth += 50;
//         isTransitioning = true;
//         audioSource.Stop();
//         audioSource.PlayOneShot(success);
//         Invoke("LoadNextLevel", levelLoadDelay); 
//     }

//     // Plays the death sound when you win and stops all other sounds 
//     private void StartDeathSequence()
//     {
//         if (currentHealth <= 0)
//         {
//             //StartCoroutine(cameraShake.Shake(.5f, .5f));
//             isTransitioning = true;
//             audioSource.Stop();
//             audioSource.PlayOneShot(death);
//             deathParticles.Play();
//             Invoke("LoadCurrentLevel", levelLoadDelay); 
//         }
//     }

//     // Plays teh audio when spacebar is pressed and stops when it is let go
//     private void RespondToThrustInput()
//     {
//         if (Input.GetKey(KeyCode.Space)) 
//         {
//             ApplyThrust();
//             fuelSlider.value -= 1;
//         }
//         else
//         {
//             StopApplyingThrust();
//         }
//     }

//     // Stops the motion of the rocket
//     private void StopApplyingThrust()
//     {
//         audioSource.Stop();
//         mainEngineParticles.Stop();
//     }

//     // Makes it so that the audio won't layer over each other and gives the rocket force and plays particles when thrust
//     private void ApplyThrust()
//     {
//         rigidBody.AddRelativeForce(Vector3.up * mainThrust);
//         if (!audioSource.isPlaying) 
//         {
//             audioSource.PlayOneShot(mainEngine);
//         }
//         mainEngineParticles.Play();
//     }

//     // The rotation will be forzen on the z axis and this function will take manual contorl of the rocket
//     private void RespondToRotateInput()
//     {
    
//         if (Input.GetKey(KeyCode.A))
//         {
//             RotateManually(rcsThrust);
//         }

//         else if (Input.GetKey(KeyCode.D))
//         {
//             RotateManually(-rcsThrust);
//         }
//     }

//     // The rotation will unfreze moving 
//     private void RotateManually(float rotationThisFrame)
//     {
//         rigidBody.freezeRotation = true; // take manual control of rotation
//         transform.Rotate(Vector3.forward * rotationThisFrame * Time.deltaTime) ;

//         rigidBody.freezeRotation = false;
//     }
// }