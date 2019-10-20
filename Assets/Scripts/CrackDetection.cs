using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrackDetection : MonoBehaviour
{
    



    private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController plyCtr = other.GetComponentInParent<PlayerController>();
        if (plyCtr != null)
        {
            Debug.Log("Player");
            if (plyCtr.hitIn)
            {
                Debug.Log("And hit ! !");
                GetComponentInParent<Crack>().HitThis();
            }
        }
    }
}
