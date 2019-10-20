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
            dustPickupEvent();
            dead = true;
            this.GetComponentInChildren<Animator>().SetTrigger("Death");
            Invoke("Death", 2f);

        }
    }



    private void dustPickupEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.DustPickup.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.DustPickup.Id);
    }

    void Death()
    {
        Destroy(this.gameObject);
    }
}
