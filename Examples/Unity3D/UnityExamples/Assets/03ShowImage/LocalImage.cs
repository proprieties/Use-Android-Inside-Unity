using UnityEngine;
using System.Collections.Generic;
using System;

public class LocalImage : MonoBehaviour
{
    public Texture2D tex;
    public WWW w;
    //public SpriteRenderer renderSpr;
    List<String> PathExternalImage = new List<String>();
    int idxCurrent = 0;
    public UnityEngine.UI.RawImage rawImg;

    void Start()
    {
        InitializeMediaPath(ref PathExternalImage);

        tex = new Texture2D(Screen.width, Screen.height);

        //renderSpr = this.gameObject.AddComponent<SpriteRenderer>();
        //renderSpr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);

        StartCoroutine(LoadTex(value => idxCurrent = value, OnCompleteLoad));

    }

    void InitializeMediaPath(ref List<string> ExternalPath)
    {
        switch(Application.platform)
        {
            case RuntimePlatform.WindowsPlayer:
            case RuntimePlatform.WindowsEditor:
                ExternalPath.Add("file:///c://Users/jeong/Downloads/image.jpg");
                ExternalPath.Add("file:///c://Users/jeong/Downloads/image2.jpg");
                break;
            case RuntimePlatform.Android:
                this.transform.parent.GetComponent<CallStaticFunctions>().GetString();
                ExternalPath = CallStaticFunctions.GetAllGalleryImagePaths(); //"file:///storage/emulated/0/DCIM/Camera/CAM00442.jpg";
                break;
        }
    }

    IEnumerator<int> LoadTex(Action<int> result, Action complete, int idx = 0)
    {
        idx = idx >= PathExternalImage.Count ? 0 : idx;
        w = new WWW(PathExternalImage[idx]);
        result(idx);
        yield return idx;

        complete();
    }

    void OnCompleteLoad()
    {
        tex = w.texture;
        //tex.Apply();
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.height * tex.width / tex.height);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        rawImg.texture = w.texture;
    }

    void Update()
    {
        if (w.isDone)
        {
            Debug.Log("done");
        }
    }

    void Zoom(float z)
    {
        //rawImg.rectTransform.sizeDelta.Scale(Vector2.one * z);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, rawImg.rectTransform.rect.width * z);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, rawImg.rectTransform.rect.height * z);
    }

    void OnGUI()
    {
        if (GUILayout.Button("github://proprieties/pickit", GUILayout.Width(200), GUILayout.Height(100)))
            Application.OpenURL("https://github.com/proprieties/Use-Android-Inside-Unity");
        if (GUILayout.Button("Next Image", GUILayout.Width(200), GUILayout.Height(100)))
            StartCoroutine(LoadTex(value => idxCurrent = value, OnCompleteLoad, ++idxCurrent));
        if (GUILayout.Button("Zoom In", GUILayout.Width(200), GUILayout.Height(100)))
            Zoom(1.5f);
        if (GUILayout.Button("Zoom Out", GUILayout.Width(200), GUILayout.Height(100)))
            Zoom(0.5f);
        if (GUILayout.Button("Get Path Test", GUILayout.Width(200), GUILayout.Height(100)))
            CallStaticFunctions.GetPath();
        //if (GUILayout.Button("Next Level"))
        //    LoadNextLevel();
    }
}