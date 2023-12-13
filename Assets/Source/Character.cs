using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum MovementType
{
    None = 0,
    Up = 1,
    UpDiagonal = 2,
    Horizontal = 3,
    DownDiagonal = 4,
    Down = 5
}

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    public Rigidbody2D RgBdy;
    public MovementType CurrentMovement = MovementType.None;

    private Vector2 _movementInput;
    private Animator _animator;
    private int _parameterID;
    
    void Start()
    {
        if (!RgBdy)
        {
            RgBdy = GetComponent<Rigidbody2D>();
        }

        RgBdy.constraints = RigidbodyConstraints2D.FreezeRotation;

        if (!_animator)
        {
            _animator = GetComponentInChildren<Animator>();
            _parameterID = Animator.StringToHash("MovementType");
        }
    }
    
    void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        
        _movementInput.Normalize();

        RgBdy.velocity = _movementInput * MoveSpeed;

        HandleAnimation();
    }

    private void HandleAnimation()
    {
        if (_movementInput == Vector2.zero)
        {
            CurrentMovement = MovementType.None;
        }
        else if (_movementInput == Vector2.up)
        {
            CurrentMovement = MovementType.Up;
        }
        else if (_movementInput == Vector2.down)
        {
            CurrentMovement = MovementType.Down;
        }
        else if (_movementInput == Vector2.left || _movementInput == Vector2.right)
        {
            CurrentMovement = MovementType.Horizontal;
        }
        else
        {
            CurrentMovement = (_movementInput.y > 0) ? MovementType.UpDiagonal : MovementType.DownDiagonal;
        }

        if (_movementInput.x != 0)
        {
            FaceRight(_movementInput.x > 0);
        }

        if (!_animator) return;
        var param = _animator.GetInteger(_parameterID);
        if (param != (int)CurrentMovement)
        {
            _animator.SetInteger(_parameterID, (int)CurrentMovement);
        }
    }

    private void FaceRight(bool right)
    {
        Vector3 scale = transform.localScale;
        if ((right && scale.x < 0 ) || (!right && scale.x > 0))
        {
            scale.x *= -1;
        }
        transform.localScale = scale;
    }
}
