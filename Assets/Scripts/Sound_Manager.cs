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

    // Start is called before the first frame update
    void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Call the event " + testEvent.Id);
            AkSoundEngine.PostEvent(testEvent.Id, this.gameObject);
            Debug.Log("Call the event " + testEvent.Id);
        }
    }
}