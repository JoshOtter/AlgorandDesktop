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
    [SerializeField] private GameObject _assetObject;

    [SerializeField] private MenuUIController _menuUIController;

    public HttpClient _client = HttpClientConfigurator.ConfigureHttpClient("https://testnet-algorand.api.purestake.io/ps2", APIKeyManager.AlgodAPIKey);
    public string walletAddress;
    public Account account;
    public static List<Asset> assets;
    public static List<Texture2D> textures;

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
        walletAddress = _walletAddressInput.text;
        await GetWalletInfo();

    }

    private async Task GetWalletInfo()
    {

        string ALGOD_API_ADDR = "https://mainnet-algorand.api.purestake.io/ps2";
        string ALGOD_API_TOKEN = APIKeyManager.AlgodAPIKey;

        using var httpClient = HttpClientConfigurator.ConfigureHttpClient(GetAlgodAPIAddress(), ALGOD_API_TOKEN);
        DefaultApi algodApiInstance = new DefaultApi(httpClient);

        account = await algodApiInstance.AccountInformationAsync(walletAddress, null, Algorand.Algod.Model.Format.Json);

        assets = new List<Asset>();
        textures = new List<Texture2D>();

        int i = 1;
        foreach (var asset in account.Assets)
        {
            try
            {
                var assetInfo = await algodApiInstance.GetAssetByIDAsync(asset.AssetId);
                assets.Add(assetInfo);
                if (i == 1)
                {
                    _assetObject.GetComponent<AssetHandler>().InitializeAsset(assetInfo, (int)asset.Amount);
                    _menuUIController.ActivateAssetMenu();
                }
                Debug.Log(assetInfo.Params.Url);
                //Debug.Log($"Asset #{i}: {assetInfo.Params.Name}");
                i++;
            }
            catch (Exception ex)
            {
                Debug.Log("Problem recovering asset data");
            }
        }


        Debug.Log($"Total Assets: {account.Assets.Count}");
    }


}
