using UnityEngine;
using UnityEngine.EventSystems;
using Vector3 = UnityEngine.Vector3;
using FirstGearGames.SmoothCameraShaker;
using Quaternion = UnityEngine.Quaternion;

public class PlaySlimeAnimations : MonoBehaviour
{
    [Header("Core Properties")]
    private Animator  _animator;

    private void Start()
    {
        _animator = GetComponent<Animator>();
    }

    private void Update()
    {
        //PlayerMovement();
        PlayerJump();
    }

    // private void PlayerMovement()
    // {
    //     if(Input.GetKey(KeyCode.D))
    //     {
    //         _animator.SetTrigger("Move");
    //     }
    // }

    private void PlayerJump()
    {
        if ((Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W)))
        {
            _animator.SetTrigger("Jump");
        }
    }
}