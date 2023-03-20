using Algorand.Algod.Model;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AssetHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public AssetObject assetObject;
    public TextMeshProUGUI assetName;
    public Image assetImage;
    public GameObject hoverObject;

    public void InitializeAsset(Sprite sprite, Asset asset, ulong amount)
    {
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
}
