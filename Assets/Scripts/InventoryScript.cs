using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryScript : MonoBehaviour
{
    [SerializeField] private int sapphireKey_Count;
    [SerializeField] private int goldKey_Count;
    [SerializeField] private int rubyKey_Count;
    [SerializeField] private int emeraldKey_Count;

    [SerializeField] private int money;

    public int SapphireKey_Count { get => sapphireKey_Count; set => sapphireKey_Count = value; }
    public int GoldKey_Count { get => goldKey_Count; set => goldKey_Count = value; }
    public int RubyKey_Count { get => rubyKey_Count; set => rubyKey_Count = value; }
    public int EmeraldKey_Count { get => emeraldKey_Count; set => emeraldKey_Count = value; }
    public int Money { get => money; set => money = value; }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("SapphireKey"))
        {
            sapphireKey_Count += 1;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("GoldKey"))
        {
            goldKey_Count += 1;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("RubyKey"))
        {
            rubyKey_Count += 1;
            Destroy(collision.gameObject);
        }
        else if (collision.CompareTag("EmeraldKey"))
        {
            emeraldKey_Count += 1;
            Destroy(collision.gameObject);
        }
    }
}
