using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroKnight : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float inputH;
    [SerializeField] private float velocityForce;
    [SerializeField] private float jumpForce;
    [SerializeField] private float comboTime;
    private Animator animator;

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
    }
}
