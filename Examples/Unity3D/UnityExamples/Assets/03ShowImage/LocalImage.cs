using UnityEngine;
using System.Collections;

public class LocalImage : MonoBehaviour
{

    public Texture2D tex;
    public WWW w;
    public SpriteRenderer renderSpr;

#if UNITY_EDITOR
    string path = "file:///c://Users/jeong/Downloads/image.jpg";
#elif UNITY_ANDROID
    string path = "file:///storage/emulated/0/DCIM/Camera/CAM00442.jpg";
#endif

    void Start()
    {
        LoadTex();

        tex = new Texture2D(Screen.width, Screen.height);
        //tex.Apply();

        renderSpr = this.GetComponent<SpriteRenderer>();
    }

    void LoadTex()
    {
        w = new WWW(path);
    }

    void Update()
    {
        if (w.isDone)
        {
            Debug.Log("done");
            tex = w.texture;
            renderSpr.sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f), 100);
        }
    }
}