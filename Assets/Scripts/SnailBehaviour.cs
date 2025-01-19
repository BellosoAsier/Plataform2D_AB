using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SnailBehaviour : MonoBehaviour
{
    private Vector2[] listPointsOfPath;
    [SerializeField] private float velocityMovement;
    [SerializeField] private float damage;
    //private Transform playerObject;
    private Vector2 pointDestination;
    private int index_listPointsOfPath;
    private Animator animator;

    private bool canMove = true;
    private bool onlyOnce = true;
    // Start is called before the first frame update
    void Start()
    {
        index_listPointsOfPath = 0;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<SnailRaycastDetection>().hideFlag)
        {
            canMove = false;
            if (onlyOnce)
            {
                animator.SetTrigger("Hide");
                GetComponent<CircleCollider2D>().enabled = false;
                onlyOnce = false;
            }
        }

        if (GetComponent<SnailRaycastDetection>().showFlag)
        {
            GetComponent<SnailRaycastDetection>().hideFlag = false;
            animator.SetTrigger("Show");
            GetComponent<CircleCollider2D>().enabled = true;
        }

        if(canMove)
        {
            Movement();
        }
    }

    private void Movement()
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

    //Se llama desde evento de animacion
    public void ToggleMovement()
    {
        onlyOnce = true;
        GetComponent<SnailRaycastDetection>().showFlag = false;
        canMove = true;
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

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            other.gameObject.GetComponent<HealthSystem>().DamageCharacter(damage);
        }
    }
}
