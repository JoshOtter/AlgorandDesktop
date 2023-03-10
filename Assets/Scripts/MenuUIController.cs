using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuUIController : MonoBehaviour
{
    [SerializeField] private GameObject _algodKeyPanel;
    [SerializeField] private GameObject _walletAddressPanel;

    private void Start()
    {
        if (PlayerPrefs.HasKey("AlgodAPIKey"))
        {
            APIKeyManager.AlgodAPIKey = PlayerPrefs.GetString("AlgodAPIKey");
            ActivateWalletAddressPanel();
        }
        else
        {
            ActivateAlgodKeyPanel();
        }
    }


    public void ActivateWalletAddressPanel()
    {
        _algodKeyPanel.SetActive(false);
        _walletAddressPanel.SetActive(true);
    }

    public void ActivateAlgodKeyPanel()
    {
        _walletAddressPanel.SetActive(false);
        _algodKeyPanel.SetActive(true);
    }
}
