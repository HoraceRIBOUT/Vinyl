using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
   
    public bool playable = true;
    
    [Header("Heighness")]
    private WorldMovement worldMove;
    public float progression = -22;


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



            float distanceFaite = (progression - (datA.timeSum * inverseMovement.x));
            float distanceAFaire = datB.beatTiming * inverseMovement.x;

            float lerpValue = distanceFaite / distanceAFaire;
            

            Vector3 posReal = Vector3.Lerp(datA.dot.transform.position, datB.dot.transform.position, lerpValue);

            if (lerpValue > 1)
            {
                currentIndex++;
                //HERE : change the target dot
                //I think it's here where we need to change the rotation
            }

            y = posReal.y;
        }
               
               
        Vector3 movement = Vector3.up * ((this.transform.position.y * -1) + y);

        this.transform.Translate(movement);

        progression += Time.deltaTime * (inverseMovement.x);
    }

}
