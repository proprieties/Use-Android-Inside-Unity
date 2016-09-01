﻿using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class CallStaticFunctions : MonoBehaviour {

    public Text outputTextView;

    // Use this for initialization
    void Start () {
        if (Application.platform == RuntimePlatform.Android)
        {
            Debug.Log("Android function is to be called");

            var ajc = new AndroidJavaClass("com.example.xyz.staticfunctionlib.Helper"); //(1)
            var output = ajc.CallStatic<string>("DoSthInAndroid"); //(2)
            outputTextView.text = output;

            Debug.Log(output);
            Debug.Log("Android function is called");
        }
    }

    // Update is called once per frame
    void Update () {

    }

    void OnGUI()
    {
        if(GUILayout.Button("Next Level"))
            LoadNextLevel();
    }

    public void LoadNextLevel()
    {
        Application.LoadLevel("02InjectSimpleAndroidUI");
    }
}
