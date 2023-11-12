using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    /*bool IsMoving
    {
        set
        {
            isMoving = value;
            animator.SetBool("isMoving", isMoving);
        }
    }
    public float moveSpeed = 150f;
    public float maxSpeed = 8f;
    public float idleFriction = 0.9f;

    Rigidbody2D rb;
    Animator animator;
    SpriteRenderer spriteRenderer;
    Vector2 moveInput = Vector2.zero;
    bool isMoving = false;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }
    void FixedUpdate()
    {
    if(moveInput != Vector2.zero)
        {
            rb.velocity = Vector2.ClampMagnitude(rb.velocity + (moveInput * moveSpeed * Time.deltaTime), maxSpeed);
            if(moveInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if(moveInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            IsMoving = true;
        }
        else
        {
            rb.velocity = Vector2.Lerp(rb.velocity, Vector2.zero, idleFriction);
            IsMoving = false;
        }
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }*/

    public float moveSpeed = 1f;

    public float collisionOffset = 0.05f;

    public ContactFilter2D movementFilter;

    public SwordAttack swordAttack;

    Vector2 movementInput;

    SpriteRenderer spriteRenderer;

    Rigidbody2D rb;

    Animator animator;

    bool canMove = true;
    List<RaycastHit2D> castCollisions = new List<RaycastHit2D>();
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void FixedUpdate()
    {
        if (canMove)
        {
            if (movementInput != Vector2.zero)
            {
                bool success = TryMove(movementInput);

                if (!success)
                {
                    success = TryMove(new Vector2(movementInput.x, 0));
                }
                if (!success)
                {
                    success = TryMove(new Vector2(0, movementInput.y));
                }

                animator.SetBool("isMoving", success);
            }
            else
            {
                animator.SetBool("isMoving", false);
            }
            //Change Direction 
            if (movementInput.x < 0)
            {
                spriteRenderer.flipX = true;
            }
            else if (movementInput.x > 0)
            {
                spriteRenderer.flipX = false;
            }

        }
    }
    private bool TryMove(Vector2 direction)
    {
        if (direction != Vector2.zero)
        {
            int count = rb.Cast(
                    direction,
                    movementFilter,
                    castCollisions,
                    moveSpeed * Time.fixedDeltaTime + collisionOffset);
            if (count == 0)
            {
                rb.MovePosition(rb.position + direction * moveSpeed * Time.fixedDeltaTime);
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }
    void OnMove(InputValue movementValue)
    {
        movementInput = movementValue.Get<Vector2>();
    }

    void OnFire()
    {
        animator.SetTrigger("swordAttack");
    }
    public void SwordAttack()
    {
        LockMovement();
        if (spriteRenderer.flipX == true)
        {
            swordAttack.AttackLeft();
        }
        else
        {
            swordAttack.AttackRight();
        }
    }
    public void EndSwordAttack()
    {
        UnlockMovement();
        swordAttack.StopAttack();
    }
    public void LockMovement()
    {
        canMove = false;
    }

    public void UnlockMovement()
    {
        canMove = true;
    }
}
