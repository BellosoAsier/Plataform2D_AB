using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroKnight : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float inputH;
    private float inputV;

    [Header("Properties")]
    [SerializeField] private float velocityForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float comboTime;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask layerToHit;

    [Header("Sensors")]
    [SerializeField] private KnightCollisionDetector groundSensor;
    [SerializeField] private KnightCollisionDetector wallSensor_RightBottom;
    [SerializeField] private KnightCollisionDetector wallSensor_RightTop;
    [SerializeField] private KnightCollisionDetector wallSensor_LeftBottom;
    [SerializeField] private KnightCollisionDetector wallSensor_LeftTop;

    private bool isWallSliding = false;
    private bool hasGrounded = false;
    private bool isRolling = false;
    private int currentAttack = 0;
    private float durationBetweenAttacks = 0;
    private float rollCurrentTime;
    private bool canUseLadder = false; 

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        durationBetweenAttacks += Time.deltaTime;

        if (isRolling)
            rollCurrentTime += Time.deltaTime;

        if (rollCurrentTime > (8 / 14))
            isRolling = false;

        if (!hasGrounded && groundSensor.CurrentState())
        {
            hasGrounded = true;
            animator.SetBool("Grounded", true);
        }

        if (hasGrounded && !groundSensor.CurrentState())
        {
            hasGrounded = false;
            animator.SetBool("Grounded", false);
        }



        Movement();

        animator.SetFloat("AirSpeedY", rb.velocity.y);

        Handle();
    }

    private void Handle()
    {
        if ((wallSensor_RightTop.CurrentState() && wallSensor_RightBottom.CurrentState()) || (wallSensor_LeftTop.CurrentState() && wallSensor_LeftBottom.CurrentState()))
        {
            isWallSliding = true;
            animator.SetBool("WallSlide", true);
        }
        else
        {
            isWallSliding = false;
            animator.SetBool("WallSlide", false);
        }

        if (Input.GetKeyDown(KeyCode.Space) && hasGrounded && !isRolling)
        {
            animator.SetTrigger("Jump");
            hasGrounded = false;
            animator.SetBool("Grounded", false);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            groundSensor.DisableCollisionDetector(0.2f);
        }

        else if (Input.GetMouseButtonDown(0) && durationBetweenAttacks > 0.25f && !isRolling)
        {
            currentAttack++;

            if (currentAttack > 3 || durationBetweenAttacks > comboTime)
            {
                currentAttack = 1;
            }

            animator.SetTrigger("Attack" + currentAttack);

            durationBetweenAttacks = 0;
        }

        else if (Input.GetMouseButtonDown(1) && !isRolling)
        {
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IdleBlock", false);
        }

        else if (Input.GetKeyDown(KeyCode.LeftShift) && !isRolling && !isWallSliding)
        {
            isRolling = true;
            animator.SetTrigger("Roll");
            rb.velocity = new Vector2(inputH * velocityForce, rb.velocity.y);
        }
    }

    private void Movement()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        inputV = Input.GetAxisRaw("Vertical");

        if (inputH != 0)
        {
            animator.SetBool("Run", true);
            if (inputH > 0)
            {
                transform.eulerAngles = Vector3.zero;
            }
            else
            {
                transform.eulerAngles = new Vector3(0, 180, 0);
            }
        }
        else
        {
            animator.SetBool("Run", false);
        }

        if (!isRolling)
        {
            rb.velocity = new Vector2(inputH * velocityForce, rb.velocity.y);
        }

        if (canUseLadder)
        {
            rb.velocity = new Vector2(rb.velocity.x, inputV * velocityForce);
        }
    }

    //Se ejecuta desde evento de animacion (Knight)
    public void Attack()
    {
        Collider2D[] listColisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerToHit);

        foreach (Collider2D collision in listColisions)
        {
            collision.gameObject.GetComponent<HealthSystem>().DamageCharacter(attackDamage);
        }
    }

    //////////////
    //COLISIONES//
    //////////////
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canUseLadder = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canUseLadder = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovePlatform"))
        {
            transform.parent = collision.transform;
        }
        if (collision.gameObject.CompareTag("Coin"))
        {
            GetComponent<InventoryScript>().Money += 1;
            Destroy(collision.gameObject);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("MovePlatform"))
        {
            transform.parent = null;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(attackPoint.position,attackRange);
    }
}
