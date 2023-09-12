using StripeTerminal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace StripeTerminal.Payment
{
    public class PaymentRequest
    {
        //public PaymentRequest(decimal amount, IShoppingCartMetadata metadata)
        //{
        //    Amount = amount;
        //    VenueId = metadata.VenueId?.ToString();
        //    EventId = metadata.EventId?.ToString();
        //    ClientCartId = metadata.ClientCartId;
        //    ServerCartId = metadata.ServerCartId;
        //    DeviceId = metadata.DeviceId;
        //    DeviceName = metadata.DeviceName;
        //}

        public PaymentRequest(decimal amount, Dictionary<string, string> metadata)
        {
            Amount = amount;
            Metadata = metadata;
        }

        public decimal Amount { get; }
        //public string VenueId { get; }
        //public string EventId { get; }
        //public string ClientCartId { get; }
        //public string ServerCartId { get; }
        //public string DeviceId { get; }
        //public string DeviceName { get; }

        public Dictionary<string, string> Metadata { get; }

        //Amounts have to be whole numbers
        //https://stripe.com/docs/currencies#zero-decimal
        public nuint GetAmount() => CalculationHelper.GetAmount(Amount);
    }
}