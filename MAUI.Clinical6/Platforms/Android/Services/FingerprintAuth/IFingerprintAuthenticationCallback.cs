using System;
using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.Helpers;

namespace FaceIDTest.Droid.Services.FingerprintAuth
{
    public interface IFingerprintAuthenticationCallback : IDisposable
    {
        Task<FingerprintAuthenticatedResult> GetTask();
    }
}