using System;
using UnityEngine;

public class CharacterCollides : MonoBehaviour
{
    public event Action Damaged;
    public event Action GemCollected;
    public event Action JumpEnemy;
    public event Action Healed;

    [SerializeField] private Character _character;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (_character.IsInvincible == false)
        {
            if (other.gameObject.GetComponent<Gem>() != null)
            {
                //StartCoroutine(other.gameObject.GetComponent<Gem>().Disapear());
                GemCollected?.Invoke();
            }

            if (other.gameObject.GetComponent<EnemyBody>() != null)
                Damaged?.Invoke();

            if (other.gameObject.GetComponent<EnemyWeakPlace>() != null)
                JumpEnemy?.Invoke();

            if (other.gameObject.GetComponent<Healing>() != null && _character.Health != _character.MaxHealth)
            {
                Healed?.Invoke();
                other.gameObject.SetActive(false);
            }
        }
    }
}
