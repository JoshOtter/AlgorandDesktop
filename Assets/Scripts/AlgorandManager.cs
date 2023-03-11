using Algorand;
using Algorand.Algod;
using Algorand.Algod.Model;
using Algorand.Utils;
using Algorand.Indexer;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using TMPro;
using UnityEngine;

public class AlgorandManager : MonoBehaviour
{
    [SerializeField] private TMP_InputField _walletAddressInput;

    public static HttpClient _client = HttpClientConfigurator.ConfigureHttpClient("https://testnet-algorand.api.purestake.io/ps2", APIKeyManager.AlgodAPIKey);
    public static string walletAddress;

    protected static string GetAlgodAPIAddress()
    {
        string testnetAddress = "https://testnet-algorand.api.purestake.io/ps2";
        string mainnetAddress = "https://mainnet-algorand.api.purestake.io/ps2";

        string _ALGOD_API_ADDR = mainnetAddress;
        if (_ALGOD_API_ADDR.IndexOf("//") == -1)
        {
            _ALGOD_API_ADDR = "http://" + _ALGOD_API_ADDR;
        }
        return _ALGOD_API_ADDR;
    }

    protected static DefaultApi GetAlgodApi()
    {
        var algodApi = new DefaultApi(_client);
        return algodApi;
    }

    public async void SetWalletAddress()
    {
        walletAddress = _walletAddressInput.text;
        await GetWalletInfo();
    }

    private async Task GetWalletInfo()
    {

        var algodApi = GetAlgodApi();
        try
        {
            var accountInfo = await algodApi.AccountInformationAsync(walletAddress, null, Algorand.Algod.Model.Format.Json);

            Debug.Log($"Total Assets: {accountInfo.Assets.Count}");
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
        }
    }
}
