# Arts.SslCommerze | Arts.SslCommerzeCore
[SSLCOMMERZ](https://www.sslcommerz.com) is the first payment gateway in Bangladesh opening doors for merchants to receive payments on the internet via their online stores. Customers are able to buy products online using their credit cards as well as bank accounts.

### Dependencies
#### ASP.NET MVC
- .NETFramework, Version >= v4.5
- Microsoft.AspNet.Mvc, Version >= 5.0.0
#### ASP.NET MVC Core
- Microsoft.AspNetCore.Http >= 2.0.0
- Newtonsoft.Json >= 10.0.1


### Features
- Creating session for initiate a trasaction
- Validation of a transaction
- Refund a transaction
- Refund query
- Transaction query by both SessionId and TransactionId

### Install via nuget
Check from Nuget [gallery](https://www.nuget.org/packages/Arts.SslCommerze)
```
PM > Install-Package Arts.SslCommerze
```
Check from Nuget [gallery](https://www.nuget.org/packages/Arts.SslCommerzeCore) for .NET Core
```
PM > Install-Package Arts.SslCommerzeCore
```

### Configuration
* Add a `sslcommerz.config` file in your project directory. The content will have your store information like **StoreId**, **StorePass**.
**IsSandBox** will be `true` or `false`. If it is true, then all the requests will be sent to the sslCommerze [sandbox url]( https://sandbox.sslcommerz.com), else all will be sent to the [real transaction url](https://securepay.sslcommerz.com).
    ```xml
    <?xml version="1.0" encoding="UTF-8" ?>
    <ArtsSslCommerz>
      <IsSandBox>true</IsSandBox>
      <Credential>
        <Sandbox storeid="SandboxStoreId" pass="SandboxStorePass"/>
        <Live storeid="RealStoreId" pass="RealStorePass" />
      </Credential>
    </ArtsSslCommerz>
    ```

* Use this namespace `ARTS.SslCommerze`
* Place this following code snippet at you application initalization function.
    ```c#
    Configuration.Configure();
    ```
    For MVC application place in `Application_Start()`. and for Core application place in `ConfigureServices` in `Startup.cs`

### How to use
* __Initiat a session for trransaction__:
    ```c#
    string customerName = "Customer name";
    string customerEmail = "Customer email";
    string customerPhone = "Customer phone";
    string transactionId = "transactionId";
    string successUrl = "successUrl";
    string failUrl = "failUrl";
    string cancelUrl = "cancelUrl";
    decimal amount = 50;

    Customer customer = new Customer(customerName, customerEmail, customerPhone);

    EmiTransaction emiTransaction = new EmiTransaction();

    Trasnaction trasnaction = new Trasnaction(amount, transactionId, successUrl, failUrl, 
                                                    cancelUrl, emiTransaction, customer);

    TransactionSession session = SslRequest.GetSessionAsync(trasnaction).Result;
    ```
    The response `TransactionSession ` has `RedirectGatewayUrl`, `DirectPaymentUrl`, `GatewayPageUrl`. Use necessary url for customer to be redirected to pay.
    
* __Verifying a transaction response__:
    Getting a response in your IPN action and process the response like below.
    ```c#
    public void SslTransactionResponse(FormCollection fromCollection)
    {
        TransactionResponse response = SslRequest.GetTransactionResponse(fromCollection);

        if (response.Status == TransactionStatus.VALID)
        {
            ValidatedTransaction validatedTransaction = SslRequest.ValidateTransaction(fromCollection);

            if (validatedTransaction.Status != ValidationStatus.INVALID_TRANSACTION)
            {
                // Update database as your need
            }
        }
    }
    ```

* __Refund__:
    ```c#
    decimal refundAnount = 100;
    RefundResponse res = SslRequest.RefundAsync("bankTrasactionId", refundAnount, "remark", "refId").Result;
    ```
   
* __Query__:
    * Refund Query
        ```c#
        RefundQueryResponse res = SslQuery.RefundQueryAsync("RefundRefId").Result;
        ```

    * Trasaction query by session id
        ```c#
        TransactionQueryBySessionResponse res = SslQuery.TransactionQueryBySessionIdAsync("SessionId").Result;
        ```

    * Trasaction query by trasaction id
        ```c#
        TransactionQueryByTransIdResponse res = SslQuery.TransactionQueryByTransIdAsync("RefundRefId").Result;
        ```
