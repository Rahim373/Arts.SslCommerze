using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace ARTS.SslCommerzeCore
{
    public class Trasnaction
    {
        private string _curency;

        public Trasnaction(decimal totalAmount, string transactionID, string successUrl, string failUrl, string cancelUrl,
            EmiTransaction emiTransaction, Customer customer)
        {
            if (totalAmount < 10 || totalAmount > 500000)
                throw new ArgumentOutOfRangeException(nameof(totalAmount), "TotalAmount must be from 10 to 500000 BDT");
            if (string.IsNullOrEmpty(transactionID))
                throw new ArgumentNullException(nameof(transactionID), "TrasactionID mustn't be empty");
            if (string.IsNullOrEmpty(successUrl))
                throw new ArgumentNullException(nameof(successUrl), "SuccessUrl mustn't be empty");
            if (string.IsNullOrEmpty(failUrl))
                throw new ArgumentNullException(nameof(failUrl), "FailUrl mustn't be empty");
            if (string.IsNullOrEmpty(cancelUrl))
                throw new ArgumentNullException(nameof(cancelUrl), "CancelUrl mustn't be empty");

            this.TotalAmount = totalAmount;
            this.TransactionID = transactionID;
            this.SuccessUrl = successUrl;
            this.FailUrl = failUrl;
            this.CancelUrl = cancelUrl;
            this.EmiTransaction = emiTransaction ?? throw new ArgumentNullException(nameof(emiTransaction));
            this.Customer = customer ?? throw new ArgumentNullException(nameof(customer));
        }

        public string StoreId { get { return Configuration.STORE_ID; } }
        public string StorePassword { get { return Configuration.STORE_PASS; } }
        public decimal TotalAmount { get; set; }
        public string Curency { get => string.IsNullOrEmpty(Curency1) ? "BDT" : Curency1; set => Curency1 = value; }
        public string TransactionID { get; set; }
        public string SuccessUrl { get; set; }
        public string FailUrl { get; set; }
        public string CancelUrl { get; set; }
        public string MultiCardName { get; set; }
        public EmiTransaction EmiTransaction { get; private set; }
        public Customer Customer { get; private set; }
        public ShipmentInformation Shipment { get; set; }
        public string ValueA { get; set; }
        public string ValueB { get; set; }
        public string ValueC { get; set; }
        public string ValueD { get; set; }
        public CartInformation Cart { get; set; }
        public string Curency1 { get => _curency; set => _curency = value; }
    }

    public class EmiTransaction
    {
        public EmiTransaction(bool isEmiEnabled = false)
        {
            this.IsEmiEnabled = isEmiEnabled;
        }

        public bool IsEmiEnabled { get; set; }
        public int? MaxInstallationOption { get; set; }
        public int? SelectedInstallment { get; set; }
    }

    public class Customer
    {
        public Customer(string name, string email, string phone)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentException(nameof(name), "Name mustn't be empty");
            if (string.IsNullOrEmpty(email)) throw new ArgumentException(nameof(email), "Email mustn't be empty");
            if (string.IsNullOrEmpty(phone)) throw new ArgumentException(nameof(phone), "Phone mustn't be empty");

            this.Name = name;
            this.Email = email;
            this.Phone = phone;
        }

        public string Name { get; }
        public string Email { get; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
        public string Phone { get; set; }
        public string Fax { get; set; }
    }

    public class ShipmentInformation
    {
        public string ShipmentAddressName { get; set; }
        public string AddressOne { get; set; }
        public string AddressTwo { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostCode { get; set; }
        public string Country { get; set; }
    }

    public class CartInformation
    {
        public CartInformation()
        {
            CartItems = new List<CartItem>();
        }

        public ICollection<CartItem> CartItems { get; set; }
        public decimal? ProductAmount { get; set; }
        public decimal? Vat { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? ConvenienceFee { get; set; }
    }

    public class CartItem
    {
        public CartItem(string product, decimal amount)
        {
            if (string.IsNullOrEmpty(product))
                throw new ArgumentException("Product name mustn't be empty", nameof(product));
            if (amount < 0)
                throw new ArgumentException("Amount can't be less than zero.", nameof(amount));

            this.Product = product;
            this.Amount = amount;
        }
        public string Product { get; set; }
        public decimal Amount { get; set; }
    }

    public class Gw
    {

        [JsonProperty("visa")]
        public string Visa { get; set; }

        [JsonProperty("master")]
        public string Master { get; set; }

        [JsonProperty("amex")]
        public string Amex { get; set; }

        [JsonProperty("othercards")]
        public string OtherCards { get; set; }

        [JsonProperty("internetbanking")]
        public string InternetBanking { get; set; }

        [JsonProperty("mobilebanking")]
        public string MobileBanking { get; set; }
    }

    public class Description
    {
        [JsonProperty("name")]
        public string Name { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        [JsonProperty("logo")]
        public string Logo { get; set; }

        [JsonProperty("gw")]
        public string Gw { get; set; }
    }

    public class TransactionSession
    {
        [JsonProperty("status")]
        public string Status { get; set; }

        [JsonProperty("failedreason")]
        public string FailedReason { get; set; }

        [JsonProperty("sessionkey")]
        public string SessionKey { get; set; }

        [JsonProperty("gw")]
        public Gw Gateway { get; set; }

        [JsonProperty("redirectGatewayURL")]
        public string RedirectGatewayUrl { get; set; }

        [JsonProperty("GatewayPageURL")]
        public string GatewayPageUrl { get; set; }

        [JsonProperty("directPaymentURL")]
        public string DirectPaymentUrl { get; set; }

        [JsonProperty("storeBanner")]
        public string StoreBanner { get; set; }

        [JsonProperty("storeLogo")]
        public string StoreLogo { get; set; }

        [JsonProperty("desc")]
        public List<Description> Description { get; set; }

        [JsonProperty("is_direct_pay_enable")]
        public string IsDirectPayEnabled { get; set; }
    }

    public class TransactionResponse
    {
        [JsonProperty("status")]
        public TransactionStatus Status { get; set; }

        [JsonProperty("tran_date")]
        public DateTime TransactionDateTime { get; set; }

        [JsonProperty("tran_id")]
        public string TransactionId { get; set; }

        [JsonProperty("val_id")]
        public string ValidationId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("store_amount")]
        public decimal StoreAmount { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_no")]
        public string CardNo { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("bank_tran_id")]
        public string BankTransactionID { get; set; }

        [JsonProperty("card_issuer")]
        public string CardIssuer { get; set; }

        [JsonProperty("card_brand")]
        public string CardBrand { get; set; }

        [JsonProperty("card_issuer_country")]
        public string CardIssuerCountry { get; set; }

        [JsonProperty("card_issuer_country_code")]
        public string CardIssuerCountryCode { get; set; }

        [JsonProperty("currency_type")]
        public string CurrencyType { get; set; }

        [JsonProperty("currency_amount")]
        public decimal CurrencyAmount { get; set; }

        [JsonProperty("value_a")]
        public string ValueA { get; set; }

        [JsonProperty("value_b")]
        public string ValueB { get; set; }

        [JsonProperty("value_c")]
        public string ValueC { get; set; }

        [JsonProperty("value_d")]
        public string ValueD { get; set; }

        [JsonProperty("verify_sign")]
        public string VerifySign { get; set; }

        [JsonProperty("verify_key")]
        public string VerifyKey { get; set; }

        [JsonProperty("risk_level")]
        public int RiskLevel { get; set; }

        [JsonProperty("risk_title")]
        public string RiskTitle { get; set; }
    }

    public class ValidatedTransaction
    {
        public ValidatedTransaction()
        {
            Status = ValidationStatus.INVALID_TRANSACTION;
        }

        [JsonProperty("status")]
        public ValidationStatus Status { get; set; }

        [JsonProperty("tran_date")]
        public DateTime TransactionDate { get; set; }

        [JsonProperty("tran_id")]
        public string TransactionId { get; set; }

        [JsonProperty("val_id")]
        public string ValidationId { get; set; }

        [JsonProperty("amount")]
        public decimal Amount { get; set; }

        [JsonProperty("store_amount")]
        public decimal StoreAmount { get; set; }

        [JsonProperty("card_type")]
        public string CardType { get; set; }

        [JsonProperty("card_no")]
        public string CardNumber { get; set; }

        [JsonProperty("currency")]
        public string Currency { get; set; }

        [JsonProperty("bank_tran_id")]
        public string BankTransactionId { get; set; }

        [JsonProperty("card_issuer")]
        public string CardIssuer { get; set; }

        [JsonProperty("card_brand")]
        public string CardBrand { get; set; }

        [JsonProperty("card_issuer_country")]
        public string CardIssuerCountry { get; set; }

        [JsonProperty("card_issuer_country_code")]
        public string CardIssuerCountryCode { get; set; }

        [JsonProperty("currency_type")]
        public string CurrencyType { get; set; }

        [JsonProperty("currency_amount")]
        public decimal CurrencyAmount { get; set; }

        [JsonProperty("emi_instalment")]
        public int EmiInstallment { get; set; }

        [JsonProperty("emi_amount")]
        public decimal EmiAmount { get; set; }

        [JsonProperty("discount_percentage")]
        public decimal DiscountPercentage { get; set; }

        [JsonProperty("discount_remarks")]
        public string DiscountRemark { get; set; }

        [JsonProperty("currency_rate")]
        public decimal CurrencyRate { get; set; }

        [JsonProperty("base_fair")]
        public decimal BaseFair { get; set; }

        [JsonProperty("value_a")]
        public string ValueA { get; set; }

        [JsonProperty("value_b")]
        public string ValueB { get; set; }

        [JsonProperty("value_c")]
        public string ValueC { get; set; }

        [JsonProperty("value_d")]
        public string ValueD { get; set; }

        [JsonProperty("risk_title")]
        public string RiskTitle { get; set; }

        [JsonProperty("risk_level")]
        public string RiskLevel { get; set; }

        [JsonProperty("APIConnect")]
        public ApiConnectStatus APIConnect { get; set; }

        [JsonProperty("validated_on")]
        public DateTime ValidatedOn { get; set; }

        [JsonProperty("gw_version")]
        public string GwVersion { get; set; }
    }

    public enum ValidationStatus
    {
        VALID,
        VALIDATED,
        INVALID_TRANSACTION
    }

    public enum TransactionQueryStatus
    {
        VALID,
        VALIDATED,
        PENDING,
        FAILED
    }

    public enum TransactionStatus
    {
        VALID,
        FAILED,
        CANCELLED
    }

    public enum ApiConnectStatus
    {
        INVALID_REQUEST, FAILED, INACTIVE, DONE
    }

    public enum RefundStatus
    {
        success, failed, processing
    }

    public class RefundResponse
    {
        [JsonProperty("APIConnect")]
        public ApiConnectStatus ApiConnect { get; set; }

        [JsonProperty("bank_tran_id")]
        public string BankTransactionID { get; set; }

        [JsonProperty("trans_id")]
        public string TransactionID { get; set; }

        [JsonProperty("refund_ref_id")]
        public string RefunndRefID { get; set; }

        [JsonProperty("status")]
        public RefundStatus RefundStatus { get; set; }

        [JsonProperty("errorReason")]
        public string ErrorReason { get; set; }
    }

    public class RefundQueryResponse : RefundResponse
    {
        [JsonProperty("initiated_on")]
        public DateTime InitiatedOn { get; set; }

        [JsonProperty("refunded_on")]
        public DateTime RefundedOn { get; set; }
    }

    public class TransactionQueryResponse : ValidatedTransaction
    {
        [JsonProperty("status")]
        public new TransactionQueryStatus Status { get; set; }
    }

    public class TransactionQueryBySessionResponse : TransactionQueryResponse
    {

        [JsonProperty("sessionkey")]
        public string SessionKey { get; set; }
    }

    public class TransactionQueryByTransIdResponse
    {
        [JsonProperty("APIConnect")]
        public ApiConnectStatus ApiConnect { get; set; }

        [JsonProperty("no_of_trans_found")]
        public int NoOfTransaction { get; set; }

        [JsonProperty("element")]
        public List<TransactionQueryResponse> Elements { get; set; }
    }
}