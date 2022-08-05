using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using CodeMonkey.HealthSystemCM;
using System.Collections.Generic;

public class EnemyAI : MonoBehaviour
{
    [Header("Core Properties")]
    [SerializeField] private LayerMask _whatIsGround, _whatIsPlayer;
    [SerializeField] private Transform _shootPoint;
    [SerializeField] private Transform _bloodPoint;
    private float _destroyTime = 0.5f;
    private HealthSystem healthSystem;
    private Transform player;

    [Header("Particle Properties")]
    [SerializeField] private GameObject _bloodEffect;
    [SerializeField] private GameObject _deathEffect;

    [Header("Attack Properties")]
    [SerializeField] private float _timeBetweenAttacks;
    [SerializeField] private GameObject _projectile;
    private bool _alreadyAttacked;

    [Header("State Properties")]
    [SerializeField] private float sightRange, attackRange;
    [SerializeField] private bool playerInSightRange, playerInAttackRange;

    [Header("Audio Properties")]
    [SerializeField] private AudioSource _hurtSound; 
    [SerializeField] private AudioSource _attackSound; 
    [SerializeField] private AudioSource _deathSound;

    [Header("Animation Properties")]
    [SerializeField] private Animator   _animator;

    private void Awake()
    {
        healthSystem = new HealthSystem(50);
        healthSystem.OnDead += HealthSystem_OnDead;
        player = GameObject.Find("SlimePlayer3D").transform;
        _animator.SetTrigger("idle");
    }
    
    private void Update()
    {
        //Check for sight and attack range
        playerInSightRange = Physics.CheckSphere(transform.position, sightRange, _whatIsPlayer);
        playerInAttackRange = Physics.CheckSphere(transform.position, attackRange, _whatIsPlayer);

        if (playerInAttackRange && playerInSightRange)
        {
            AttackPlayer();
            _animator.SetTrigger("attack");
        } 
    }

    private void AttackPlayer()
    {
        transform.LookAt(player);

        if (!_alreadyAttacked)
        {
            // Attack code here
            _attackSound.Play();
            Rigidbody spikeBall = Instantiate(_projectile, _shootPoint.position, _shootPoint.rotation).GetComponent<Rigidbody>();
            spikeBall.AddForce(transform.forward * 5, ForceMode.Impulse);
            spikeBall.AddForce(transform.up * 5f, ForceMode.Impulse);

            ///End of attack code
            _alreadyAttacked = true;
            Invoke(nameof(ResetAttack), _timeBetweenAttacks);
        }
    }

    private void ResetAttack()
    {
        _alreadyAttacked = false;
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRange);
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, sightRange);
    }

    public HealthSystem GetHealthSystem()
    {
        return healthSystem;
    }
    
    private void HealthSystem_OnDead(object sender, System.EventArgs e) 
    {
        _deathSound.Play();
        _animator.SetTrigger("death");
        _alreadyAttacked = true;
        Instantiate(_deathEffect, transform.position, transform.rotation);
        StartCoroutine(DestroyAfterSeconds());
    }
 
    private IEnumerator DestroyAfterSeconds()
    {
        yield return new WaitForSeconds(_destroyTime);
        Destroy(gameObject);
    }

    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "PlayerProjectile":
                _hurtSound.Play();
                _animator.SetTrigger("GetHit");
                healthSystem.Damage(10);
                Instantiate(_bloodEffect, _bloodPoint.position, _bloodPoint.rotation);
                break;

            case "Player":
                _hurtSound.Play();
                _animator.SetTrigger("GetHit");
                healthSystem.Damage(10);
                Instantiate(_bloodEffect, _bloodPoint.position, _bloodPoint.rotation);
                break;
        }
    }
}
