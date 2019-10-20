using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager : MonoBehaviour
{
    public static Sound_Manager instance;

    public void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Debug.LogError("Already the Sound_Manager");
    }

    public AK.Wwise.Event testEvent;
    public AK.Wwise.Event startEvent;

    //BEAT
    float lastBeatTime = 0;


    [Header("A little surprise")]
    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent(startEvent.Id, this.gameObject);

        startEvent.Post(this.gameObject, (uint)AkCallbackType.AK_MusicSyncBeat 
                                       | (uint)AkCallbackType.AK_MusicSyncBar, CallBackFunction, this);
    }

    private void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
        switch (type)
        {
            case AkCallbackType.AK_MusicSyncBeat:
                //Debug.Log("Call by " + type + " time = " + Time.timeSinceLevelLoad);
                Beat();
                break;
            case AkCallbackType.AK_MusicSyncBar:
                //Debug.Log("Call by " + type + " time = " + Time.timeSinceLevelLoad);
                Bar();
                break;
        }
    }

    [ContextMenu("Beat")]
    public void Beat()
    {
        if(ani != null)
        {
            float ran = Random.Range(0f, 1f);
            foreach (SpriteRenderer sR in ani.GetComponentsInChildren<SpriteRenderer>())
                sR.color = Color.HSVToRGB(ran, 1, 1);
        }

        if(Time.timeSinceLevelLoad - lastBeatTime > 0.1f) // security to avoid two beat at the same or next frame
        {
            //calcul how long between each beat :
            print("Beat timing : " + (Time.timeSinceLevelLoad - lastBeatTime));


            GameManager.instance.beatTiming = Time.timeSinceLevelLoad - lastBeatTime;
            lastBeatTime = Time.timeSinceLevelLoad;

            GameManager.instance.worldMove.Beat();
        }
        

    }

    [ContextMenu("Bar")]
    public void Bar()
    {
        
    }


    private void TestEvent()
    {
            AkSoundEngine.PostEvent(testEvent.Id, this.gameObject);
            Debug.Log("Call the event " + testEvent.Id);
    }




}