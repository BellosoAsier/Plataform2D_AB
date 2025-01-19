using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BlackSmithBehaviour : MonoBehaviour
{
    [SerializeField] private GameObject canvas;
    [SerializeField] private GameObject canvas2;
    [SerializeField] private int healthIncrease;
    [SerializeField] private int moneyPrice;

    private bool canBuy;
    private GameObject player;

    private void Start()
    {
        player = GameObject.Find("HeroKnight");
    }
    private void Update()
    {
        if (canBuy)
        {
            if (Input.GetKeyDown(KeyCode.B) && player.GetComponent<InventoryScript>().Money >= moneyPrice)
            {
                player.GetComponent<HealthSystem>().SetHealthIncrease(healthIncrease);
                player.GetComponent<InventoryScript>().Money -= moneyPrice;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(true);
            canvas2.SetActive(true);
            canBuy = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            canvas.SetActive(false);
            canvas2.SetActive(false);
        }
    }
}
