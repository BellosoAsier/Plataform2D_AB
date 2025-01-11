using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class KoboldBehaviour : MonoBehaviour
{
    [SerializeField] private Transform[] listPointsOfPath;
    [SerializeField] private float velocityMovement;
    // Start is called before the first frame update
    private Transform playerObject;
    private Vector3 pointDestination;
    private int index_listPointsOfPath;
    private Animator animator;

    [Header("Attack Settings")]
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackDamage;
    [SerializeField] private float attackRange;
    [SerializeField] private float attackRate;
    [SerializeField] private LayerMask layerToHit;

    private Rigidbody2D rb;
    private int direction;

    private bool canAttack;

    void Start()
    {
        canAttack = true;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.Find("DetectionPoint_Player").transform;
        index_listPointsOfPath = 0;
        pointDestination = listPointsOfPath[index_listPointsOfPath].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<KoboldRaycastDetection>().hasDetectedPlayer)
        {
            LookAtDestinationPoint(pointDestination);
            MovementToWaypoints();
        }
        else
        {
            if (playerObject != null)
            {
                LookAtDestinationPoint(playerObject.position);
                MovementTowardsPlayer();
                if (GetComponent<KoboldRaycastDetection>().isInsideAttackRange && canAttack)
                {
                    StartCoroutine(AttackKobold(attackRate));
                }
            }
        }
    }

    IEnumerator AttackKobold(float seconds)
    {
        canAttack = false;
        animator.SetTrigger("Attack");
        Collider2D[] listColisions = Physics2D.OverlapCircleAll(attackPoint.position, attackRange, layerToHit);

        foreach (Collider2D collision in listColisions)
        {
            collision.gameObject.GetComponent<HealthSystem>().DamageCharacter(attackDamage);
        }
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }
    private void LookAtDestinationPoint(Vector3 other)
    {
        if (other.x > transform.position.x)
        {
            direction = 1;
            transform.rotation = Quaternion.identity;
        }
        else
        {
            direction = -1;
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
    }

    private void MovementToWaypoints()
    {
        animator.SetBool("isRunning", false);
        float distanceBetweenPoint = Mathf.Abs(transform.position.x - listPointsOfPath[index_listPointsOfPath].position.x);
        if (distanceBetweenPoint > 0.1f)
        {
            rb.velocity = new Vector2(direction * velocityMovement, rb.velocity.y);
        }
        else
        {
            ChangeDestinationPoint();
        }
    }

    private void MovementTowardsPlayer()
    {
        animator.SetBool("isRunning", true);
        rb.velocity = new Vector2(direction * (velocityMovement + 0.5f), rb.velocity.y);
    }

    private void ChangeDestinationPoint()
    {
        index_listPointsOfPath++;

        if(index_listPointsOfPath >= listPointsOfPath.Length)
        {
            index_listPointsOfPath = 0;
        }
        
        pointDestination = listPointsOfPath[index_listPointsOfPath].position;
    }

    public void SetListPointOfPath(Transform[] list)
    {
        listPointsOfPath = list;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.grey;
        Gizmos.DrawWireSphere(attackPoint.position, attackRange);
    }
}
