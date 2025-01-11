using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum EnemyType { Kobold, Demon, Snail, Bee, Knight }
public class HealthSystem : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType;
 
    private float maxHealthValue;
    [SerializeField] private float currentHealthValue;
    private Animator animator;
    [SerializeField] private GameObject coinPrefab;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void DamageCharacter(float damageDealt)
    {
        switch (enemyType)
        {
            case EnemyType.Kobold:
                currentHealthValue -= damageDealt;

                if (currentHealthValue <= 0f)
                {
                    animator.SetTrigger("Death");
                    GetComponent<KoboldBehaviour>().enabled = false;
                }
                break;

            case EnemyType.Bee:
                currentHealthValue -= damageDealt;
                // Faltan cosas
                break;

            case EnemyType.Knight:
                currentHealthValue -= damageDealt;
                animator.SetTrigger("Hurt");

                if (currentHealthValue <= 0f)
                {
                    animator.SetTrigger("Death");
                    Application.Quit();
                }
                break;

            case EnemyType.Demon:
                currentHealthValue -= damageDealt;
                // Faltan cosas
                break;

            case EnemyType.Snail:
                currentHealthValue -= damageDealt;

                if (currentHealthValue <= 0f)
                {
                    animator.SetTrigger("Death");
                    GetComponent<SnailBehaviour>().enabled = false;
                }
                break;
        }
    }

    //Se llama desde evento de animacion
    public void DestroyCharacter()
    {
        GameObject coin = Instantiate(coinPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }
}
