using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IAmazonPaymentService
    {
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<AmazonPayment> GetAmazonPayments(string userId);

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        Task<bool> RedeemAmazonBalance(RedeemPayment payment);
    }
}
