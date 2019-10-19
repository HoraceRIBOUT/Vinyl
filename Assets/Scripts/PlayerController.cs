using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playable = true;

    [HideInInspector]
    public Vector3 inverseMovement;

    [Header("Heighness")]
    private WorldMovement worldMove;
    public float progression = -11;

    [Header("Jump part")]
    public string jumpAxis = "Jump";
    public Transform jumpingPart;
    public Vector3 jumpForce;//?








    // Start is called before the first frame update
    void Start()
    {
        worldMove = GameManager.instance.worldMove;
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movementForThisFrame = inverseMovement * Time.deltaTime;
        if (!playable)
            return;


        if (Input.GetAxis(jumpAxis) != 0)
            Jump();

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

        movementForThisFrame.y = (this.transform.position.y * -1) + y;
        
        this.transform.Translate(movementForThisFrame);

        progression += Time.deltaTime;
    }
    
    void Jump()
    {
        float y = 0;

        // y = .5f * (a0 / (v0² *cos² alpha)) x² +x * Mathf.Tan(alpha);
        
    }

}
