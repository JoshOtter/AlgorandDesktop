using Algorand.Algod.Model;
using UnityEngine;

public class AssetObject : MonoBehaviour
{
    public Asset asset;
    public ulong assetId;
    public string assetName;
    public string assetUnitName;
    public ulong assetAmount;
    public string assetCreator;
    public Sprite assetImage;

    public void InitializeAssetData(Sprite sprite, Asset asset, ulong amount)
    {
        this.asset = asset;
        assetId = asset.Index;
        assetName = asset.Params.Name;
        assetUnitName = asset.Params.UnitName;
        assetAmount = amount;
        assetCreator = asset.Params.Creator.EncodeAsString();
        assetImage = sprite;
    }
}
