using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailBehaviour : MonoBehaviour
{
    private Vector2[] listPointsOfPath;
    [SerializeField] private float velocityMovement;
    //private Transform playerObject;
    private Vector2 pointDestination;
    private int index_listPointsOfPath;
    // Start is called before the first frame update
    void Start()
    {
        index_listPointsOfPath = 0;
    }

    // Update is called once per frame
    void Update()
    {
        float distanceBetweenPoint = Mathf.Abs(((Vector2)transform.position - pointDestination).magnitude);
        if (distanceBetweenPoint > 0.1f)
        {
            LookAtDestinationPoint(pointDestination);
            transform.position = Vector3.MoveTowards(transform.position, pointDestination, velocityMovement * Time.deltaTime);
            //Move
        }
        else
        {
            ChangeDestinationPoint();
        }
    }

    private void LookAtDestinationPoint(Vector2 other)
    {
        Vector2 direction = pointDestination - (Vector2)transform.position;
        if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
        {
            if (direction.x > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 180f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 0f);
            }
        }
        else
        {
            if (direction.y > 0)
            {
                transform.rotation = Quaternion.Euler(0f, 180f, 90f);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0f, 180f, -90f);
            }
        }

    }

    public void SetListPointOfPath(Vector2[] list)
    {
        listPointsOfPath = list;
        pointDestination = listPointsOfPath[0];
    }

    private void ChangeDestinationPoint()
    {
        index_listPointsOfPath++;

        if (index_listPointsOfPath >= listPointsOfPath.Length)
        {
            index_listPointsOfPath = 0;
        }

        pointDestination = listPointsOfPath[index_listPointsOfPath];
    }
}
