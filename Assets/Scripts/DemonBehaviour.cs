using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DemonBehaviour : MonoBehaviour
{
    private Transform target;
    [SerializeField] private GameObject fireBallPrefab;
    [SerializeField] private Transform firePoint;
    [SerializeField] private float force;

    private float currentTime = 0f;
    private Animator animator;
    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.Find("HeroKnight").transform;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GetComponent<DemonRaycastDetection>().hasDetectedPlayer)
        {
            LookAtDestinationPoint(target.transform.position);
            currentTime += Time.deltaTime;

            if (currentTime > 2f)
            {
                animator.SetTrigger("Attack");
                currentTime = 0;
            }
        }
        else
        {
            currentTime = 2f;
        }
    }

    //Se llama desde evento de animacion
    public void DemonAttack()
    {
        Vector2 direction = (target.position - firePoint.position).normalized;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        Quaternion rotation = Quaternion.Euler(0, 0, angle);
        GameObject fireBall = Instantiate(fireBallPrefab, firePoint.position, rotation);
        fireBall.GetComponent<FireBallBehaviour>().SetDemon(transform.position);
    }

    private void LookAtDestinationPoint(Vector3 other)
    {
        if (other.x > transform.position.x)
        {
            transform.rotation = Quaternion.identity;
        }
        else
        {
            transform.rotation = Quaternion.Euler(0f, -180f, 0f);
        }
    }
}
