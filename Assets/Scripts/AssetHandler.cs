using Algorand.Algod.Model;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssetHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    public AssetObject assetObject;
    public TextMeshProUGUI assetName;
    public Image assetImage;
    public GameObject hoverObject;
    private AssetCreator _assetCreator;

    public void InitializeAsset(Sprite sprite, Asset asset, ulong amount)
    {
        assetObject = new AssetObject();
        _assetCreator = AssetCreator.assetCreator;
        assetObject.InitializeAssetData(sprite, asset, amount);

        assetName.text = assetObject.assetName;
        assetImage.sprite = assetObject.assetImage;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverObject.SetActive(false);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        hoverObject.SetActive(false);
        _assetCreator.ActivateAssetInfoPanel(assetObject);
    }
}
