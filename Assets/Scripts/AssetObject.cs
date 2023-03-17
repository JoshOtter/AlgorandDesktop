using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

[CreateAssetMenu(fileName = "NewAssetObject", menuName = "AssetObject")]
public class AssetObject : ScriptableObject
{
    public Asset asset;
    public ulong assetId;
    public string assetName;
    public string assetUnitName;
    public int assetAmount;
    public string assetCreator;
    public Sprite assetImage;

    public void InitializeAssetData(Asset asset, int amount)
    {
        this.asset = asset;
        assetId = asset.Index;
        assetName = asset.Params.Name;
        assetUnitName = asset.Params.UnitName;
        assetAmount = amount;
        assetCreator = asset.Params.Creator.EncodeAsString();
    }
}
