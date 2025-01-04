using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHeroKnight : MonoBehaviour
{
    // Start is called before the first frame update
    private Rigidbody2D rb;
    private float inputH;
    [SerializeField] private float velocityForce;
    [SerializeField] private float jumpForce;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        inputH = Input.GetAxisRaw("Horizontal");
        rb.velocity = new Vector2(inputH * velocityForce, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
        }
    }
}
