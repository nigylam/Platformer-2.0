using System.Collections;
using UnityEngine;

public class CharacterSoundController : MonoBehaviour
{
    [SerializeField] private AudioSource _audiosource;
    [SerializeField] private AudioClip _audioJump;
    [SerializeField] private AudioClip _audioDamage;
    [SerializeField] private AudioClip _audioHeal;
    [SerializeField] private CharacterControler _controller;
    [SerializeField] private CharacterCollides _collides;
    [SerializeField] private Character _character;

    private WaitUntil _waitForDisable;
    private float _volume = 1;

    private void OnEnable()
    {
        _character.Dead += DisableSound;
        _controller.Jumped += PlayJumpSound;
        _collides.Damaged += PlayDamageSound;
        _collides.Healed += HealSound;

        _waitForDisable = new WaitUntil(() => _audiosource.isPlaying == false);
    }

    private void OnDisable()
    {
        _character.Dead -= DisableSound;
        _controller.Jumped -= PlayJumpSound;
        _collides.Damaged -= PlayDamageSound;
        _collides.Healed -= HealSound;
    }

    private IEnumerator DisableSoundAfterDelay()
    {
        PlayDamageSound();

        yield return _waitForDisable;

        _audiosource.volume = 0;
    }

    private void HealSound()
    {
        _audiosource.clip = _audioHeal;
        _audiosource.Play();
    }

    private void DisableSound(bool isDisabled)
    {
        if (isDisabled)
        {
            StartCoroutine(DisableSoundAfterDelay());
        }
        else
        {
            StopAllCoroutines();
            _audiosource.volume = _volume;
        }
    }

    private void PlayJumpSound()
    {
        _audiosource.clip = _audioJump;
        _audiosource.Play();
    }

    private void PlayDamageSound()
    {
        _audiosource.clip = _audioDamage;
        _audiosource.Play();
    }
}
