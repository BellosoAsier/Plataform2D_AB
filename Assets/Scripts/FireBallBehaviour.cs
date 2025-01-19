using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallBehaviour : MonoBehaviour
{
    private Rigidbody2D rb;
    [SerializeField] private Vector3 demon;
    [SerializeField] private float velocity;
    [SerializeField] private float damage; 
    private Vector3 position;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        position = (GameObject.Find("HeroKnight").transform.position - transform.position).normalized;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position += position * velocity * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && gameObject.CompareTag("FireBall"))
        {
            if (collision.GetComponent<PlayerHeroKnight>().canReflect)
            {
                Vector2 direction = (demon - transform.position).normalized;
                float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
                Quaternion rotation = Quaternion.Euler(0, 0, angle);
                transform.rotation = rotation;
                gameObject.tag = "ReflectedFireBall";
                position = (demon - transform.position).normalized;
            }
            else
            {
                if (collision.GetComponent<PlayerHeroKnight>().isBlocking)
                {
                    Destroy(gameObject);
                }
                else
                {
                    collision.GetComponent<HealthSystem>().DamageCharacter(damage);
                    Destroy(gameObject);
                }
            }
        }
        else if (collision.gameObject.CompareTag("Demon") && gameObject.CompareTag("FireBall"))
        {
            //Nothing
        }
        else if (collision.gameObject.CompareTag("Demon") && gameObject.CompareTag("ReflectedFireBall"))
        {
            collision.GetComponent<HealthSystem>().DamageCharacter(damage);
            Destroy(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void SetDemon(Vector3 demonPos)
    {
        demon = demonPos;
    }
}
