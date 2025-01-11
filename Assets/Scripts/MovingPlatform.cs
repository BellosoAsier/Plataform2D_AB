using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlatformStartPoint { StartPoint, EndPoint}
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private GameObject movingPlatform;

    [SerializeField] private PlatformStartPoint platformStartPoint;

    [SerializeField] private Transform startPoint;
    [SerializeField] private Transform endPoint;

    [SerializeField] private float velocity;

    [SerializeField] private Vector3 moveTowardsPoint;

    private bool canMove;
    // Start is called before the first frame update
    void Start()
    {
        switch (platformStartPoint)
        {
            case PlatformStartPoint.StartPoint:
                movingPlatform.transform.position = startPoint.position;
                moveTowardsPoint = endPoint.position;
                break;
            case PlatformStartPoint.EndPoint:
                movingPlatform.transform.position = endPoint.position;
                moveTowardsPoint = startPoint.position;
                break;
        }
        
        canMove = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (canMove)
        {
            movingPlatform.transform.position = Vector3.MoveTowards(movingPlatform.transform.position, moveTowardsPoint, velocity * Time.deltaTime);

            float distanceToPoint = Mathf.Abs(movingPlatform.transform.position.x - moveTowardsPoint.x);

            if (distanceToPoint <= 0.1f)
            {
                if (moveTowardsPoint == startPoint.position)
                {
                    StartCoroutine(ChangeMoveTowardsPoint(endPoint));
                }
                else
                {
                    StartCoroutine(ChangeMoveTowardsPoint(startPoint));
                }

            }
        }
    }

    IEnumerator ChangeMoveTowardsPoint(Transform point)
    {
        canMove = false;
        yield return new WaitForSeconds(2f);
        moveTowardsPoint = point.position;
        canMove = true;
    }
}
