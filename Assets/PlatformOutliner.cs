using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlatformOutliner : MonoBehaviour
{

    [SerializeField] private CompositeCollider2D compositeCollider; // CompositeCollider2D del Tilemap
    private Vector2 tilePosition;                 // Posición del Tile (en coordenadas mundiales) de la plataforma específica
    //[SerializeField] private float speed = 2f;                     // Velocidad del caracol
    [SerializeField] private float offset;                  // Desplazamiento hacia fuera

    [SerializeField] public Vector2[] pathPoints;                // Puntos del contorno de la plataforma específica
    private int currentTargetIndex = 0;          // Índice del punto objetivo actual

    void Awake()
    {
        tilePosition = transform.position;
        if (compositeCollider == null)
        {
            Debug.LogError("Debes asignar un CompositeCollider2D al script.");
            return;
        }

        // Obtener los puntos de la plataforma específica con desplazamiento
        GetPathPointsForSpecificTile();
    }

    void Update()
    {
        //if (pathPoints == null || pathPoints.Length == 0) return;

        //// Mover al caracol hacia el punto objetivo actual
        //Vector2 targetPoint = pathPoints[currentTargetIndex];
        //Vector2 currentPosition = transform.position;

        //// Movimiento lineal hacia el punto objetivo
        //transform.position = Vector2.MoveTowards(currentPosition, targetPoint, speed * Time.deltaTime);

        //// Verificar si llegó al punto objetivo
        //if (Vector2.Distance(currentPosition, targetPoint) < 0.1f)
        //{
        //    // Pasar al siguiente punto
        //    currentTargetIndex = (currentTargetIndex + 1) % pathPoints.Length;
        //}
    }

    void GetPathPointsForSpecificTile()
    {
        // Obtener el número total de rutas en el colisionador
        int pathCount = compositeCollider.pathCount;
        if (pathCount == 0) return;

        // Buscar la ruta que contiene el Tile deseado
        for (int i = 0; i < pathCount; i++)
        {
            Vector2[] path = new Vector2[compositeCollider.GetPathPointCount(i)];
            compositeCollider.GetPath(i, path);

            // Verificar si la posición del Tile está dentro de esta ruta
            if (IsPointInPolygon(tilePosition, path))
            {
                // Aplicar el desplazamiento hacia fuera
                pathPoints = OffsetPath(path, offset);
                break;
            }
        }

        if (pathPoints == null || pathPoints.Length == 0)
        {
            Debug.LogWarning("No se encontró una ruta que contenga la posición del Tile especificado.");
        }
    }

    // Función para verificar si un punto está dentro de un polígono
    private bool IsPointInPolygon(Vector2 point, Vector2[] polygon)
    {
        int crossings = 0;
        for (int i = 0; i < polygon.Length; i++)
        {
            Vector2 v1 = polygon[i];
            Vector2 v2 = polygon[(i + 1) % polygon.Length];

            // Verificar si el punto está entre las coordenadas Y de los dos vértices
            if (((v1.y > point.y) != (v2.y > point.y)) &&
                (point.x < (v2.x - v1.x) * (point.y - v1.y) / (v2.y - v1.y) + v1.x))
            {
                crossings++;
            }
        }

        // Si el número de cruces es impar, el punto está dentro
        return crossings % 2 != 0;
    }

    // Función para desplazar el contorno hacia afuera
    private Vector2[] OffsetPath(Vector2[] path, float offset)
    {
        Vector2[] offsetPath = new Vector2[path.Length];

        for (int i = 0; i < path.Length; i++)
        {
            // Puntos actuales y adyacentes
            Vector2 current = path[i];
            Vector2 next = path[(i + 1) % path.Length];
            Vector2 prev = path[(i - 1 + path.Length) % path.Length];

            // Calcular vectores normales de los segmentos anterior y siguiente
            Vector2 normalPrev = GetNormal(prev, current);
            Vector2 normalNext = GetNormal(current, next);

            // Promediar las normales y normalizar
            Vector2 offsetDirection = (normalPrev + normalNext).normalized;

            // Desplazar el punto actual en la dirección calculada
            offsetPath[i] = (Vector2) compositeCollider.transform.TransformPoint(current) + offsetDirection * offset;
        }
        return offsetPath;
    }

    // Calcular la normal perpendicular a un segmento
    private Vector2 GetNormal(Vector2 from, Vector2 to)
    {
        Vector2 direction = (to - from).normalized;
        return new Vector2(-direction.y, direction.x);
    }

    void OnDrawGizmos()
    {
        // Dibujar los puntos y líneas para depuración
        if (pathPoints != null && pathPoints.Length > 0)
        {
            Gizmos.color = Color.red;
            for (int i = 0; i < pathPoints.Length; i++)
            {
                Gizmos.DrawSphere(pathPoints[i], 0.1f);
                Vector2 nextPoint = pathPoints[(i + 1) % pathPoints.Length];
                Gizmos.DrawLine(pathPoints[i], nextPoint);
            }
        }

        // Dibujar la posición del Tile para referencia
        Gizmos.color = Color.blue;
        Gizmos.DrawSphere(transform.position, 0.2f);
    }
}

