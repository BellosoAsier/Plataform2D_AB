using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ColorKey { Sapphire, Gold, Ruby, Emerald}
public class DoorBehaviour : MonoBehaviour
{
    [SerializeField] private ColorKey key;
    [SerializeField] private int necessaryKeysAmount;

    private GameObject player;
    private bool canOpen;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("HeroKnight");
    }

    // Update is called once per frame
    void Update()
    {
        switch (key)
        {
            case ColorKey.Sapphire:
                if (necessaryKeysAmount == player.GetComponent<InventoryScript>().SapphireKey_Count)
                {
                    canOpen = true;
                    if (transform.childCount != 0)
                    {
                        Destroy(transform.GetChild(0).gameObject);
                    }
                    Destroy(GetComponent<BoxCollider2D>());
                }
                break;
            case ColorKey.Gold:
                if (necessaryKeysAmount == player.GetComponent<InventoryScript>().GoldKey_Count)
                {
                    canOpen = true;
                    if (transform.childCount != 0)
                    {
                        Destroy(transform.GetChild(0).gameObject);
                    }
                    Destroy(GetComponent<BoxCollider2D>());
                }
                break;
            case ColorKey.Ruby:
                if (necessaryKeysAmount == player.GetComponent<InventoryScript>().RubyKey_Count)
                {
                    canOpen = true;
                    if (transform.childCount != 0)
                    {
                        Destroy(transform.GetChild(0).gameObject);
                    }
                    Destroy(GetComponent<BoxCollider2D>());
                }
                break;
            case ColorKey.Emerald:
                if (necessaryKeysAmount == player.GetComponent<InventoryScript>().EmeraldKey_Count)
                {
                    canOpen = true;
                    if (transform.childCount != 0)
                    {
                        Destroy(transform.GetChild(0).gameObject);
                    }
                    Destroy(GetComponent<BoxCollider2D>());
                }
                break;
        }
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canOpen)
        {
            GetComponent<Animator>().SetBool("isOpen",true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && canOpen)
        {
            GetComponent<Animator>().SetBool("isOpen", false);
        }
    }
}
