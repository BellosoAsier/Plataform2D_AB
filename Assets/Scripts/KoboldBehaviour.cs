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

    private Rigidbody2D rb;
    private int direction;
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerObject = GameObject.Find("DetectionPoint_Player").transform;
        index_listPointsOfPath = 0;
        pointDestination = listPointsOfPath[index_listPointsOfPath].position;
    }

    // Update is called once per frame
    void Update()
    {
        if (!GetComponent<RaycastDetection>().hasDetectedPlayer)
        {
            LookAtDestinationPoint(pointDestination);
            MovementToWaypoints();
        }
        else
        {
            LookAtDestinationPoint(playerObject.position);
            MovementTowardsPlayer();
        }

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
}
