using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetCreator : MonoBehaviour
{
    [SerializeField] private GameObject _assetDisplayPrefab;
    [SerializeField] private Transform _AssetContainer;

    public void StartDownloading(List<Asset> assets, List<ulong> amounts)
    {
        for (var i = 0; i < assets.Count; i++)
        {
            StartCoroutine(DownloadImage(assets[i], amounts[i]));
        }
    }

    private IEnumerator DownloadImage(Asset asset, ulong amount)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(asset.Params.Url);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Sprite sprite = TextureToSprite(texture);

            GameObject assetDisplay = Instantiate(_assetDisplayPrefab, _AssetContainer, false);
            AssetHandler display = assetDisplay.GetComponent<AssetHandler>();
            display.InitializeAsset(sprite, asset, amount);
        }
    }

    private Sprite TextureToSprite(Texture2D texture)
    {
        Rect rect = new Rect(0, 0, texture.width, texture.height);
        Vector2 pivot = new Vector2(0.5f, 0.5f);
        return Sprite.Create(texture, rect, pivot);
    }
}
