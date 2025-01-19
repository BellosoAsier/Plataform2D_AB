using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public enum EnemyType { Kobold, Demon, Snail, Knight }
public class HealthSystem : MonoBehaviour
{
    [SerializeField] public EnemyType enemyType;
 
    private float maxHealthValue;
    [SerializeField] private float currentHealthValue;
    private Animator animator;
    [SerializeField] private GameObject coinPrefab;

    [SerializeField] private Image bar;

    [SerializeField] private AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        maxHealthValue = currentHealthValue;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        bar.fillAmount = currentHealthValue / maxHealthValue;
    }

    public void DamageCharacter(float damageDealt)
    {
        switch (enemyType)
        {
            case EnemyType.Kobold:
                currentHealthValue -= damageDealt;
                audioSource.Play();
                if (currentHealthValue <= 0f)
                {
                    animator.SetTrigger("Death");
                    GetComponent<KoboldBehaviour>().enabled = false;
                }
                break;

            case EnemyType.Knight:
                currentHealthValue -= damageDealt;
                animator.SetTrigger("Hurt");
                audioSource.Play();
                if (currentHealthValue <= 0f)
                {
                    animator.SetTrigger("Death");
                    SceneManager.LoadSceneAsync("00_MenuScene");
                }
                break;

            case EnemyType.Demon:
                currentHealthValue -= damageDealt;
                audioSource.Play();
                if (currentHealthValue <= 0f)
                {
                    animator.SetBool("isDead", true);
                }
                animator.SetTrigger("Hurt");
                break;

            case EnemyType.Snail:
                currentHealthValue -= damageDealt;
                audioSource.Play();
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
        Debug.Log(coin);
        Destroy(gameObject);
    }

    public void SetHealthIncrease(float value)
    {
        currentHealthValue += value;

        if (currentHealthValue > maxHealthValue)
        {
            maxHealthValue = currentHealthValue;
        }
    }
}
