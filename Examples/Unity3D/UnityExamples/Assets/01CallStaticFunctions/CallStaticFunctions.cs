using UnityEngine;
using System.Collections;
//using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class CallStaticFunctions : MonoBehaviour {

    public Text outputTextView;

    // Use this for initialization
    void Start () {

        GetString();
    }

    // Update is called once per frame
    void Update () {

    }

    public static List<string> GetAllGalleryImagePaths()
    {
        List<string> results = new List<string>();
        HashSet<string> allowedExtesions = new HashSet<string>() { ".png", ".jpg", ".jpeg" };

        try
        {
            AndroidJavaClass mediaClass = new AndroidJavaClass("android.provider.MediaStore$Images$Media");

            // Set the tags for the data we want about each image.  This should really be done by calling; 
            //string dataTag = mediaClass.GetStatic<string>("DATA");
            // but I couldn't get that to work...

            const string dataTag = "_data";

            string[] projection = new string[] { dataTag };
            AndroidJavaClass player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = player.GetStatic<AndroidJavaObject>("currentActivity");

            string[] urisToSearch = new string[] { "EXTERNAL_CONTENT_URI" }; //, "INTERNAL_CONTENT_URI" };
            foreach (string uriToSearch in urisToSearch)
            {
                AndroidJavaObject externalUri = mediaClass.GetStatic<AndroidJavaObject>(uriToSearch);
                AndroidJavaObject finder = currentActivity.Call<AndroidJavaObject>("managedQuery", externalUri, projection, null, null, null);
                bool foundOne = finder.Call<bool>("moveToFirst");
                while (foundOne)
                {
                    int dataIndex = finder.Call<int>("getColumnIndex", dataTag);
                    string data = finder.Call<string>("getString", dataIndex);
                    if (allowedExtesions.Contains(Path.GetExtension(data).ToLower()))
                    {
                        string path = @"file:///" + data;
                        results.Add(path);
                    }

                    foundOne = finder.Call<bool>("moveToNext");
                }
            }
        }
        catch (System.Exception e)
        {
            // do something with error...
        }

        return results;
    }

    public void GetString()
    {
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

    public static void GetPath () {
		if (Application.platform == RuntimePlatform.Android)
		{
            var ajc = new AndroidJavaClass("com.example.xyz.staticfunctionlib.Helper"); //(1)

            AndroidJavaClass jo = new AndroidJavaClass("android.provider.MediaStore.Images.Media");
            AndroidJavaClass up = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
            AndroidJavaObject currentActivity = up.GetStatic<AndroidJavaObject>("currentActivity");
            AndroidJavaObject contentResolver = currentActivity.Call<AndroidJavaObject>("getContentResolver");

            AndroidJavaObject obj = ajc.CallStatic<AndroidJavaObject>("getPathOfAllImages", contentResolver);
			if (obj.GetRawObject().ToInt32() != 0)
			{
				// String[] returned with some data!
				string[] result = AndroidJNIHelper.ConvertFromJNIArray<string[]>
					(obj.GetRawObject());
				Debug.Log("result : " + result);
			}
			else
				Debug.Log("Failed get array");
		}
	}

    public void LoadNextLevel()
    {
        Application.LoadLevel("02InjectSimpleAndroidUI");
    }
}
