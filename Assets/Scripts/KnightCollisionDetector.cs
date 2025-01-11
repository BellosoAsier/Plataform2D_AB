using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnightCollisionDetector : MonoBehaviour
{
    private bool hasCollided = false;
    private float disableTimer;
    // Start is called before the first frame update
    void OnEnabled()
    {
        hasCollided = false;
    }

    public bool CurrentState()
    {
        if (disableTimer > 0)
        {
            return false;
        }

        return hasCollided;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ladder") )
        {
            hasCollided = true;
        }
        else if (!collision.CompareTag("Snail"))
        {
            hasCollided = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.CompareTag("Ladder"))
        {
            hasCollided = false;
        }
        else if (!collision.CompareTag("Snail"))
        {
            hasCollided = false;
        }
    }

    public void DisableCollisionDetector(float duration)
    {
        disableTimer = duration;
    }
    // Update is called once per frame
    void Update()
    {
        disableTimer -= Time.deltaTime;
    }
}
