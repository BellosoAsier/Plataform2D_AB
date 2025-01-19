using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailRaycastDetection : MonoBehaviour
{
    [Header("CHARACTERISTICS")]
    [SerializeField] private Transform target;      
    [SerializeField] private float detectionRange;  
    [SerializeField] private LayerMask ignoreLayer; 
    [SerializeField] private string tagName;      
    
    [SerializeField] private float hideCountdown = 3f;
    [SerializeField] private float currentHideCountdown = 0f;
    [SerializeField] private float showCountdown = 5f;
    [SerializeField] private float currentShowCountdown = 0f;

    [Header("FLAGS")]
    public bool hasDetectedPlayer = false;
    public bool hideFlag = false;
    public bool showFlag = false;

    

    private void Start()
    {
        target = GameObject.Find("DetectionPoint_Player").transform;
    }
    void Update()
    {
        if (target != null)
        {
            SnailDetections();

            if (hasDetectedPlayer)
            {
                currentHideCountdown += Time.deltaTime;
                if (currentHideCountdown >= hideCountdown)
                {
                    currentHideCountdown = hideCountdown;
                    hideFlag = true;
                }
                currentShowCountdown = 0f;
            }
            else
            {
                if (hideFlag && !showFlag)
                {
                    currentShowCountdown += Time.deltaTime;
                    if (currentShowCountdown >= showCountdown)
                    {
                        currentShowCountdown = showCountdown;
                        showFlag = true;
                    }
                }
                currentHideCountdown = 0f;
            }
        }
    }

    private void SnailDetections()
    {
        // Detectamos todo en un ángulo simétrico (tipo luz) desde el eje rojo (eje X del objeto)
        int rayCount = 10; // Número de rayos en el abanico
        float angleRange = 60f; // Ángulo total del abanico (45 grados hacia arriba y 45 grados hacia abajo desde el eje X)
        float startAngle = -angleRange / 2f; // Ángulo inicial (45 grados hacia abajo desde el eje X)

        for (int i = 0; i < rayCount; i++)
        {
            // Calculamos la dirección del rayo en el espacio local del objeto
            float angle = startAngle + i * (angleRange / (rayCount - 1));
            Vector2 localDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            // Convertimos la dirección local a global según la rotación del objeto
            Vector2 globalDirection = transform.TransformDirection(localDirection);

            // Lanzamos el raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, globalDirection, detectionRange, ~ignoreLayer);
            Debug.DrawRay(transform.position, globalDirection * detectionRange, Color.red);

            if (hit.collider != null && hit.collider.CompareTag(tagName))
            {
                hasDetectedPlayer = true;
                return;
            }
        }
        hasDetectedPlayer = false;

    }


    void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireSphere(transform.position, detectionRange);
        }
    }
}
