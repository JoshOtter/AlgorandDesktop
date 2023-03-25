using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AssetDisplayHandler : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _assetName;
    [SerializeField] private TextMeshProUGUI _assetUnitName;
    [SerializeField] private TextMeshProUGUI _assetId;
    [SerializeField] private TextMeshProUGUI _assetCreator;
    [SerializeField] private TextMeshProUGUI _assetAmount;
    [SerializeField] private Image _assetImage;

    public void DisplayInfo(AssetObject asset)
    {
        _assetImage.sprite = asset.assetImage;
        _assetName.text = asset.assetName;
        _assetUnitName.text = asset.assetUnitName;
        _assetCreator.text = asset.assetCreator;
        _assetAmount.text = asset.assetAmount.ToString();
    }
}
