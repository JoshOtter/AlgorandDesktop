using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class AssetHandler : MonoBehaviour
{
    public AssetObject assetObject;
    public TextMeshProUGUI _assetName;
    public TextMeshProUGUI _assetUnitName;
    public TextMeshProUGUI _assetId;
    public TextMeshProUGUI _assetCreator;
    public TextMeshProUGUI _assetAmount;

    public void InitializeAsset(Asset asset, int amount)
    {
        assetObject.InitializeAssetData(asset, amount);

        _assetName.text = assetObject.assetName;
        _assetUnitName.text = assetObject.assetUnitName;
        _assetId.text = assetObject.assetId.ToString();
        _assetCreator.text = assetObject.assetCreator;
        _assetAmount.text = assetObject.assetAmount.ToString();
    }
}
