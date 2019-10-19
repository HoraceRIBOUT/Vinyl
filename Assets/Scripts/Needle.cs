using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Needle : MonoBehaviour
{
   
    public bool playable = true;
    
    [Header("Heighness")]
    private WorldMovement worldMove;
    public float progression = -22;
    
    // Start is called before the first frame update
    void Start()
    {
        worldMove = GameManager.instance.worldMove;
    }

    // Update is called once per frame
    void Update()
    {
        if (!playable)
            return;

        //Follow the ground : 
        float y = 0;
        if (progression > -2)
        {
            float realPos = progression / 2f;
            int dotIndex = (int)realPos;
            float lerpValue = realPos - (float)dotIndex;

            Vector3 posA = Vector3.zero;
            if (progression > 0)
                posA = worldMove.eachDotFromMusique[dotIndex].transform.position;
            else
            {
                lerpValue = 1 + lerpValue;
                dotIndex = -1;
            }
            Vector3 posB = worldMove.eachDotFromMusique[dotIndex + 1].transform.position;

            Vector3 posReal = Vector3.Lerp(posA, posB, lerpValue);
            y = posReal.y;
        }

        Vector3 movement = Vector3.up * ((this.transform.position.y * -1) + y);

        this.transform.Translate(movement);

        progression += Time.deltaTime;
    }

}
