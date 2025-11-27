using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Helpers;
using Clinical6SDK.Services;
using Clinical6SDK.Services.Requests;
using Device = Clinical6SDK.Helpers.Device;

namespace Clinical6SDK
{
    public interface IClinical6Instance
    {
        string AuthToken { get; set; }
        string BaseUrl { get; set; }
        Device Device { get; set; }
        string MobileApplicationKey { set; get; }
        User User { get; set; }

        Task<T> Delete<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel;
        Task<T> Get<T>(IJsonApiModel obj = null, Options options = null) where T : new();
        Task<T> GetChildren<T>(IJsonApiModel obj, string childType, Options options = null) where T : new();
        Task<User> GetProfile();
        Task<T> Insert<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel;
        Task<User> Register<T>(IParams parameters = null, string Json = "", Options options = null) where T : IJsonApiModel;
        Task<T> Update<T>(IJsonApiModel obj, Options options = null) where T : IJsonApiModel;

        Task<bool> SignInGuestAsync();
        Task<bool> SignUpAsync(string accountName, string password, Profile profile);
        Task<bool> SignInAsync(string accountName, string password);
        Task<bool> SignInEmailAsync(string email, string password);
        Task SignOutAsync();
        Task<bool> ForgotPasswordAsync(string email);
        Task<bool> ForgotAccountNameAsync(string email);
        Task<bool> ResetPasswordAsync(string oldPassword, string newPassword, string newPasswordConfirmation);
        Task DisableAccountAsync();
        Task DeleteAccountAsync();
        Task<bool> VerifyAccountAsync(string code);
        Task<bool> SetPinAsync(int pin, int pinConfirmation);
        Task<User> AcceptInvitation<T>(Invitation invitation) where T : IJsonApiModel;
        Task<T> Confirm<T>(string token);
        Task<T> GetRegistrationStatus<T>(IParams parameters = null, string Json = "", Options options = null);
        Task<T> GetSession<T>(string token);
        Task<User> Invite(Invitation invitation);
        Task<User> RequestPasswordReset(PasswordResetModel reset, Options options = null);
        Task<User> ResetPassword(PasswordResetModel reset, Options options = null);
        Task<T> SendConfirmation<T>(string email);
        Task<Session> SignIn<T>(Session session, Options options = null) where T : IJsonApiModel;
    }
}
