using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonRaycastDetection : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float detectionRange;
    [SerializeField] private LayerMask ignoreLayer;
    [SerializeField] private string tagName;
    [SerializeField] private Transform areaPoint;
    [SerializeField] private float angleRange;

    public bool hasDetectedPlayer = false;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        DemonDetection();
    }

    private void DemonDetection()
    {
        // Detectamos todo en un �ngulo sim�trico (tipo luz) desde el eje rojo (eje X del objeto)
        int rayCount = Mathf.RoundToInt(angleRange / 7); // N�mero de rayos en el abanico

        Vector2 directionToAreaPoint = (areaPoint.position - transform.position).normalized;
        // Calculamos el �ngulo base hacia el areaPoint
        float baseAngle = Mathf.Atan2(directionToAreaPoint.y, directionToAreaPoint.x) * Mathf.Rad2Deg;

        // �ngulo inicial del abanico
        float startAngle = baseAngle - angleRange / 2f;

        for (int i = 0; i < rayCount; i++)
        {
            // Calculamos la direcci�n del rayo en el espacio local del objeto
            float angle = startAngle + i * (angleRange / (rayCount - 1));
            Vector2 localDirection = new Vector2(Mathf.Cos(angle * Mathf.Deg2Rad), Mathf.Sin(angle * Mathf.Deg2Rad)).normalized;

            // Convertimos la direcci�n local a global seg�n la rotaci�n del objeto
            Vector2 globalDirection = transform.TransformDirection(localDirection);

            // Lanzamos el raycast
            RaycastHit2D hit = Physics2D.Raycast(transform.position, globalDirection, detectionRange, ~ignoreLayer);
            Debug.DrawRay(transform.position, globalDirection * detectionRange, Color.red);

            if (hit.collider != null && hit.collider.tag.Contains("Player"))
            {
                hasDetectedPlayer = true;
                return;
            }
        }
        hasDetectedPlayer = false;
    }

}
