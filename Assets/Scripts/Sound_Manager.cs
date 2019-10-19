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


    [Header("A little surprise")]
    public Animator ani;

    // Start is called before the first frame update
    void Start()
    {
        AkSoundEngine.PostEvent(startEvent.Id, this.gameObject);

        startEvent.Post(this.gameObject, (uint)AkCallbackType.AK_MusicSyncBeat 
                                        | (uint)AkCallbackType.AK_EndOfEvent
                                        | (uint)AkCallbackType.AK_MusicSyncBar
                                        | (uint)AkCallbackType.AK_MusicSyncAll, CallBackFunction, this);
    }

    private void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
        switch (type)
        {
            case AkCallbackType.AK_EndOfEvent:
                Debug.Log("Call by " + type + " time = " + Time.timeSinceLevelLoad);
                break;
            case AkCallbackType.AK_MusicSyncBeat:
                Debug.Log("Call by " + type + " time = " + Time.timeSinceLevelLoad);
                break;
            case AkCallbackType.AK_MusicSyncBar:
                Debug.Log("Call by " + type + " time = " + Time.timeSinceLevelLoad);
                break;
            case AkCallbackType.AK_MusicSyncAll:
                Debug.Log("Call by " + type + " time = "+ Time.timeSinceLevelLoad);
                break;
        }
        Beat();
    }

    [ContextMenu("Beat")]
    public void Beat()
    {
        ani.SetTrigger("Beat");
        float ran = Random.Range(0f, 1f);
        foreach (SpriteRenderer sR in ani.GetComponentsInChildren<SpriteRenderer>())
            sR.color = Color.HSVToRGB(ran, 1, 1);
    }


    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            AkSoundEngine.PostEvent(testEvent.Id, this.gameObject);
            Debug.Log("Call the event " + testEvent.Id);
        }
    }




}