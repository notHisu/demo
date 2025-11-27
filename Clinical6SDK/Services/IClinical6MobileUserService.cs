using System;
using System.Net.Http;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Services.Responses;
using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Helpers;
using Entry = Clinical6SDK.Models.Entry;

namespace Clinical6SDK.Services
{
    public interface IClinical6MobileUserService : IClinical6Service
    {
        Task<SignInResponse> SignInGuest(string udid, string technology);
        Task<SignInResponse> SignIn(SignInRequestMobileUser reqUser, string udid, string technology);
        Task<Response> SignOut();
        Task<SignInResponse> SignUp(string accountName, string password, string udid, string technology, Profile profile);
        Task<Response> Verify(string verificationCode);
        Task<Response> SetPin(int pin, int pinConfirmation);
        Task<Response> SetPin(string pin, string pinConfirmation);
        Task<Response> ForgotPassword(string email);
        Task<Response> ForgotAccountName(string email);
        Task<Response> ResetPassword(string oldPassword, string newPassword, string newPasswordConfirmation);
        Task<Response> Disable();
        Task<Response> Delete();

        Task<Response> AddEntry(AddEntryRequest request, User user, EntryTemplate template, int deviceId);
        Task<Response> Confirm(string token);
        Task<Response> Delete(User user);
        Task<Response> RequestPasswordReset(RequestPasswordRequest request, User user, int deviceId);
        Task<Response> ResetPassword(string token, string password, int deviceId);
        Task<Response> Register(RegisterRequest request, User user, int deviceId);
        Task<AwardedBadge> GetAwardedBadge(int awardeeId = -1, int awardedBadgeId = -1);
        Task<IEnumerable<AwardedBadge>> GetAwardedBadges(User user = null, Options options = null);
        Task<Response> GetDataGroup(int dataGroupid, User user);
        Task<T> GetDataGroup<T>(int dataGroupid = -1, int userId = -1);
        Task<IEnumerable<T>> GetEntries<T>(User user, Options options = null) where T : Entry, new();
        Task<IEnumerable<T>> GetNotifications<T>(User user, Options options = null) where T : Notification, new();
        Task<Notification> RemoveNotification(Notification notification, User user);
        Task<Notification> UpdateNotification(Notification notification, User user);
        Task<User> GetProfile(User user = null);
        Task<IEnumerable<Schedule>> GetSchedules(User user);
        Task<Response> GetSession(string token);
        Task<Response> GetRegistrationStatus(RegistrationStatusRequest request);
        Task<User> Get(User user);
        Task<Profile> UpdateProfile(Profile profile);
        Task<Schedule> UpdateSchedule(Schedule schedule, User user);
        Task<AwardedBadge> RemoveAwardedBadge(User user, AwardedBadge badge);
        Task<AwardedBadge> UpdateAwardedBadge(User user, AwardedBadge badge);
        Task<bool> SendConfirmation(string email);


        Task<Session> _LoginGetSession(IJsonApiModel response, int? deviceId = null);
        Task<User> _LoginGetUser(IJsonApiModel response, int? deviceId = null);
        Task<User> AcceptInvitation(Invitation invitation);
        Task<InvitationValidation> GetInvitationStatus(Invitation invitation, Options options = null);
        Task<T> AddAwardedBadge<T>(IParams parameters, string Json = "", Options options = null);
        Task<T> AddEntry<T>(IParams parameters, int userId = -1, string Json = "", Options options = null);
        Task<T> Confirm<T>(string token);
        Task<T> Delete<T>(IParams parameters, string Json = "", Options options = null);
        Task<T> Get<T>(Options options = null);
        Task<T> GetRegistrationStatus<T>(IParams parameters, string Json = "", Options options = null);
        Task<T> GetSession<T>(string token);
        Task<IEnumerable<Site>> GetSites(User user);
        Task<User> Insert(User user, Options options);
        Task<User> Invite(Invitation invitation);
        Task<User> Register<T>(IParams parameters, string Json = "", Options options = null) where T : IJsonApiModel;
        Task<T> RemoveAwardedBadge<T>(IParams parameters, int userId = -1, int awardeBadgeId = -1, string Json = "", Options options = null);
        Task<T> RemoveNotification<T>(IParams parameters, int userId = -1, int notificationId = -1, string Json = "", Options options = null);
        Task<User> RequestPasswordReset(PasswordResetModel reset, Options options = null);
        Task<User> ResetPassword(PasswordResetModel reset, Options options = null);
        Task<T> SendConfirmation<T>(string email);
        Task<Session> SignIn<T>(Session session, Options options = null) where T : IJsonApiModel;
        Task<User> Update(User user, Options options = null);
        Task<T> UpdateAwardedBadge<T>(IParams parameters, int userId = -1, int awardedBadgeId = -1, string Json = "", Options options = null);
        Task<T> UpdateNotification<T>(IParams parameters, int userId = -1, string Json = "", Options options = null);
        Task<T> GetRegistrationStatusWithResponse<T>(IParams parameters, string Json = "", Options options = null);

        Task<AmazonPayment> GetAmazonPayments(string userId);
        Task<bool> RedeemAmazonPayments(RedeemPayment payment);
    }

}