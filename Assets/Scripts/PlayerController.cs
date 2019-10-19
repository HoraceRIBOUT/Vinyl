using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public bool playable = true;

    [HideInInspector]
    public Vector3 inverseMovement;

    [Header("Component Part")]
    public Animator _animator;

    [Header("Heighness")]
    private WorldMovement worldMove;
    public float progression = -11;

    [Header("Jump part")]
    public string jumpAxis = "Jump";
    public Transform jumpingPart;
    /*public Vector3 jumpForce;//?
    */

    public AnimationCurve jumpCurve;
    public Vector2 speedOnBothAxis = new Vector2(1, 1);
    public bool jumpingIn = false;


    




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

        //Jump
        if (Input.GetAxis(jumpAxis) != 0 && !jumpingIn)
            Jump();

        //Hit
        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (jumpingIn)
            {
                if(!_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("hit"))
                    _animator.SetTrigger("JumpHit");
            }
            else
                if (!_animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("hit"))
                _animator.SetTrigger("Hit");
        }
        

        //Follow the ground : 
        float y = 0;
        if (progression > -2)
        {
            y = ProgressionUpdate();
        }

        movementForThisFrame.y = (this.transform.position.y * -1) + y;
        
        this.transform.Translate(movementForThisFrame);
        
        progression += Time.deltaTime * (inverseMovement.x);
    }

    public float debugVaLl = 1f;
    public float seeOtherValue = 0;

    float ProgressionUpdate()
    {
        float distanceAssumee = inverseMovement.x * GameManager.instance.beatTiming;//replace by the beat of that point and reset the progression
        float realPos = progression * (1f / (distanceAssumee));
        int dotIndex = (int)realPos;

        //Debug.Log("DotIndex : " + dotIndex);

        float lerpValue = realPos - (float)dotIndex;

        seeOtherValue = lerpValue;

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
        return posReal.y;

        /*
        float realPos = progression * (1f/ (distanceAssumee));//GameManager.instance.beatTiming * 0.5f; // distance made between each. If -1 speed set the 2f , then...

        if ((int)realPos != 1){
            currentdotIndex++;
            progression = 0.001f;
        }

        //Debug.Log("DotIndex : " + dotIndex);

        float lerpValue = realPos;

        seeOtherValue = lerpValue;

        Vector3 posA = Vector3.zero;
        if (progression > 0)
            posA = worldMove.eachDotFromMusique[currentdotIndex].transform.position;
        else
        {
            lerpValue = 1 + lerpValue;
            currentdotIndex = -1;
        }
        Vector3 posB = worldMove.eachDotFromMusique[currentdotIndex + 1].transform.position;

        Vector3 posReal = Vector3.Lerp(posA, posB, lerpValue);
        return posReal.y;*/
    }

    void Jump()
    {
        Debug.Log("Jump");
        StartCoroutine(jumping());
    }

    IEnumerator jumping()
    {
        _animator.SetBool("Jump", true);
        jumpingIn = true;
        float timer = 0;
        while (jumpingPart.localPosition.y >= 0)
        {
            float y = 0;

            timer += Time.deltaTime;
            y = jumpCurve.Evaluate(timer * speedOnBothAxis.x) * speedOnBothAxis.y;

            jumpingPart.localPosition = new Vector3(jumpingPart.localPosition.x, y, jumpingPart.localPosition.z); //brrr,non pas ça, beurk
            yield return new WaitForSeconds(1f / 30f);
        }
        jumpingPart.localPosition = new Vector3(jumpingPart.localPosition.x, 0, jumpingPart.localPosition.z);
        jumpingIn = false;
        _animator.SetBool("Jump", false);
    }

    /*
    IEnumerator jumping()
    {
        //Launch the jump : 
        float progressionJump = 0;

        float alpha = 45;
        //A tweaker : 
        float acc = 1;
        float vit = 1;

        while (jumpingPart.position.y > 0)
        {
            float y = 0;


            y = .5f * (acc) * (1f / Mathf.Pow(Mathf.Cos(vit * alpha), 2)) * Mathf.Pow(progressionJump, 2) + progressionJump * Mathf.Tan(alpha);

            jumpingPart.position = new Vector3(jumpingPart.position.x, y, jumpingPart.position.z);
            yield return new WaitForSeconds(1 / 30f);
            progressionJump += Time.deltaTime;
        }
        
        jumpingPart.position = new Vector3(jumpingPart.position.x, 0, jumpingPart.position.z);
    }
    */


}
