using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

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

    [Header("Audio")]
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioSource audioSource2;
    [SerializeField] private AudioSource audioSource3;
    [SerializeField] private AudioSource audioSource4;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask layerToHit;

    [Header("Sensors")]
    [SerializeField] private float distanceToCollide;
    [SerializeField] private Transform groundSensor;
    [SerializeField] private Transform wallSensor_RightBottom;
    [SerializeField] private Transform wallSensor_RightTop;
    [SerializeField] private Transform wallSensor_LeftBottom;
    [SerializeField] private Transform wallSensor_LeftTop;
    [SerializeField] private LayerMask ignoreLayers;

    [Header("Spawn World 2")]
    [SerializeField] private Transform spawnPointWorld2;

    [SerializeField] private float currentTimeGame;
    [SerializeField] private GameObject canvas;

    private bool isWallSliding = false;
    private bool hasGrounded = false;
    private bool isRolling = false;
    private int currentAttack = 0;
    private float durationBetweenAttacks = 0;
    private float rollCurrentTime;
    private bool canUseLadder = false;

    public bool canReflect = false;
    public bool isBlocking = false;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        currentTimeGame += Time.deltaTime;
        durationBetweenAttacks += Time.deltaTime;
        if (isRolling)
            rollCurrentTime += Time.deltaTime;

        if (rollCurrentTime > (8 / 14))
            isRolling = false;

        if (!hasGrounded && Knight_Grounded())
        {
            hasGrounded = true;
            animator.SetBool("Grounded", true);
        }

        if (hasGrounded && !Knight_Grounded())
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
        if (Knight_LeftCollision() || Knight_RightCollision())
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
            audioSource3.Play();
            hasGrounded = false;
            animator.SetBool("Grounded", false);
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }

        else if (Input.GetMouseButtonDown(0) && durationBetweenAttacks > 0.25f && !isRolling)
        {
            currentAttack++;
            audioSource.Play();
            if (currentAttack > 3 || durationBetweenAttacks > comboTime)
            {
                currentAttack = 1;
            }

            animator.SetTrigger("Attack" + currentAttack);

            durationBetweenAttacks = 0;
        }

        else if (Input.GetMouseButtonDown(1) && !isRolling)
        {
            audioSource2.Play();
            animator.SetTrigger("Block");
            animator.SetBool("IdleBlock", true);
        }

        else if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool("IdleBlock", false);
            isBlocking = false;
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

    private bool Knight_Grounded()
    {
        Debug.DrawRay(groundSensor.position, Vector3.down * distanceToCollide, Color.green, 0.3f);
        return Physics2D.Raycast(groundSensor.position, Vector3.down, distanceToCollide, ~ignoreLayers);
    }

    private bool Knight_LeftCollision()
    {
        Debug.DrawRay(wallSensor_LeftTop.position, Vector3.left * distanceToCollide, Color.blue, 0.3f);
        Debug.DrawRay(wallSensor_LeftBottom.position, Vector3.left * distanceToCollide, Color.blue, 0.3f);
        return (Physics2D.Raycast(wallSensor_LeftBottom.position, Vector3.left, distanceToCollide, ~ignoreLayers) && Physics2D.Raycast(wallSensor_LeftTop.position, Vector3.left, distanceToCollide, ~ignoreLayers));
    }

    private bool Knight_RightCollision()
    {
        Debug.DrawRay(wallSensor_RightTop.position, Vector3.right * distanceToCollide, Color.blue, 0.3f);
        Debug.DrawRay(wallSensor_RightBottom.position, Vector3.right * distanceToCollide, Color.blue, 0.3f);
        return (Physics2D.Raycast(wallSensor_RightTop.position, Vector3.right, distanceToCollide, ~ignoreLayers) && Physics2D.Raycast(wallSensor_RightBottom.position, Vector3.right, distanceToCollide, ~ignoreLayers));
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

    private void FinishTheGame()
    {
        int hours = Mathf.FloorToInt(currentTimeGame / 3600); // Calcular horas
        int minutes = Mathf.FloorToInt((currentTimeGame % 3600) / 60); // Calcular minutos
        int seconds = Mathf.FloorToInt(currentTimeGame % 60); // 
        string formattedTime = $"{hours:00}:{minutes:00}:{seconds:00}";
        canvas.transform.parent.gameObject.SetActive(true);
        canvas.GetComponent<TMP_Text>().text = "You have finished the game in " + formattedTime;
        StartCoroutine(EndGame());
    }

    IEnumerator EndGame()
    {
        yield return new WaitForSeconds(3f);
        SceneManager.LoadSceneAsync("00_MenuScene");
    }

    public void Reflect()
    {
        canReflect = true;
        StartCoroutine(CanReflectDisable());
    }

    IEnumerator CanReflectDisable()
    {
        yield return new WaitForSeconds(1f);
        canReflect = false;
    }

    public void Block()
    {
        isBlocking = true;
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
        if (collision.CompareTag("Spikes"))
        {
            GetComponent<HealthSystem>().DamageCharacter(25f);
            transform.position = spawnPointWorld2.position;
        }
        if (collision.CompareTag("HiddenZone"))
        {
            Color currentColor = collision.GetComponent<Tilemap>().color;
            currentColor.a = 0.5f;
            collision.GetComponent<Tilemap>().color = currentColor;
        }
        if (collision.CompareTag("Finish"))
        {
            Destroy(collision.gameObject);
            FinishTheGame();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Ladder"))
        {
            canUseLadder = false;
        }
        if (collision.CompareTag("HiddenZone"))
        {
            collision.GetComponent<Tilemap>().color = Color.white;
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
            audioSource4.Play();
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
