using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
   
    public bool playable = true;
    public float groove = 0.3f;
    
    [Header("Heighness")]
    private WorldMovement worldMove;
    public float progression = -22;
    public Transform smoke;

    public PlayerController player;

    private int currentIndex = 0;
    private Vector3 inverseMovement;

    // Start is called before the first frame update
    void Start()
    {
        worldMove = GameManager.instance.worldMove;
        inverseMovement = worldMove.mainMovementSpeed * -1f;
    }

    // Update is called once per frame
    void Update()
    {

        if (!playable)
            return;

        //Follow the ground : 
        float y = 0;
        if (progression > 0)//no more progression when > -2
        {
            WorldMovement.BeatData datA = GameManager.instance.worldMove.beatDatas[currentIndex];
            WorldMovement.BeatData datB = GameManager.instance.worldMove.beatDatas[currentIndex + 1];

            WorldMovement.BeatData datA_ = GameManager.instance.worldMove.beatDatas[currentIndex + 1];
            WorldMovement.BeatData datB_ = GameManager.instance.worldMove.beatDatas[currentIndex + 2];
            Transform dotA = datA_.dot.transform;
            Transform dotB = datB_.dot.transform;

            float distanceFaite = (progression - (datA.timeSum * inverseMovement.x));
            float distanceAFaire = datB.beatTiming * inverseMovement.x;

            float lerpValue = distanceFaite / distanceAFaire;
            

            Vector3 posReal = Vector3.Lerp(datA.dot.transform.position, datB.dot.transform.position, lerpValue);

            if (lerpValue > 1)
            {
                currentIndex++;
                //HERE : change the target dot
                float h = Mathf.Sqrt(Mathf.Pow(dotB.position.x - dotA.position.x, 2) + Mathf.Pow(dotB.position.y - dotB.position.y, 2));
                Vector3 v = dotB.position - dotA.position;
                Vector3 normV = new Vector3(-v.y, v.x, 0) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y, 2)) * h;
                smoke.up = normV;


            }

            y = posReal.y;

            groove++;
        }
               
               
        Vector3 movement = Vector3.up * ((this.transform.position.y * -1) + y);

        this.transform.Translate(movement);

        progression += Time.deltaTime * (inverseMovement.x);
    }

    public void OnTriggerEnter2D(Collider2D collision)
    { 
        Dust dust = collision.gameObject.GetComponentInParent<Dust>();
        Crack crack = collision.gameObject.GetComponentInParent<Crack>();

        if (dust != null)
        {

            if (dust.dead)
                return;
            dustMissedEvent();
            dust.dead = true;
            dust.GetComponentInChildren<Animator>().SetTrigger("Death");
            Invoke("Death", 2f);
            groove -= 100f;

            if(groove < 0)
            {
                WorldMovement.gameover = true;
                Destroy(player);
                //gameoverEvent();
            }
        }

        if (crack != null)
        {

            if (!crack.alive)
                return; 
            scratchMissedEvent();
            crack.objectiveDeath();
            groove -= 0.2f;

            if(groove < 0)
            {
                WorldMovement.gameover = true;
                Destroy(player);
                //gameoverEvent();
            }
        }
    }

    private void dustMissedEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.DustMissed.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.DustMissed.Id);
    }

    private void scratchMissedEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.ScratchMissed.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.ScratchMissed.Id);
    }

    private void gameoverEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.GameOver.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.GameOver.Id);
    }

}
