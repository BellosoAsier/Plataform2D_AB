using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoboldRaycastDetection : MonoBehaviour
{
    [Header("CHARACTERISTICS")]
    [SerializeField] private Transform target;  
    [SerializeField] private float detectionRange;  
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
        if (target != null)
        {
            KoboldDetections();
        }
        JumpDetection();
    }


    private void KoboldDetections()
    {
        // Calculamos la direcci�n desde este objeto hacia el objetivo
        Vector3 directionToTarget = target.position - transform.position;

        // Verificamos si el objetivo est� dentro del rango
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
                    return;
                }
            }
        }
        isInsideAttackRange = false;
        hasDetectedPlayer = false;
    }

    private void JumpDetection()
    {
        // Direcci�n del eje rojo (eje X positivo)
        Vector2 direction = pointForJump.right;

        // Dibujar el raycast para visualizarlo
        Debug.DrawLine(pointForJump.position, pointForJump.position + (Vector3)direction, Color.cyan);

        // Lanzar el raycast en el eje X positivo con un rango de 1 unidad
        RaycastHit2D hit = Physics2D.Raycast(pointForJump.position, direction, jumpRange, worldLayer);

        if (hit.collider != null)
        {
            // Si colisiona con algo, ejecuta una funci�n
            if (canJump)
            {
                GetComponent<Rigidbody2D>().AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                canJump = false;
                StartCoroutine(WaitSecondorJump());
            }
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
