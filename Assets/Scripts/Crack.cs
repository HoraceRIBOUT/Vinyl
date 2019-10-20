using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crack : MonoBehaviour
{
    public bool alive = true;
    public Animator crackAnim;
    public SpriteRenderer sR_Circle;

    public Color colorToGoDown = Color.black;

    public void Start()
    {
        crackAnim.speed = 0;
    }

    /*private void OnTriggerStay2D(Collider2D other)
    {
        PlayerController plyCtr = other.GetComponent<PlayerController>();
        if(plyCtr != null)
        {
            Debug.Log("Touch !");
            if (Input.GetKeyDown(KeyCode.Return))
            {
                Debug.Log("And hit ! !");
                HitThis();
            }
        }
    }*/


    public void HitThis()
    {
        if (!alive)
            return;

        Debug.Log("And hit ! !");
        alive = false;
        crackAnim.speed = 1;
        StartCoroutine(disapearRoseRound(1f, true));
        scratchRepairedEvent();
    }

    IEnumerator disapearRoseRound(float speed, bool player)
    {
        while(sR_Circle.color.a > 0)
        {
            sR_Circle.color -= colorToGoDown * Time.deltaTime * speed;
            yield return new WaitForSeconds(0.01f);
        }

        if(player == true)
            Destroy(this.gameObject);
    }

    public void objectiveDeath()
    {
    
        StartCoroutine(disapearRoseRound(12f, false));
    }

    private void scratchRepairedEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.ScratchRepaired.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.ScratchRepaired.Id);
    }

}
