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

    // Start is called before the first frame update
    void Start()
    {
        
    }
    

}
