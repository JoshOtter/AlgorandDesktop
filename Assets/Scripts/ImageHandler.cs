using Algorand.Algod.Model;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class ImageHandler : MonoBehaviour
{
    private UnityWebRequest www;
    private int index = 0;


    public void DownloadImages()
    {
        //Add check if the image is already saved

        DownloadImageTexture(AlgorandManager.assets[index].Params.Url);
    }


    public void DownloadImageTexture(string url)
    {
        www = UnityWebRequestTexture.GetTexture(url);
        www.SendWebRequest().completed += OnGetTextureResult;
    }

    private void OnGetTextureResult(AsyncOperation result)
    {
        if (www.result == UnityWebRequest.Result.Success)
        {
            var texture = DownloadHandlerTexture.GetContent(www);
        }
    }
}
