using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetHandler : MonoBehaviour
{
    public AssetObject assetObject;
    public TextMeshProUGUI assetName;
    public Image assetImage;

    public void InitializeAsset(Sprite sprite, Asset asset, ulong amount)
    {
        assetObject.InitializeAssetData(sprite, asset, amount);

        assetName.text = assetObject.assetName;
        assetImage.sprite = assetObject.assetImage;
    }
}
