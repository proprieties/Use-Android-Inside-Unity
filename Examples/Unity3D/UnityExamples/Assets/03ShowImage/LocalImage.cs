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

        StartCoroutine(LoadTex(value => idxCurrent = value, OnCompleteLoad, OnFailedLoad));

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

    IEnumerator<int> LoadTex(Action<int> result, Action complete, Action failed, int idx = 0)
    {
        idx = idx >= PathExternalImage.Count ? 0 : idx;

        try
        {
            w = new WWW(PathExternalImage[idx]);
        }
        catch(Exception genErr)
        {
            Debug.LogError(genErr.ToString());
            failed();
            yield break;
        }

        if (w == null)
        {
            failed();
            yield break;
        }

        yield return idx;
        result(idx);
        complete();

        if (w != null)
        {
            w.Dispose();
            w = null;
        }
    }

    void OnFailedLoad()
    {
        Debug.LogError("Image Load Failed");
    }

    void OnCompleteLoad()
    {
        tex = w.texture;
        //tex.Apply();
        rawImg.transform.localPosition = Vector3.zero;
        sizeOrigin.x = Screen.height * tex.width / tex.height;
        sizeOrigin.y = Screen.height;
        sizeZoom = 1f;
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, Screen.height * tex.width / tex.height);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, Screen.height);
        rawImg.texture = w.texture;
    }

    void Update()
    {
        if (w != null && w.isDone)
        {
            Debug.Log("done");
        }
    }

    public void Move(float x, float y)
    {
        rawImg.rectTransform.localPosition += new Vector3(x, y);
    }

    Vector2 sizeOrigin;
    float sizeZoom = 1f;
    public void Zoom(float z)
    {
        sizeZoom += z;
        if (sizeZoom < 0) sizeZoom = 0;
        Debug.LogError(z + " : " + sizeZoom);
        //size = size > 0 ? 1.5f : 0.5f;
        //rawImg.rectTransform.sizeDelta.Scale(Vector2.one * z);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, sizeOrigin.x * sizeZoom);
        rawImg.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, sizeOrigin.y * sizeZoom);
    }

    void OnGUI()
    {
        if (GUILayout.Button("github://proprieties/pickit", GUILayout.Width(200), GUILayout.Height(100)))
            Application.OpenURL("https://github.com/proprieties/Use-Android-Inside-Unity");
        if (GUILayout.Button("Next Image", GUILayout.Width(200), GUILayout.Height(100)))
            StartCoroutine(LoadTex(value => idxCurrent = value, OnCompleteLoad, OnFailedLoad, ++idxCurrent));
        if (GUILayout.Button("Get Path Test", GUILayout.Width(200), GUILayout.Height(100)))
            CallStaticFunctions.GetPath();
        //if (GUILayout.Button("Next Level"))
        //    LoadNextLevel();
    }
}