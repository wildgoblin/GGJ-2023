using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{

    public static GameController Instance { get; private set; }

    private void Awake()
    {
        Singleton();
    }

    private void Singleton()
    {
        if(Instance != null)
        {
            Instance = this;
            //DontDestroyOnLoad(); MAY NOT NEED THIS
        }
        else
        {
            Destroy(this);
        }
    }


}
