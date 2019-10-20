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
    public int currentIndex = 0;

    [Header("Jump part")]
    public string jumpAxis = "Jump";
    public Transform jumpingPart;
    public bool jumpingIn = false;

    public AnimationCurve jumpCurve;
    public Vector2 speedOnBothAxis = new Vector2(1, 1);

    public bool hitIn = false;
    private Vector3 originUp;




    // Start is called before the first frame update
    void Start()
    {
        originUp = transform.up;
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
        hitIn = _animator.GetCurrentAnimatorClipInfo(0)[0].clip.name.Contains("hit");

        if (Input.GetKeyDown(KeyCode.Return))
        {
            if (jumpingIn)
            {
                if(!hitIn)
                    _animator.SetTrigger("JumpHit");
                //swipeActionAirEvent();
            }
            else
                if (!hitIn)
                _animator.SetTrigger("Hit");
                //swipeActionGroundEvent();
        }


        //Follow the ground : 
        float y = 0;
        if (progression > 0)//no more progression when > -2
        {
            y = ProgressionUpdate();
        }

        movementForThisFrame.y = (this.transform.position.y * -1) + y;
        
        this.transform.Translate(movementForThisFrame);
        
        progression += Time.deltaTime * (inverseMovement.x);
    }

    public float debugVaLl = 1f;
    public float seeOtherValue = 0;
    public AnimationCurve followTheCurveharry;

    float ProgressionUpdate()
    {
        
        
        /*public float timeSum = 0;
        public float beatTiming = 0;
        */
        ///
        WorldMovement.BeatData datA = GameManager.instance.worldMove.beatDatas[currentIndex];
        WorldMovement.BeatData datB = GameManager.instance.worldMove.beatDatas[currentIndex + 1];
        WorldMovement.BeatData datA_ = GameManager.instance.worldMove.beatDatas[currentIndex + 1];
        WorldMovement.BeatData datB_ = GameManager.instance.worldMove.beatDatas[currentIndex + 2];
        Transform dotA = datA_.dot.transform;
        Transform dotB = datB_.dot.transform;

        float distanceFaite = (progression - (datA.timeSum * inverseMovement.x));

        float distanceAFaire = datB.beatTiming * inverseMovement.x;

        float lerpValue = distanceFaite / distanceAFaire;

        seeOtherValue = lerpValue;
        followTheCurveharry.AddKey(Time.timeSinceLevelLoad - 5, lerpValue);

        Vector3 posReal = Vector3.Lerp(datA.dot.transform.position, datB.dot.transform.position, lerpValue);

        if(lerpValue > 1)
        {
           // Debug.Log("distanceAFaire = "+ distanceAFaire + " datA.timeSum : "+ datA.timeSum + " datB.beat = "+ datB.beatTiming + " ");
            currentIndex++;
            //HERE : change the target dot
            //I think it's here where we need to change the rotation
            float h = Mathf.Sqrt(Mathf.Pow(dotB.position.x - dotA.position.x, 2) + Mathf.Pow(dotB.position.y - dotB.position.y, 2));
            Vector3 v = dotB.position - dotA.position;
            Vector3 normV = new Vector3(-v.y, v.x, 0) / Mathf.Sqrt(Mathf.Pow(v.x, 2) + Mathf.Pow(v.y,2)) * h;
            if (jumpingIn == false)
            {
                jumpingPart.up = normV;
            }

        }

        return posReal.y;
        ///
    }

    void Jump()
    {
        //Debug.Log("Jump");
        StartCoroutine(jumping());
    }

    IEnumerator jumping()
    {
        //launch Jump
        jumpingPart.up = originUp;
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

        //end jump
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
    private void swipeActionGroundEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.SwipeActionGround.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.SwipeActionGround.Id);
    }

    private void swipeActionAirEvent()
    {
        AkSoundEngine.PostEvent(Sound_Manager.instance.SwipeActionAir.Id, this.gameObject);
        Debug.Log("Call the event " + Sound_Manager.instance.SwipeActionAir.Id);
    }

}
