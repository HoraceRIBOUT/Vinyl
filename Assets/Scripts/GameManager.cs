//using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public bool _DebugMode;

    public WorldMovement worldMove;
    public PlayerController player;

    public float beatTiming = 0.45f;

    public void Awake()
    {
#if !UNITY_EDITOR
        _DebugMode = false;
#endif

        if (instance == null)
            instance = this;
        else
            Debug.LogError("Already have the GameManager");
    }



    
}
