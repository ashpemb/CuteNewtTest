using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    public float MoveSpeed = 1.0f;
    public Rigidbody2D RgBdy;

    private Vector2 _movementInput;
    
    // Start is called before the first frame update
    void Start()
    {
        if (RgBdy == null)
        {
            RgBdy = GetComponent<Rigidbody2D>();
        }

        RgBdy.constraints = RigidbodyConstraints2D.FreezeRotation;
    }

    // Update is called once per frame
    void Update()
    {
        _movementInput.x = Input.GetAxisRaw("Horizontal");
        _movementInput.y = Input.GetAxisRaw("Vertical");
        
        _movementInput.Normalize();

        RgBdy.velocity = _movementInput * MoveSpeed;
    }
}
