using System;
using System.Collections;
using UnityEngine;

public class Character : MonoBehaviour
{
    public event Action<bool> Dead;

    [SerializeField] private CharacterAnimationController _animationController;
    [SerializeField] private CharacterControler _controller;
    [SerializeField] private CharacterCollides _collides;
    [SerializeField] private Game _game;

    private WaitForSeconds _invincibleTimeWait;

    public float InvincibleTime { get; private set; } = 2f;
    public bool IsInvincible { get; private set; } = false;
    public int Health { get; private set; }
    public int MaxHealth { get; private set; } = 2;

    private void Start()
    {
        Health = MaxHealth;
        _invincibleTimeWait = new WaitForSeconds(InvincibleTime);
    }

    private void OnEnable()
    {
        _collides.Damaged += GetDamage;
        _game.Restarted += Respawn;
        _collides.Healed += Heal;
    }

    private void OnDisable()
    {
        _collides.Damaged -= GetDamage;
        _game.Restarted -= Respawn;
        _collides.Healed -= Heal;
    }

    private void Heal()
    {
        Health++;
    }

    private void GetDamage()
    {
        if (IsInvincible == false)
        {
            Health--;

            if (Health <= 0)
            {
                IsInvincible = true;
                Dead?.Invoke(true);
            }
            else
            {
                IsInvincible = true;
                StartCoroutine(BecomeInvincible());
            }
        }
    }

    private IEnumerator BecomeInvincible()
    {
        yield return _invincibleTimeWait;
        IsInvincible = false;
    }

    public void Respawn()
    {
        IsInvincible = false;
        Health = MaxHealth;
        Dead?.Invoke(false);
    }
}
