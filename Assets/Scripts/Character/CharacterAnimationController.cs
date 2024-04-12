using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterAnimationController : MonoBehaviour
{
    private readonly int DamagedState = Animator.StringToHash("Damaged");
    private readonly int IdleState = Animator.StringToHash("Idle");
    private readonly int FloatVelocityY = Animator.StringToHash("VelocityY");
    private readonly int FloatVelocityX = Animator.StringToHash("VelocityX");
    private readonly int BoolIsGrounded = Animator.StringToHash("IsGrounded");

    [SerializeField] private CharacterControler _controller;
    [SerializeField] private CharacterCollides _collides;
    [SerializeField] private Character _character;
    [SerializeField] private Animator Animator;

    private float _damageStateTime;
    private WaitForSeconds _damageAnimationDuration;

    private void Update()
    {
        Animator.SetBool(BoolIsGrounded, _controller.IsGrounded());
        Animator.SetFloat(FloatVelocityY, _controller.RigidbodyVelocityY);
        Animator.SetFloat(FloatVelocityX, Mathf.Abs(_controller.RigidbodyVelocityX));
    }

    private void OnEnable()
    {
        _collides.Damaged += PlayDamageAnimation;
        _character.Dead += SetRespawned;

        _damageStateTime = _character.InvincibleTime;
        _damageAnimationDuration = new WaitForSeconds(_damageStateTime);
    }

    private void OnDisable()
    {
        _collides.Damaged -= PlayDamageAnimation;
        _character.Dead -= SetRespawned;
    }

    private void PlayDamageAnimation()
    {
        StartCoroutine(SetDamaged());
    }

    private IEnumerator SetDamaged()
    {
        Animator.Play(DamagedState);
        yield return _damageAnimationDuration;
        Animator.Play(IdleState);
    }

    private void SetRespawned(bool isDead)
    {
        if (isDead == false)
        {
            Animator.Play(IdleState);
        }
        else
        {
            StopAllCoroutines();
            Animator.Play(DamagedState);
        }
    }

}
