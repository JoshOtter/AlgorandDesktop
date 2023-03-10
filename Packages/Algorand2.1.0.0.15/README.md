


# Dotnet Algorand SDK

The dotnet-algorand-skd is a dotnet library for communicating and interacting with the Algorand network.

Interaction with the Algorand network is carried out mainly via the Algod HTTP API and Indexer HTTP API endpoints. This SDK offers API clients for both of those endpoints.

Most operations involve producing, submitting and examining the status of transactions. To help achieve this the SDK offers a class model (Algod/Model/Transactions) of Algorand transactions and their properties. Transactions can then be instantiated, signed, sent and read back using this model. There are additional utility methods, such as on the Account class, which simplify tasks like signing the transaction and correlating transaction identifiers.

The Indexer client is used for connecting to the Algorand Indexer, which offers a predefined set of queries over a Postgres database of Algorand Blocks and Transactions. In this SDK the Indexer client and entity model is separate from Algod, mainly because the model in Indexer is expected to be an ongoing superset of fields and properties over historical versions of the model. For example, if a field becomes redundant, changes meaning, or is split into new fields, the Indexer model will continue to offer the historical view.

## Installation

From the NuGet command line you can execute:

```powershell
Install-Package Algorand2
```
Or from Project -> Manage NuGet Packages

The Nuget package is here <https://www.nuget.org/packages/Algorand2/>

UPDATE: There is now a Unity build at <https://www.nuget.org/packages/Algorand2_Unity/1.0.0.10>

The Unity build allows the DLL to be used directly in Unity without Newtonsoft or Codedom conflicts.
It also offers a new optional parameter to HttpClientConfigurator allowing a shim to be injected
so that WebGL builds can use a different http client by delegation.

## Note about the KMD API

This legacy implementation is due for replacement. The KMD API as is is barely functional,
uses the RestSharp client, and is not connected to Algorand Codegen. Please expect
the KMD API to be completely changed in the coming months.

## Quick Start

For a set of working examples please see the sdk-examples project.

### Get a node and account(s) 
To get working with the Algorand network you will need access to a node. There are a number of ways to do this. The quickest ways of getting set up are to rent access to a node from a service like PureStake, or to install the Algorand Sandbox.

To install the Algorand Sandbox please see instructions here: <https://github.com/algorand/sandbox>

Once you have a node you will get two key pieces of information:

- The API URL
- The API KEY

If you installed sandbox you will also be given some dev pre-made test accounts. You will need to run [this command](https://developer.algorand.org/docs/clis/goal/account/export/) to extract the account mnemonic.

An account mnemonic is a textual representation, a string of English language words, of the private key of the account.

So in all you should now have:

- The API URL and KEY
- Account addresses
- Account private key expressed as a mnemonic

### Connecting to the network

In Visual Studio create a new Console App and add the NuGet as described above.

In the Main method add code as follows:

```cs
namespace sdk_examples
{
    class BasicExample
    {
        public static async Task Main(string[] args)
        {
            string ALGOD_API_ADDR = "<YOUR API URL, eg:http://localhost:4001/>";
            string ALGOD_API_TOKEN = "<YOUR API KEY>";

            var httpClient = HttpClientConfigurator.ConfigureHttpClient(ALGOD_API_ADDR, ALGOD_API_TOKEN);
            DefaultApi algodApiInstance = new DefaultApi(httpClient);

```

*Technical note: when specifying the Host in HttpClientConfigurator, a trailing slash is automatically added to that host so that relative URIs can be combined with it correctly.
If you are using DI to inject the HttpClient then the base URL should use a trailing slash (eg ps2/ on purestake) as according to https://datatracker.ietf.org/doc/html/rfc3986
and HttpClient documentation remarks.*

The above creates an httpClient and passes it into a new client of the AlgoD "default" api set. There are actually three APIs. "Common", "Default" and "Private". The "Common" and "Default" APIs contain the methods you will be using to interact with the network. The "Private" APIs are not exposed by the SDK, require a different type of authentication key, and are reserved for use by the Algorand command line tools.

### Test the connection

Let's call the network and get some information:

```cs
            try
            {
                var supply = await algodApiInstance.GetSupplyAsync();
                Console.WriteLine("Total Algorand Supply: " + supply.TotalMoney);
                Console.WriteLine("Online Algorand Supply: " + supply.OnlineMoney);

            }
            catch (Algorand.Algod.Model.ApiException<ErrorResponse> e)
            {
                Console.WriteLine("Exception when calling algod#getSupply:" + e.Result.Message);
            }

```

The above asks the Algorand network for information on the total money supply. 

**Important** The ```ApiException<ErrorResponse>``` exception type is needed to catch information returned by the Algorand node in the case of an error. The ```e.Result.Message``` contains the error information.

### Make a payment from one account to another

When you use the Algorand Sandbox your node is initialised with some test accounts. At the time of writing there are three developer accounts created by the sandbox.

As described above you will want to try getting the mnemonic representation of the private key of one of those test accounts.

Use another as a source account.

Modify the above code to add something like the following, replacing the values with those specific to your sandbox:

```cs
            string DEST_ADDR = "KV2XGKMXGYJ6PWYQA5374BYIQBL3ONRMSIARPCFCJEAMAHQEVYPB7PL3KU";
            string SRC_ACCOUNT = "lift gold aim couch filter amount novel scrap annual grow amazing pioneer disagree sense phrase menu unknown dolphin style blouse guide tell also about case";

            Account src = new Account(SRC_ACCOUNT);
            Console.WriteLine("My account address is:" + src.Address.ToString());

```

The above invokes the Account constructor overload for interpreting mnemonics into private key values and creates a representation of an Algorand "Account".

Before we can create a Transaction, we need to get some information about the network. This information is general (such as that which identifies which sub-network of Algorand we are on, like the main, test or beta networks), and specific (such as the current time or 'round' of the network, to set transaction validity duration).

To achieve this add this into the code above:

```cs
            TransactionParametersResponse transParams;
            try
            {
                transParams = await algodApiInstance.TransactionParamsAsync();
            }
            catch (Algorand.Algod.Model.ApiException<ErrorResponse> e)
            {
                Console.WriteLine("Exception when calling algod#getSupply:" + e.Result.Message);
            }

```

Now, with the above network state information, we will send a microalgo from one account to another.

To achieve this we will use a help method on the ```PaymentMethod``` class.

Add the following code:

```cs
            var amount = Utils.AlgosToMicroalgos(1);
            
            var tx = PaymentTransaction.GetPaymentTransactionFromNetworkTransactionParameters(src.Address, new Address(DEST_ADDR), amount, "pay message", transParams);
```

After that, we need to sign the transaction using the sender account, for which we have the private key:

```cs
            // payment transactions must be signed by the sender
            var signedTx = tx.Sign(src);

            Console.WriteLine("Signed transaction with txid: " + signedTx.Tx.TxID());

```

Now let's send it to the network and execute the payment:

```cs
            // send the transaction to the network
            try
            {
                var id = await Utils.SubmitTransaction(algodApiInstance, signedTx);
                Console.WriteLine("Successfully sent tx with id: " + id.Txid);
                
                var resp = await Utils.WaitTransactionToComplete(algodApiInstance, id.Txid) as Transaction;
                
                Console.WriteLine("Confirmed Round is: " + resp.ConfirmedRound);
            }
            catch (ApiException<ErrorResponse> e)
            {
                // This is generally expected, but should give us an informative error message.
                Console.WriteLine("Exception when calling algod#rawTransaction: " + e.Result.Message);
            }
```

The above submits our transaction, gets the id, sends that back to the node and asks to be notified when the transaction completes. This should take on average about 2 seconds even on the live main network. 

That's it! You have used .NET to interact with Algorand, work a bit with Accounts and send a payment from one account to another.

## 4. Quick Start for Indexer

As we all know blockchain has a chain data struct, so it's very different for us to search the data. So algorand retrieve the blockchain data from a PostgreSQL compatible database. Then we can search for the blockchain very easily.

![Algorand Indexer](indexerv2.png)

The indexer has 12 methods to search the blockchain and some of these methods have a lot of variables to control the result. Let's try some code.

```csharp
string ALGOD_API_ADDR = "your algod api address";
string ALGOD_API_TOKEN = "your algod api token";

IndexerApi indexer = new IndexerApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
//AlgodApi algodApiInstance = new AlgodApi(ALGOD_API_ADDR, ALGOD_API_TOKEN);
var health = indexer.MakeHealthCheck();
Console.WriteLine("Make Health Check: " + health.ToJson());

System.Threading.Thread.Sleep(1200); //test in purestake, imit 1 req/sec
var address = "KV2XGKMXGYJ6PWYQA5374BYIQBL3ONRMSIARPCFCJEAMAHQEVYPB7PL3KU";
var acctInfo = indexer.LookupAccountByID(address);
Console.WriteLine("Look up account by id: " + acctInfo.ToJson());

System.Threading.Thread.Sleep(1200); //test in purestake, limit 1 req/sec
var transInfos = indexer.LookupAccountTransactions(address, 10);
Console.WriteLine("Look up account transactions(limit 10): " + transInfos.ToJson());

System.Threading.Thread.Sleep(1200); //test in purestake, limit 1 req/sec
var appsInfo = indexer.SearchForApplications(limit: 10);
Console.WriteLine("Search for application(limit 10): " + appsInfo.ToJson());

var appIndex = appsInfo.Applications[0].Id;
System.Threading.Thread.Sleep(1200); //test in purestake, limit 1 req/sec
var appInfo = indexer.LookupApplicationByID(appIndex);
Console.WriteLine("Look up application by id: " + appInfo.ToJson());

System.Threading.Thread.Sleep(1200); //test in purestake, limit 1 req/sec
var assetsInfo = indexer.SearchForAssets(limit: 10, unit: "LAT");
Console.WriteLine("Search for assets" + assetsInfo.ToJson());

var assetIndex = assetsInfo.Assets[0].Index;
System.Threading.Thread.Sleep(1200); //test in purestake, limit 1 req/sec
var assetInfo = indexer.LookupAssetByID(assetIndex);
Console.WriteLine("Look up asset by id:" + assetInfo.ToJson());
```

Please enjoy!!!
