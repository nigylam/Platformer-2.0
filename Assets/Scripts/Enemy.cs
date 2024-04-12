using System.Collections;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private readonly int DieAnimation = Animator.StringToHash("Die");
    private readonly string AnimatorBaseLayer = "Base Layer";

    [SerializeField] private Animator _animator;
    [SerializeField] private Transform[] _patrolPoints;
    [SerializeField] private Collider2D _bodyCollider;
    [SerializeField] private Collider2D _weekPlaceCollider;
    [SerializeField] private Gem _gemReward;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private Character _character;
    [SerializeField] private float _speed;
    [SerializeField] private float _speedCharacterPursuing = 1.1f;
    [SerializeField] private float _closeDistanceToTarget = 2f;
    [SerializeField] private float _closeDistanceToCharacter = 4f;

    public float JumpPadForce { get; private set; } = 3f;

    private bool _isFacingRight = false;
    private int _currentPatrolPoint = 0;
    private float _horizontalMoving = -1;
    private float _usualSpeed;
    private Vector2 _startPosition;

    private void Start()
    {
        _usualSpeed = _speed;
        _startPosition = transform.position;
    }

    private void Update()
    {
        Patrolling();
        Flip();
    }

    public void Die()
    {
        _speed = 0;
        _bodyCollider.enabled = false;
        _audioSource.Play();
        SpawnReward();
        StartCoroutine(PlayDie());
    }

    public void Respawn()
    {
        gameObject.SetActive(true);
        _bodyCollider.enabled = true;
        _weekPlaceCollider.enabled = true;
        _speed = _usualSpeed;
        transform.position = _startPosition;
    }

    private void SpawnReward()
    {
        _gemReward.gameObject.SetActive(true);
        _gemReward.transform.position = transform.position;
    }

    private IEnumerator PlayDie()
    {
        _animator.Play(DieAnimation);

        yield return new WaitForSeconds(_animator.GetCurrentAnimatorStateInfo(_animator.GetLayerIndex(AnimatorBaseLayer)).length);

        gameObject.SetActive(false);
    }

    private void Patrolling()
    {
        Vector2 targetPosition;

        if (Vector2.Distance(transform.position, _character.transform.position) <= _closeDistanceToCharacter && _character.IsInvincible == false)
        {
            targetPosition = _character.transform.position;
            _speed = _speedCharacterPursuing;
        }
        else
        {
            if (Vector2.Distance(transform.position, _patrolPoints[_currentPatrolPoint].position) <= _closeDistanceToTarget)
                _currentPatrolPoint = (_currentPatrolPoint + 1) % _patrolPoints.Length;

            targetPosition = _patrolPoints[_currentPatrolPoint].position;
            _speed = _usualSpeed;
        }

        _horizontalMoving = Mathf.Clamp(targetPosition.x - transform.position.x, -1, 1);

        Move();
    }

    private void Move() => transform.Translate(_horizontalMoving * _speed * Time.deltaTime, 0f, 0f);

    private void Flip()
    {
        if (_isFacingRight && _horizontalMoving < 0 || _isFacingRight == false && _horizontalMoving > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
