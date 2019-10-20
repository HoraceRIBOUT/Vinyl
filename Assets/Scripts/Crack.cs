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
        StartCoroutine(disapearRoseRound());
    }

    IEnumerator disapearRoseRound()
    {
        while(sR_Circle.color.a > 0)
        {
            sR_Circle.color -= colorToGoDown * Time.deltaTime;
            yield return new WaitForSeconds(0.01f);
        }


        Destroy(this.gameObject);
    }

}
