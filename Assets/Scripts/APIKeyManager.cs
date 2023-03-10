using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class APIKeyManager : MonoBehaviour
{
    [Header("Object References")]
    [SerializeField] private TMP_InputField _keyInput;
    [SerializeField] private Toggle _rememberKeyToggle;
    [SerializeField] private Button _enterKeyButton;

    [Header("Script References")]
    [SerializeField] private MenuUIController _menuUIController;

    public static string AlgodAPIKey;

    private void Start()
    {
        _enterKeyButton.onClick.AddListener(EnterAlgodKey);
    }

    private void EnterAlgodKey()
    {
        if (string.IsNullOrEmpty(_keyInput.text)) 
        {
            Debug.LogError("Please Enter an Algod API Key");
            return;
        }

        AlgodAPIKey = _keyInput.text;

        if (_rememberKeyToggle.isOn)
        {
            PlayerPrefs.SetString("AlgodAPIKey", AlgodAPIKey);
        }
        else
        {
            if (PlayerPrefs.HasKey("AlgodAPIKey"))
            {
                PlayerPrefs.DeleteKey("AlgodAPIKey");
            }
        }

        _menuUIController.ActivateWalletAddressPanel();
    }
}
