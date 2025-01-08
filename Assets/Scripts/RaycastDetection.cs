using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public enum EnemyType { Kobold, Demon, Snail, Bee}
public class RaycastDetection : MonoBehaviour
{
    [Header("ENEMY TYPE")]
    [SerializeField] private EnemyType enemyType;

    [Header("CHARACTERISTICS")]
    [SerializeField] private Transform target;  // Objeto que quieres detectar
    [SerializeField] private float detectionRange;  // Rango de detección
    [SerializeField] private float attackRange;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private string tagName;

    [Header("JUMP")]
    [SerializeField] private Transform pointForJump;
    [SerializeField] private float jumpForce;
    [SerializeField] private float jumpRange;
    [SerializeField] private LayerMask worldLayer;

    [Header("FLAGS")]
    public bool hasDetectedPlayer = false;
    public bool isInsideAttackRange = false;
    public bool canJump;

    private void Start()
    {
        target = GameObject.Find("DetectionPoint_Player").transform;
    }
    void Update()
    {
        switch (enemyType)
        {
            case EnemyType.Kobold:
                KoboldDetections();
                JumpDetection();
                break;

            case EnemyType.Demon:
                
                break;

            case EnemyType.Snail:
                
                break;

            case EnemyType.Bee:
                
                break;

            default:
                break;
        }

        
    }

    private void KoboldDetections()
    {
        // Calculamos la dirección desde este objeto hacia el objetivo
        Vector3 directionToTarget = target.position - transform.position;

        // Verificamos si el objetivo está dentro del rango
        if (directionToTarget.magnitude <= detectionRange)
        {
            Debug.DrawRay(transform.position, directionToTarget.normalized * detectionRange, Color.red);

            RaycastHit2D hit = Physics2D.Raycast(transform.position, directionToTarget.normalized, detectionRange, ~ignoreLayer);

            // Realizamos el raycast
            if (hit.collider != null)
            {
                // Verificamos si el objeto detectado es el objetivo
                if (hit.collider.CompareTag(tagName))
                {
                    if (directionToTarget.magnitude <= attackRange)
                    {
                        isInsideAttackRange = true;
                    }
                    else
                    {
                        isInsideAttackRange = false;
                    }
                    hasDetectedPlayer = true;
                    Debug.Log("El objetivo está dentro del rango y no hay bloqueos.");
                    return;
                }
            }
        }
        isInsideAttackRange = false;
        hasDetectedPlayer = false;
    }

    private void JumpDetection()
    {
        // Dirección del eje rojo (eje X positivo)
        Vector2 direction = pointForJump.right;

        // Dibujar el raycast para visualizarlo
        Debug.DrawLine(pointForJump.position, pointForJump.position + (Vector3)direction, Color.cyan);

        // Lanzar el raycast en el eje X positivo con un rango de 1 unidad
        RaycastHit2D hit = Physics2D.Raycast(pointForJump.position, direction, jumpRange, worldLayer);

        if (hit.collider != null)
        {
            // Si colisiona con algo, ejecuta una función
            if (canJump)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                canJump = false;
                StartCoroutine(WaitSecondorJump());
            }
            Debug.Log("Salto: " + hit.collider.name);
        }
    }

    IEnumerator WaitSecondorJump()
    {
        yield return new WaitForSeconds(1f);
        canJump = true;
    }

    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRange);

            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, attackRange);
        }
    }
}
