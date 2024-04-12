using System;
using UnityEngine;

public class CharacterControler : MonoBehaviour
{
    private const string Horizontal = nameof(Horizontal);
    private const string Vertical = nameof(Vertical);

    public event Action Jumped;

    [SerializeField] private Rigidbody2D _rigidbody;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundLayer;
    [SerializeField] private Character _character;
    [SerializeField] private CharacterCollides _collides;
    [SerializeField] private Game _game;
    [SerializeField] private float _speed;
    [SerializeField] private float _jumpForce;

    public float RigidbodyVelocityY { get => _rigidbody.velocity.y; }
    public float RigidbodyVelocityX { get => _rigidbody.velocity.x; }

    private float _horizontal;
    private bool _isFacingRight = true;
    private bool _canDoSecondJump = true;
    private bool _isDisable = false;
    private Vector2 _startPosition;

    private void Start()
    {
        _startPosition = transform.position;
    }

    private void Update()
    {
        Move();
        Jump();
        Flip();
    }

    private void FixedUpdate()
    {
        if (_isDisable == false)
            _rigidbody.velocity = new Vector2(_horizontal * _speed, _rigidbody.velocity.y);
    }

    private void OnEnable()
    {
        _character.Dead += SetDisable;
        _game.Won += SetDisable;
        _collides.JumpEnemy += JumpEnemy;
    }

    private void OnDisable()
    {
        _character.Dead -= SetDisable;
        _game.Won -= SetDisable;
        _collides.JumpEnemy -= JumpEnemy;
    }

    private void JumpEnemy()
    {
        _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
    }

    private void SetDisable(bool isDisable)
    {
        _isDisable = isDisable;
        _rigidbody.velocity = Vector2.zero;

        if(_isDisable == false)
            transform.position = _startPosition;
    }

    private void SetDisable()
    {
        _isDisable = true;
    }

    public bool IsGrounded()
    {
        float radius = 0.2f;

        return Physics2D.OverlapCircle(_groundCheck.position, radius, _groundLayer);
    }

    private void Jump()
    {
        if (Input.GetKeyDown(KeyCode.W) && CanJump() && _isDisable == false)
        {
            Jumped?.Invoke();

            if (IsGrounded() == false)
                _canDoSecondJump = false;
            else
                _canDoSecondJump = true;

            _rigidbody.velocity = new Vector2(_rigidbody.velocity.x, _jumpForce);
        }
    }

    private bool CanJump() => IsGrounded() || _canDoSecondJump;

    private void Move()
    {
        _horizontal = Input.GetAxisRaw(Horizontal);
    }

    private void Flip()
    {
        if (_isFacingRight && _horizontal < 0 || _isFacingRight == false && _horizontal > 0)
        {
            _isFacingRight = !_isFacingRight;
            Vector2 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }
}
