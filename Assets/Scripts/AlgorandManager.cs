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
    [Header("Object References")]
    [SerializeField] private TMP_InputField _walletAddressInput;

    [Header("Script References")]
    [SerializeField] private AssetCreator _assetCreator;
    [SerializeField] private MenuUIController _menuUIController;

    public HttpClient _client = HttpClientConfigurator.ConfigureHttpClient("https://testnet-algorand.api.purestake.io/ps2", APIKeyManager.AlgodAPIKey);
    private string _walletAddress;
    public Account account;
    public static List<Asset> assets;
    public static List<ulong> amounts;

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

    protected DefaultApi GetAlgodApi()
    {
        var algodApi = new DefaultApi(_client);
        return algodApi;
    }

    public async void SetWalletAddress()
    {
        _walletAddress = _walletAddressInput.text;
        await GetWalletInfo();

    }

    private async Task GetWalletInfo()
    {

        string ALGOD_API_ADDR = "https://mainnet-algorand.api.purestake.io/ps2";
        string ALGOD_API_TOKEN = APIKeyManager.AlgodAPIKey;

        using var httpClient = HttpClientConfigurator.ConfigureHttpClient(GetAlgodAPIAddress(), ALGOD_API_TOKEN);
        DefaultApi algodApiInstance = new DefaultApi(httpClient);

        account = await algodApiInstance.AccountInformationAsync(_walletAddress, null, Algorand.Algod.Model.Format.Json);

        assets = new List<Asset>();

        int i = 1;
        foreach (var asset in account.Assets)
        {
            try
            {
                var assetInfo = await algodApiInstance.GetAssetByIDAsync(asset.AssetId);
                assets.Add(assetInfo);
                amounts.Add(asset.Amount);
                i++;
            }
            catch (Exception ex)
            {
                Debug.Log("Problem recovering asset data: " + ex.Message);
            }
        }


        Debug.Log($"Total Assets: {account.Assets.Count}");
        _menuUIController.ActivateAssetMenu();
        _assetCreator.StartDownloading(assets, amounts);
    }


}
