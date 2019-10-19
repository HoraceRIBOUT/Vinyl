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

        startEvent.Post(this.gameObject, (uint)AkCallbackType.AK_MusicSyncBeat, CallBackFunction);
    }

    private void CallBackFunction(object baseObject, AkCallbackType type, object info)
    {
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