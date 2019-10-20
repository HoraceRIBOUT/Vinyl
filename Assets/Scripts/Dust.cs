using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dust : MonoBehaviour
{
    public bool dead = false;

    public void OnTriggerEnter2D(Collider2D collision)
    {

        PlayerController pC = collision.gameObject.GetComponentInParent<PlayerController>();
        if (pC != null)
        {
            dead = true;
            this.GetComponentInChildren<Animator>().SetTrigger("Death");
            Invoke("Death", 2f);

        }
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
