﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private static bool created = false;

    private void Awake()
    {
        if (!created)
        {
            DontDestroyOnLoad(this.gameObject);
            created = true;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
#if UNITY_IOS
    Debug.Log("Screen height = " + Screen.height);
    Camera.main.orthographicSize = (float)Screen.height / 128;
#endif
#if UNITY_WEBGL
        Camera.main.orthographicSize = 8;
#endif

    }
}
