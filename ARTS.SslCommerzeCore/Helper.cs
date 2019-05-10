using Newtonsoft.Json;
using System.Collections.Generic;

namespace ARTS.SslCommerzeCore
{
    internal static class Helper
    {
        internal static Dictionary<string, string> GetSessionRequestParameter(Trasnaction trasnaction)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("store_id", trasnaction.StoreId);
            data.Add("store_passwd", trasnaction.StorePassword);
            data.Add("total_amount", trasnaction.TotalAmount.ToString());
            data.Add("currency", trasnaction.Curency);
            data.Add("tran_id", trasnaction.TransactionID);
            data.Add("success_url", trasnaction.SuccessUrl);
            data.Add("fail_url", trasnaction.FailUrl);
            data.Add("cancel_url", trasnaction.CancelUrl);
            data.Add("multi_card_name", trasnaction.MultiCardName);
            data.Add("emi_option", trasnaction.EmiTransaction.IsEmiEnabled ? "1" : "0");

            if (trasnaction.EmiTransaction.MaxInstallationOption.HasValue)
                data.Add("emi_max_inst_option", trasnaction.EmiTransaction.MaxInstallationOption.ToString());

            if (trasnaction.EmiTransaction.SelectedInstallment.HasValue)
                data.Add("emi_selected_inst", trasnaction.EmiTransaction.SelectedInstallment.ToString());

            data.Add("cus_name", trasnaction.Customer.Name);
            data.Add("cus_email", trasnaction.Customer.Email);

            if (!string.IsNullOrEmpty(trasnaction.Customer.AddressOne))
                data.Add("cus_add1", trasnaction.Customer.AddressOne);
            if (!string.IsNullOrEmpty(trasnaction.Customer.AddressTwo))
                data.Add("cus_add2", trasnaction.Customer.AddressTwo);
            if (!string.IsNullOrEmpty(trasnaction.Customer.City))
                data.Add("cus_city", trasnaction.Customer.City);
            if (!string.IsNullOrEmpty(trasnaction.Customer.State))
                data.Add("cus_state", trasnaction.Customer.State);
            if (!string.IsNullOrEmpty(trasnaction.Customer.PostCode))
                data.Add("cus_postcode", trasnaction.Customer.PostCode);
            if (!string.IsNullOrEmpty(trasnaction.Customer.Country))
                data.Add("cus_country", trasnaction.Customer.Country);
            data.Add("cus_phone", trasnaction.Customer.Phone);
            if (!string.IsNullOrEmpty(trasnaction.Customer.Fax))
                data.Add("cus_fax", trasnaction.Customer.Fax);

            if (!string.IsNullOrEmpty(trasnaction.Shipment?.ShipmentAddressName))
                data.Add("ship_name", trasnaction.Shipment.ShipmentAddressName);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.AddressOne))
                data.Add("ship_add1", trasnaction.Shipment.AddressOne);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.AddressTwo))
                data.Add("ship_add2", trasnaction.Shipment.AddressTwo);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.City))
                data.Add("ship_city", trasnaction.Shipment.City);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.State))
                data.Add("ship_state", trasnaction.Shipment.State);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.PostCode))
                data.Add("ship_postcode", trasnaction.Shipment.PostCode);
            if (!string.IsNullOrEmpty(trasnaction.Shipment?.Country))
                data.Add("ship_country", trasnaction.Shipment.Country);

            if (!string.IsNullOrEmpty(trasnaction.ValueA))
                data.Add("value_a", trasnaction.ValueA);
            if (!string.IsNullOrEmpty(trasnaction.ValueB))
                data.Add("value_b", trasnaction.ValueB);
            if (!string.IsNullOrEmpty(trasnaction.ValueC))
                data.Add("value_c", trasnaction.ValueC);
            if (!string.IsNullOrEmpty(trasnaction.ValueD))
                data.Add("value_d", trasnaction.ValueD);

            if (trasnaction.Cart?.CartItems.Count > 0)
                data.Add("cart", JsonConvert.SerializeObject(trasnaction.Cart.CartItems));
            if (trasnaction.Cart?.ProductAmount != null)
                data.Add("product_amount", trasnaction.Cart.ProductAmount.ToString());
            if (trasnaction.Cart?.ProductAmount != null)
                data.Add("vat", trasnaction.Cart.Vat.ToString());
            if (trasnaction.Cart?.ProductAmount != null)
                data.Add("discount_amount", trasnaction.Cart.DiscountAmount.ToString());
            if (trasnaction.Cart?.ProductAmount != null)
                data.Add("convenience_fee", trasnaction.Cart.ConvenienceFee.ToString());

            return data;
        }

        internal static Dictionary<string, string> GetTransactionValidationParameter(string validationId)
        {
            Dictionary<string, string> data = new Dictionary<string, string>();

            data.Add("val_id", validationId);
            data.Add("store_id", Configuration.STORE_ID);
            data.Add("store_passwd", Configuration.STORE_PASS);
            data.Add("format", "json");

            return data;
        }
    }
}
