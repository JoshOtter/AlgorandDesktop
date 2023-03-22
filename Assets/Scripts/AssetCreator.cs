using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class AssetCreator : MonoBehaviour
{
    [SerializeField] private GameObject _assetDisplayPrefab;
    [SerializeField] private Transform _AssetContainer;
    [SerializeField] private GameObject _pagePrefab;

    public void StartDownloading(List<Asset> assets, List<ulong> amounts)
    {
        for (var i = 0; i < assets.Count; i++)
        {
            StartCoroutine(DownloadImage(assets[i], amounts[i]));
        }
    }

    private IEnumerator DownloadImage(Asset asset, ulong amount)
    {
        if (string.IsNullOrEmpty(asset.Params.Url))
        {
            Debug.LogWarning("Skipping download due to null or empty URL for asset: " + asset.Params.Name);
            yield break;
        }

        string imageUrl = ConvertIpfsUrlToGatewayUrl(asset.Params.Url);

        UnityWebRequest request = UnityWebRequestTexture.GetTexture(imageUrl);
        yield return request.SendWebRequest();

        if (request.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download image: " + request.error);
        }
        else
        {
            Texture2D texture = DownloadHandlerTexture.GetContent(request);
            Debug.Log(texture);
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

    private string ConvertIpfsUrlToGatewayUrl(string ipfsUrl)
    {
        const string gatewayUrl = "https://gateway.pinata.cloud/ipfs/";
        const string ipfsProtocol = "ipfs://";

        if (ipfsUrl.StartsWith(ipfsProtocol))
        {
            string ipfsHash = ipfsUrl.Substring(ipfsProtocol.Length);
            return gatewayUrl + ipfsHash;
        }

        return ipfsUrl;
    }

}
