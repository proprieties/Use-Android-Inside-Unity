﻿using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class InjectSimpleAndroidUI : MonoBehaviour {

	// Use this for initialization
	void Start () {

        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android function is to be called");

            var ajc = new AndroidJavaClass("com.example.xyz.uihelper.Utils"); 
            ajc.CallStatic("AddImageView", GetAndroidActivity(),  0, 0);
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}

    public static AndroidJavaObject GetAndroidActivity()
    {
        AndroidJavaClass jc = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        AndroidJavaObject jo = jc.GetStatic<AndroidJavaObject>("currentActivity");
        return jo;
    }


    public void LoadPreviousLevel()
    {
        SceneManager.LoadScene("01CallStaticAndroidFunction");
    }
}