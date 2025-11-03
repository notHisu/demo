using System;
using System.Reflection;
using System.Runtime;
using System.IO;
using System.Threading.Tasks;
using System.Net.Http;
using System.Collections.Generic;

using Clinical6SDK.Services.Responses;
using Clinical6SDK.Services.Requests;
using Clinical6SDK.Models;
using Clinical6SDK.Helpers;

using JsonApiSerializer;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Linq;
using System.Text.RegularExpressions;
using JsonApiSerializer.JsonApi;
using System.Diagnostics.Contracts;
using Entry = Clinical6SDK.Models.Entry;

namespace Clinical6SDK.Services
{
    public class Clinical6MobileUserService : JsonApiHttpService, IClinical6MobileUserService
    {
        public async Task<SignInResponse> SignInGuest(string udid, string technology)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(udid)
                || string.IsNullOrWhiteSpace(technology))
                return null;


            var postData = new GuestSignInRequest
            {
                Device = new SignInRequestDevice
                {
                    Udid = udid,
                    Technology = technology
                }
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SIGN_IN_GUEST };

            return await oldService.Insert<SignInResponse>(postData, urlOptions);
        }

        public async Task<SignInResponse> SignIn(SignInRequestMobileUser reqUser, string udid, string technology)
        {
            var oldService = new GenericJsonHttpService();
            if (reqUser == null
                || string.IsNullOrWhiteSpace(reqUser.AccountName)
                || string.IsNullOrWhiteSpace(reqUser.Email)
                || string.IsNullOrWhiteSpace(reqUser.Password)
                || string.IsNullOrWhiteSpace(udid)
                || string.IsNullOrWhiteSpace(technology))
                throw new Exception("Account name , email and password are required");

            var postData = new SignInRequest
            {
                Device = new SignInRequestDevice
                {
                    Udid = udid,
                    Technology = technology
                },
                MobileUser = new SignInRequestMobileUser
                {
                    AccountName = reqUser.AccountName,
                    Email = reqUser.Email,
                    Password = reqUser.Password
                }
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SIGN_IN };

            return await oldService.Insert<SignInResponse>(postData, urlOptions);
        }



        /// <summary>
        /// Signs the out DEPRECATED
        /// </summary>
        /// <returns>The out.</returns>
        public async Task<Response> SignOut()
        {
            var response = await Send(
                Constants.ApiRoutes.MobileUsers.SIGN_OUT,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete);

            if (response.IsResponseSuccessful)
                AuthToken = null;

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Signs up. DEPRECATED
        /// </summary>
        /// <returns>The up.</returns>
        /// <param name="accountName">Account name.</param>
        /// <param name="password">Password.</param>
        /// <param name="udid">Udid.</param>
        /// <param name="technology">Technology.</param>
        /// <param name="profile">Profile.</param>
        public async Task<SignInResponse> SignUp(string accountName, string password, string udid, string technology, Profile profile)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(udid)
                || string.IsNullOrWhiteSpace(technology)
                || string.IsNullOrWhiteSpace(accountName)
                || string.IsNullOrWhiteSpace(password)
                || profile == null)
                return null;

            var postData = new SignUpRequest
            {
                Device = new SignInRequestDevice
                {
                    Udid = udid,
                    Technology = technology
                },
                MobileUser = new SignInRequestMobileUser
                {
                    AccountName = accountName,
                    Password = password
                },
                Profile = profile
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SIGN_UP };

            return await oldService.Insert<SignInResponse>(postData, urlOptions);
        }

        public async Task<Response> Verify(string verificationCode)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(verificationCode))
                return null;

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.VERIFY));

            var headers = new Dictionary<string, string> {
                { "Authorization", string.Format ("Token token={0}", AuthToken) }
            };

            var postData = new
            {
                verification_code = verificationCode
            };


            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.VERIFY };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> SetPin(int pin, int pinConfirmation)
        {
            var oldService = new GenericJsonHttpService();
            if (pin < 0 || pinConfirmation < 0)
                return null; // TODO: Throw exception

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.SET_PIN));

            var headers = new Dictionary<string, string> {
                { "Authorization", string.Format ("Token token={0}", AuthToken) }
            };

            var postData = new
            {
                pin = pin,
                pin_confirmation = pinConfirmation
            };


            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SET_PIN };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> SetPin(string pin, string pinConfirmation)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(pinConfirmation))
                return null; // TODO: Throw exception

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.SET_PIN));

            var headers = new Dictionary<string, string> {
                { "Authorization", string.Format ("Token token={0}", AuthToken) }
            };

            var postData = new
            {
                pin = pin,
                pin_confirmation = pinConfirmation
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SET_PIN };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> ForgotPassword(string email)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(email))
                return null; // TODO: Throw exception

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.FORGOT_PASSWORD));

            var postData = new
            {
                email = email
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.FORGOT_PASSWORD };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> ForgotAccountName(string email)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(email))
                return null; // TODO: Throw exception

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.FORGOT_ACCOUNT));

            var postData = new
            {
                email = email
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.FORGOT_PASSWORD };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> ResetPassword(string oldPassword, string newPassword, string newPasswordConfirmation)
        {
            if (string.IsNullOrWhiteSpace(oldPassword)
                || string.IsNullOrWhiteSpace(newPassword)
                || string.IsNullOrWhiteSpace(newPasswordConfirmation))
                return null; // TODO: Throw exception

            var postData = new
            {
                old_password = oldPassword,
                new_password = newPassword,
                new_password_confirmation = newPasswordConfirmation
            };

            var response = await Send<Response, ResponseError>(
                               Constants.ApiRoutes.MobileUsers.RESET_PASSWORD,
                               DeserializeResponse<Response>,
                               DeserializeResponseError<ResponseError>,
                               HttpMethod.Put,
                               requestData: postData);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        public async Task<Response> Disable()
        {
            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.DISABLE));

            var response = await Send<Response, ResponseError>(
                Constants.ApiRoutes.MobileUsers.DISABLE,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete);

            if (response.IsResponseSuccessful)
                AuthToken = null;

            return response.IsResponseSuccessful ? response.Data : null;
        }

        public async Task<Response> Delete()
        {
            var response = await Send<Response, ResponseError>(
                Constants.ApiRoutes.MobileUsers.MOBILE_USERS,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete);

            if (response.IsResponseSuccessful)
                AuthToken = null;

            return response.IsResponseSuccessful ? response.Data : null;
        }




        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <returns>The entry.</returns>
        /// <param name="request">Request.</param>
        /// <param name="user">User.</param>
        /// <param name="template">Template.</param>
        /// <param name="deviceId">Device identifier.</param>
        public async Task<Response> AddEntry(AddEntryRequest request, User user, EntryTemplate template, int deviceId)
        {
            var oldService = new GenericJsonHttpService();
            var path = string.Format(Constants.ApiRoutes.MobileUsers.ADDENTRY, user.Id);

            var url = new Uri(string.Format("{0}{1}", BaseUrl, path));              var relationship = new EntryRelationship             {                 template = new Requests.Relationship<EntrysRelationshipData>
                {
                    Data = new EntrysRelationshipData
                    {
                        Id = deviceId
                    }
                }             };              var postData = new DataAttributesRequest<AddEntryRequest, EntryRelationship>             {                 DataAttributes = new DataAttributes<AddEntryRequest, EntryRelationship>                 {                     Attributes = request,                     Relationships = relationship                 }             };              var urlOptions = new Options { Url = path };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        /// <summary>
        /// Confirm the specified token.
        /// </summary>
        /// <returns>The confirm.</returns>
        /// <param name="token">Token.</param>
        public async Task<Response> Confirm(string token)
        {
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("token must be initialized with a valid value.");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.CONFIRMATION, token);

            var response = await Send(
               path,
               DeserializeResponse<Response>,
               DeserializeResponseError<ResponseError>);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        public async Task<Response> Delete(User user)
        {
            var path = string.Format(Constants.ApiRoutes.MobileUsers.DELETE, user.Id);

            var response = await base.Send<Response, ResponseError>(
                path,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete);

            if (response.IsResponseSuccessful)
                AuthToken = null;

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Requests the password reset.
        /// </summary>
        /// <returns>The password reset.</returns>
        /// <param name="request">Request.</param>
        /// <param name="user">User.</param>
        /// <param name="deviceId">Device identifier.</param>
        public async Task<Response> RequestPasswordReset(RequestPasswordRequest request, User user, int deviceId)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(request.Account_name) || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Request must contain a valid value.");
            }

            var postData = new DataAttributesRequest<RequestPasswordRequest, RequestPasswordRelationship>
            {
                DataAttributes = new DataAttributes<RequestPasswordRequest, RequestPasswordRelationship>
                {
                    Attributes = request
                }
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.PASSWORD };

            return await oldService.Insert<Response>(postData, urlOptions);
        }

        public async Task<Response> ResetPassword(string token, string password, int deviceId)
        {
            if (string.IsNullOrWhiteSpace(token)
                || string.IsNullOrWhiteSpace(password))
                throw new ArgumentException("Request must contain a valid value.");

            var resetPasswordRequest = new ResetPasswordRequest()
            {
                Reset_Password_Token = token,
                Password = password
            };


            var relationship = new ResetPasswordRelationship
            {
                devices = new Requests.Relationship<ResetPasswordRelationshipData>
                {
                    Data = new ResetPasswordRelationshipData
                    {
                        Id = deviceId
                    }
                }
            };

            var postData = new DataAttributesRequest<ResetPasswordRequest, ResetPasswordRelationship>
            {
                DataAttributes = new DataAttributes<ResetPasswordRequest, ResetPasswordRelationship>
                {
                    Attributes = resetPasswordRequest,
                    Relationships = relationship
                }
            };

            var response = await Send(
                               Constants.ApiRoutes.MobileUsers.PASSWORD,
                               DeserializeResponse<Response>,
                               DeserializeResponseError<ResponseError>,
                               new HttpMethod("PATCH"),
                               requestData: postData);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Register the specified request, user and deviceId.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="request">Request.</param>
        /// <param name="user">User.</param>
        /// <param name="deviceId">Device identifier.</param>
        public async Task<Response> Register(RegisterRequest request, User user, int deviceId)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(request.Guest) || string.IsNullOrWhiteSpace(request.Account_name) || string.IsNullOrWhiteSpace(request.Email))
            {
                throw new ArgumentException("Request must contain a valid value.");
            }

            if (string.IsNullOrWhiteSpace(request.Password))
            {
                throw new ArgumentException("Request must contain a valid Password value.");
            }

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.REGISTER));

            var relationship = new RegisterRelationship
            {
                devices = new Requests.Relationship<RegisterRelationshipData>
                {
                    Data = new RegisterRelationshipData
                    {
                        Id = deviceId
                    }
                }
            };

            var postData = new DataAttributesRequest<RegisterRequest, RegisterRelationship>
            {
                DataAttributes = new DataAttributes<RegisterRequest, RegisterRelationship>
                {
                    Attributes = request,
                    Relationships = relationship
                }
            };


            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.REGISTER };

            return await oldService.Insert<SignInResponse>(postData, urlOptions);
        }

        /// <summary>
        /// Get a single awarded badge.
        /// </summary>
        /// <returns>The awarded badges.</returns>
        /// <param name="awardeeId">Awardee identifier.</param>
        /// <param name="awardedBadgeId">Awarded badge identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<AwardedBadge> GetAwardedBadge(int awardeeId = -1, int awardedBadgeId = -1)
        {
            if (string.IsNullOrWhiteSpace(AuthToken))
                throw new ArgumentException("Token must be valid.");

            if (awardeeId <= 0)
            {
                throw new ArgumentException("Awardee ID must be vallid ID.");
            }
            if (awardedBadgeId <= 0)
            {
                throw new ArgumentException("AwardedBadge ID must be vallid ID.");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGES, awardeeId);
            string groupid = (awardedBadgeId > 0 ? "/" + awardedBadgeId.ToString() : "");
            var url = string.Format($"{path}{groupid}");

            var response = await Send(
                url,
                DeserializeResponse<AwardedBadge>,
                DeserializeResponseError<ResponseError>
            );

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Gets User badges.
        /// </summary>
        /// <returns>The awarded badges.</returns>
        /// <param name="user">User. The user must have id</param>
        public async Task<IEnumerable<AwardedBadge>> GetAwardedBadges(User user = null, Options options = null)
        {
            return await GetChildren<List<AwardedBadge>>(user ?? ClientSingleton.Instance.User, new AwardedBadge().Type, options);
        }

        /// <summary>
        /// Get Flow Data Group for a user based on an id.
        /// </summary>
        /// <returns>The data group.</returns>
        /// <param name="dataGroupid">Data group id. Parameters used to get information from server (such as id)</param>
        /// <param name="user">User. The user must to have id</param>
        public async Task<Response> GetDataGroup(int dataGroupid, User user = null)
        {
            user = user ?? ClientSingleton.Instance.User;
            if (user == null || user.Id <= 0 || dataGroupid <= 0)
                throw new ArgumentException("User id and DataGroup id are required");

            var path = string.Format(Constants.ApiRoutes.MobileUsers.DATAGROUPS, user.Id, dataGroupid);

            var response = await Send(
                path,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Gets the data group.
        /// </summary>
        /// <returns>The data group.</returns>
        /// <param name="dataGroupid">Data groupid.</param>
        /// <param name="userId">User identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> GetDataGroup<T>(int dataGroupid = -1, int userId = -1)
        {
            if (userId <= 0 || dataGroupid <= 0)
            {
                throw new ArgumentException("User id and DataGroup id are required");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.DATAGROUPS, userId, dataGroupid);

            return await Send<T>(path);
        }

        /// <summary>
        /// Gets the entries.
        /// </summary>
        /// <returns>The entries.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<T>> GetEntries<T>(User user, Options options = null) where T : Entry, new()
        {
            if (user.Id <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            return await GetChildren<List<T>>(user, new T().Type, options);
        }

        /// <summary>
        /// Gets the notifications.
        /// </summary>
        /// <returns>The notifications.</returns>
        /// <param name="userId">User identifier.</param>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<T>> GetNotifications<T>(User user, Options options = null) where T : Notification, new()
        {
            if (user.Id <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            return await GetChildren<List<T>>(user, new T().Type, options);
        }

        /// <summary>
        /// Remove Notification
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="notification">Notification.</param>
        /// <param name="user">User.</param>
        public async Task<Notification> RemoveNotification(Notification notification, User user)
        {
            if (notification == null || notification.Id <= 0)
            {
                throw new ArgumentException("Notification must have id");
            }
            if (user == null || user.Id <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.REMOVENOTIFICATION, user.Id, notification.Id);

            var response = await Send(
                path,
                DeserializeResponse<Notification>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Update Notification
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="notification">Notification.</param>
        /// <param name="user">User.</param>
        public async Task<Notification> UpdateNotification(Notification notification, User user)
        {
            if (notification == null || notification.Id <= 0)
            {
                throw new ArgumentException("Notification is required");
            }
            if (user == null || user.Id <= 0)
            {
                throw new ArgumentException("User must have id");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.UPDATENOTIFICATION, user.Id, notification.Id);

            var response = await Send(
                path,
                DeserializeResponse<Notification>,
                DeserializeResponseError<ResponseError>,
                new HttpMethod("PATCH"),
                null,
                JsonConvert.SerializeObject(notification, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore })
            );

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Get the profile of a user
        /// </summary>
        /// <returns>The user profile.</returns>
        /// <param name="user">User must have id</param>
        public async Task<User> GetProfile(User user = null)
        {
            var _client = ClientSingleton.Instance;
            user = (user == null) ? _client.User : user;
            if (user == null || user.Id <= 0)
                throw new ArgumentException("User must have id");

            var profile = await GetChildren<Profile>(user, "profile");
            user.Profile = profile ?? user.Profile;
            return user;
        }


        /// <summary>
        /// Get Schedules for a user based on an id.
        /// </summary>
        /// <returns>returns a schedule or list of schedules</returns>
        /// <param name="user">User. User ID must exist</param>
        public async Task<IEnumerable<Schedule>> GetSchedules(User user)
        {
            if (user != null && user.Id <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            //var path = string.Format(Constants.ApiRoutes.MobileUsers.SCHEDULES, user.Id);

            //var response = await Send(
            //    path,
            //    DeserializeResponse<List<Schedule>>,
            //    DeserializeResponseError<ResponseError>);

            //return response.IsResponseSuccessful ? response.Data : null;

            return await GetChildren<List<Schedule>>(user, new Schedule().Type);
        }

        /// <summary>
        /// Get the session of the logged in user.
        /// </summary>
        /// <returns>object that returns success or failure</returns>
        /// <param name="token">Access token</param>
        public async Task<Response> GetSession(string token)
        {
            string httpQuery = string.Empty;

            if (token.Length > 0)
            {
                httpQuery = "?access_token=" + token;
            }

            var path = Constants.ApiRoutes.MobileUsers.SESSIONS + httpQuery;

            var response = await Send(
                path,
                DeserializeResponse<Response>,
                DeserializeResponseError<ResponseError>);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Gets the registration status of the user.
        /// </summary>
        /// <returns>The registration status.</returns>
        /// <param name="request">Request data for Registration status like Account name or email.</param>
        public async Task<Response> GetRegistrationStatus(RegistrationStatusRequest request)
        {
            var oldService = new GenericJsonHttpService();
            if (request == null ||
                (string.IsNullOrWhiteSpace(request.AccountName) && string.IsNullOrWhiteSpace(request.Email)))
            {
                throw new ArgumentException("Account name or email are required.");
            }

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.REGISTRATIONSTATUS };

            return await oldService.Insert<Response>(request, urlOptions);
        }

        /// <summary>
        /// Call a GET request.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="user">User. user id must exits</param>
        public async Task<User> Get(User user)
        {
            if (user != null && user.Id <= 0)
                throw new ArgumentException("User must have id");

            return await this.GetProfile(user);
        }

        /// <summary>
        /// Updates the profile.
        /// </summary>
        /// <returns>The profile.</returns>
        /// <param name="user">User.</param>
        /// <param name="profile">Profile.</param>
        public async Task<Profile> UpdateProfile(Profile profile)
        {
            if (profile.User == null || profile.User.Id <= 0)
                throw new ArgumentException("User id is required");

            var options = new Options { Url = string.Format(Constants.ApiRoutes.MobileUsers.PROFILE, profile.User.Id) };

            return await Update<Profile>(profile, options);
        }

        /// <summary>
        /// Updates the schedule.
        /// </summary>
        /// <returns>The schedule.</returns>
        /// <param name="schedule">Schedule to uodate.</param>
        /// <param name="user">User. The user must have id</param>
        public async Task<Schedule> UpdateSchedule(Schedule schedule, User user)
        {
            if (schedule == null || schedule.Id <= 0 || user == null || user.Id <= 0)
                throw new ArgumentException("Schedule id and user id to update are required");

            var path = string.Format(Constants.ApiRoutes.MobileUsers.SCHEDULES, user.Id);

            var url = string.Format("{0}{1}", path, "/" + schedule.Id.ToString());

            var response = await Send<Schedule, ResponseError>(
                url,
                DeserializeResponse<Schedule>,
                DeserializeResponseError<ResponseError>,
                new HttpMethod("PATCH"),
                requestData: schedule);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Removes the awarded badge.
        /// </summary>
        /// <returns>The awarded badge.</returns>
        /// <param name="user">User.</param>
        /// <param name="badge">Badge.</param>
        public async Task<AwardedBadge> RemoveAwardedBadge(User user, AwardedBadge badge)
        {
            if (user == null || user.Id <= 0 || badge == null || badge.Id <= 0)
                throw new ArgumentException("User id and Badge id are required");

            var path = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGES, user.Id);

            var url = string.Format("{0}{1}", path, "/" + badge.Id.ToString());

            var response = await Send<AwardedBadge, ResponseError>(
                url,
                DeserializeResponse<AwardedBadge>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Delete,
                requestData: badge);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        /// <summary>
        /// Updates the awarded badge.
        /// </summary>
        /// <returns>The awarded badge.</returns>
        /// <param name="user">User.</param>
        /// <param name="badge">Badge.</param>
        public async Task<AwardedBadge> UpdateAwardedBadge(User user, AwardedBadge badge)
        {
            if (user == null || user.Id <= 0 || badge == null || badge.Id <= 0)
                throw new ArgumentException("User id and Badge id are required");


            var path = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGES, user.Id);

            var url = string.Format("{0}{1}", path, "/" + badge.Id.ToString());

            var response = await Send(
                url,
                DeserializeResponse<AwardedBadge>,
                DeserializeResponseError<ResponseError>,
                new HttpMethod("PATCH"),
                requestData: badge);

            return response.IsResponseSuccessful ? response.Data : null;
        }

        public async Task<bool> SendConfirmation(string email)
        {
            var oldService = new GenericJsonHttpService();

            if (string.IsNullOrWhiteSpace(email))
            {
                throw new ArgumentException("email ir required");
            }
            // verify if is valid email
            var data = new SendConfirmationRequest
            {
                Data = new SendConfirmationRequestData
                {
                    Attributes = email
                }
            };

            var urlOptions = new Options { Url = Constants.ApiRoutes.MobileUsers.SENDCONFIRMATION };

            try
            {
                await oldService.Insert<Response>(data, urlOptions);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        /// <summary>
        /// Logins the get session.
        /// </summary>
        /// <returns>The get session.</returns>
        /// <param name="response">Response.</param>
        /// <param name="device">Device.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<Session> _LoginGetSession(IJsonApiModel response, int? deviceId = null)
        {
            var session = new Session();
            await Task.Run(() =>
            {
                if (response != null)
                {
                    if (response.Type == "mobile_users")
                    {
                        session.User = (User)response;
                        ClientSingleton.Instance.Device = session.User.Devices.Find(d => d.Id != null && d.Id == deviceId);
                        if (ClientSingleton.Instance.Device != null)
                        {
                            session.AuthenticationToken = ClientSingleton.Instance.Device.AccessToken;
                        }
                    }
                    else if (response.Type == "user_sessions")
                    {
                        session = (Session)response;
                    }
                    // v4
                    else if (response.Type == "sessions")
                    {
                        session = (Session)response;
                        session.AuthenticationToken = session.AccessToken;  // for now
                        ClientSingleton.Instance.AuthToken = session.AccessToken;
                    }
                }
            });
            ClientSingleton.Instance.AuthToken = session.AuthenticationToken;
            ClientSingleton.Instance.User = session.User;
            return session;
        }

        /// <summary>
        /// Logins the get user.
        /// </summary>
        /// <returns>The get user.</returns>
        /// <param name="response">Response.</param>
        /// <param name="device">Device.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> _LoginGetUser(IJsonApiModel response, int? deviceId = null)
        {
            User user = new User();

            await Task.Run(() =>
            {
                if (response != null)
                {
                    if (response.Type == "mobile_users")
                    {
                        user = (User)response;
                        ClientSingleton.Instance.Device = user.Devices.Find(d => d.Id != null && d.Id == deviceId);
                        if (ClientSingleton.Instance.Device != null)
                        {
                            ClientSingleton.Instance.AuthToken = ClientSingleton.Instance.Device.AccessToken;
                        }

                    }
                    else if (response.Type == "user_sessions")
                    {
                        user = ((Session)response).User;
                        ClientSingleton.Instance.AuthToken = ((Session)response).AuthenticationToken;
                    }
                }
            });
            ClientSingleton.Instance.User = user;
            return user;
        }

        /// <summary>
        /// Accepts the invitation.
        /// </summary>
        /// <returns>The invitation.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> AcceptInvitation(Invitation invitation)
        {
            if (string.IsNullOrWhiteSpace(invitation.InvitationToken))
            {
                throw new ArgumentException("Token must be a valid value");
            }
            if (string.IsNullOrWhiteSpace(invitation.Email))
            {
                throw new ArgumentException("Email is required or email is not a valid email");
            }

            var options = new Options { Url = Constants.ApiRoutes.MobileUsers.INVITATION };

            var response = await Update<Invitation>(invitation, options);

            return await _LoginGetUser(response);
        }

        public async Task<InvitationValidation> GetInvitationStatus(Invitation invitation, Options options = null)
        {
            if (string.IsNullOrWhiteSpace(invitation.InvitationToken))
            {
                throw new ArgumentException("Token must be a valid value");
            }
            if (!(invitation.Email != null))
            {
                throw new ArgumentException("Email is required or email is not a valid email");
            }
            if (!(invitation.Device != null && invitation.Device.Id != null))
            {
                throw new ArgumentException("Device is required");
            }

            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.VALIDATEINVITATION;

            return await Insert<InvitationValidation>(invitation, options);
        }

        /// <summary>
        /// Adds the awarded badge.
        /// </summary>
        /// <returns>The awarded badge.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> AddAwardedBadge<T>(IParams parameters, string Json = "", Options options = null)
        {
            var oldService = new GenericJsonHttpService();
            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            int awardeeID = -1;
            if (_parameters.ContainsKey("awardeeid"))
            {
                try
                {
                    awardeeID = (int)_parameters["awardeeid"];
                }
                catch
                {
                    awardeeID = -1;
                }
            }
            if (awardeeID <= 0)
            {
                throw new ArgumentException("Awardee id is required");
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGES, awardeeID);

            return await oldService.Insert<T>(json, options);
        }

        /// <summary>
        /// Adds the entry.
        /// </summary>
        /// <returns>The entry.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> AddEntry<T>(IParams parameters, int userId = -1, string Json = "", Options options = null)
        {
            var oldService = new GenericJsonHttpService();
            if (userId <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.ADDENTRY, userId);

            return await oldService.Insert<T>(json, options);
        }

        /// <summary>
        /// Confirm the specified token.
        /// </summary>
        /// <returns>The confirm.</returns>
        /// <param name="token">Token.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> Confirm<T>(string token)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(token))
            {
                throw new ArgumentException("token must be initialized with a valid value.");
            }

            var path = string.Format(Constants.ApiRoutes.MobileUsers.CONFIRMATION, token);
            var _options = new Options { Url = path };

            return await oldService.Get<T>(_options);
        }

        /// <summary>
        /// Delete the specified parameters, Json and options.
        /// </summary>
        /// <returns>The delete.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> Delete<T>(IParams parameters, string Json = "", Options options = null)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(AuthToken))
            {
                throw new ArgumentException("AuthToken must be a valid value.");
            }

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return await oldService.Delete<T>(_parameters, options);
        }

        /// <summary>
        /// Get the specified parameters and options.
        /// </summary>
        /// <returns>The get.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> Get<T>(Options options = null)
        {
            var oldService = new GenericJsonHttpService();
            if (string.IsNullOrWhiteSpace(AuthToken))
                throw new ArgumentException("Token must be valid.");

            return await oldService.Get<T>(options);
        }

        /// <summary>
        /// Gets the registration status.
        /// </summary>
        /// <returns>The registration status.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        //public async Task<Response<T, TError>> GetRegistrationStatus<T, TError>(IParams parameters, string Json = "", Options options = null)
        public async Task<T> GetRegistrationStatus<T>(IParams parameters, string Json = "", Options options = null)
        {
            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }

            if (!((_parameters.ContainsKey("account_name") && _parameters["account_name"] != null) ||
                    (_parameters.ContainsKey("email") && _parameters["email"] != null && IsValidEmail(_parameters["email"].ToString()))
            ))
            {
                throw new ArgumentException("account_name or valid email are required.");
            }

            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.REGISTRATIONSTATUS;

            //var response = await Send<T>(options.Url, requestData: json, httpMethod: HttpMethod.Post);

            var response = await Send(
                options.Url,
                DeserializeResponse<T>,
                DeserializeResponseError<ResponseError>,
                HttpMethod.Post,
                requestData: json
            );
            return response.IsResponseSuccessful ? response.Data : default(T);
        }

        /// <summary>
        /// Gets the session.
        /// </summary>
        /// <returns>The session.</returns>
        /// <param name="token">Token.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> GetSession<T>(string token)
        {
            string httpQuery = (token.Length > 0) ? "?access_token=" + token : string.Empty;

            var options = new Options { Url = Constants.ApiRoutes.MobileUsers.SESSIONS + httpQuery };

            return await base.Get<T>(options);
        }

        /// <summary>
        /// Gets the sites.
        /// </summary>
        /// <returns>The sites.</returns>
        /// <param name="userId">User identifier.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<IEnumerable<Site>> GetSites(User user)
        {
            if (user.Id <= 0)
            {
                throw new ArgumentException("User id is required");
            }

            return await GetChildren<List<Site>>(user, new Site().Type);
        }

        /// <summary>
        /// Insert the specified user and options.
        /// </summary>
        /// <returns>The insert.</returns>
        /// <param name="user">User.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> Insert(User user, Options options)
        {
            if (user == null)
            {
                throw new ArgumentException("User is required");
            }

            return await Insert<User>(user, options);
        }

        /// <summary>
        /// Invite the specified parameters, userId, Json and options.
        /// </summary>
        /// <returns>The invite.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> Invite(Invitation invitation)
        {
            if (string.IsNullOrEmpty(invitation.Email))
                throw new ArgumentException("Email is required");

            var options = new Options { Url = Constants.ApiRoutes.MobileUsers.INVITATION };
            return await Insert<User>(invitation, options);
        }

        /// <summary>
        /// Register the specified parameters, deviceId, Json and options.
        /// </summary>
        /// <returns>The register.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="deviceId">Device identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> Register<T>(IParams parameters, string Json = "", Options options = null) where T : IJsonApiModel
        {
            var oldService = new GenericJsonHttpService();
            AreParamsValid(new List<string>()
            {
                "deviceid",
                "guest",
                "account_name",
                "email",
                "password"
            }, parameters, Json);

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            int deviceId = 0;
            try
            {
                deviceId = (int)_parameters["deviceid"];
            }
            catch (Exception ex)
            {
                deviceId = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.REGISTER;

            string device = @"{
                'data':{
                    'type':'mobile_users',
                    'attributes':{
                        'guest':'" + _parameters["guest"].ToString() + @"',
                        'account_name':'" + _parameters["account_name"].ToString() + @"',
                        'email':'" + _parameters["email"].ToString() + @"',
                        'password':'" + _parameters["password"] + @"'
                    },
                    'relationships':{
                        'devices':{
                            'data':{
                                'id':" + deviceId.ToString() + @",
                                'type':'devices'
                            }
                        }
                    }
                }
            }";

            try
            {
                IJsonApiModel response = await oldService.Insert<T>(device, options);
                return await _LoginGetUser(response, deviceId);
            }
            catch
            {
                return default(User);
            }
        }

        /// <summary>
        /// Removes the awarded badge.
        /// </summary>
        /// <returns>The awarded badge.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="awardeBadgeId">Awarde badge identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> RemoveAwardedBadge<T>(IParams parameters, int userId = -1, int awardeBadgeId = -1, string Json = "", Options options = null)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User id is required");
            }
            if (awardeBadgeId <= 0)
            {
                throw new ArgumentException("Awarde Badge id is required");
            }

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGESID, userId, awardeBadgeId);

            return await base.Delete<T>(json, options);
        }

        /// <summary>
        /// Removes the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="notificationId">Notification identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> RemoveNotification<T>(IParams parameters, int userId = -1, int notificationId = -1, string Json = "", Options options = null)
        {
            if (userId <= 0)
            {
                throw new ArgumentException("User id is required");
            }
            if (notificationId <= 0)
            {
                throw new ArgumentException("Notification id is required");
            }

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }


            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.REMOVENOTIFICATION, userId, notificationId);

            return await base.Delete<T>(json, options);
        }

        /// <summary>
        /// Requests the password reset.
        /// </summary>
        /// <returns>The password reset.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> RequestPasswordReset(PasswordResetModel reset, Options options = null)
        {
            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.PASSWORD;
            return await Insert<User>(reset, options);
        }

        /// <summary>
        /// Resets the password.
        /// </summary>
        /// <returns>The password.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> ResetPassword(PasswordResetModel reset, Options options = null)
        {
            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.PASSWORD;
            return await Update<User>(reset, options);
        }

        /// <summary>
        /// Sends the confirmation.
        /// </summary>
        /// <returns>The confirmation.</returns>
        /// <param name="email">Email.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> SendConfirmation<T>(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("Email is required");

            return await Send<T>(
                Constants.ApiRoutes.MobileUsers.SENDCONFIRMATION,
                httpMethod: HttpMethod.Post,
                requestData: "{'data':{'type':'confirmations','attributes'{" + email + "}}}"
            );
        }

        /// <summary>
        /// Signs in the user given a session
        /// </summary>
        /// <typeparam name="T">Required for expected response</typeparam>
        /// <param name="session"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public async Task<Session> SignIn<T>(Session session, Options options = null) where T : IJsonApiModel
        {
            if (!(session.Email != null && IsValidEmail(session.Email.ToString())))
            {
                if (!(session.AccountName != null))
                {
                    throw new Exception("email is required or is not valid");
                }
            }
            else if (!(session.Password != null))
            {
                throw new Exception("password is required");
            }

            options = options ?? new Options();
            options.Url = Constants.ApiRoutes.MobileUsers.SESSIONS;

            var result = await base.Insert<T>(session, options);

            return await _LoginGetSession(result, session.Device?.Id);
        }


        /// <summary>
        /// Update the specified user and options.
        /// </summary>
        /// <returns>The update.</returns>
        /// <param name="user">User.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<User> Update(User user, Options options = null)
        {
            if (user != null && user.Id <= 0)
                throw new ArgumentException("User is required");

            return await base.Update<User>(user, options);
        }

        /// <summary>
        /// Updates the awarded badge.
        /// </summary>
        /// <returns>The awarded badge.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="awardedBadgeId">Awarded badge identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> UpdateAwardedBadge<T>(IParams parameters, int userId = -1, int awardedBadgeId = -1, string Json = "", Options options = null)
        {
            if (awardedBadgeId <= 0)
            {
                throw new ArgumentException("awardedBadgeId is required");
            }

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.AWARDEDBADGESID, userId, awardedBadgeId);

            return await base.Update<T>(json, options);
        }

        /// <summary>
        /// Updates the notification.
        /// </summary>
        /// <returns>The notification.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> UpdateNotification<T>(IParams parameters, int userId = -1, string Json = "", Options options = null)
        {
            AreParamsValid(new List<string> { "id", "notificationid" }, parameters, Json);

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }
            int notid = -1;
            try
            {
                notid = (int)_parameters["notificationid"];
            }
            catch (Exception ex)
            {
                notid = 0;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }
            if (notid <= 0)
            {
                throw new ArgumentException("Notification mst be a valid id");
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.REMOVENOTIFICATION, userId, notid);

            return await base.Update<T>(json, options);
        }

        /// <summary>
        /// Calls a POST to return a status based on either the email or the account_name of the user
        /// </summary>
        /// <param name="registrationValidation"></param>
        /// <returns></returns>
        public async Task<DocumentRoot<StatusModel>> ValidateRegistrationStatus(RegistrationValidationModel registrationValidation = null)
        {
            var rv = registrationValidation ?? new RegistrationValidationModel
            {
                Email = ClientSingleton.Instance.User?.Email
            };
            if (rv.Email == null && rv.AccountName == null)
            {
                throw new ArgumentException("account_name or valid email are required.");
            }

            var service = new JsonApiDocumentRootHttpService();
            Options options = new Options { Url = Constants.ApiRoutes.MobileUsers.REGISTRATIONSTATUS };
            return await service.Insert<StatusModel>(rv, options);
        }

        /// <summary>
        /// Retreive amazon payments for current user
        /// </summary>
        /// <returns></returns>
        public async Task<AmazonPayment> GetAmazonPayments(string userId)
        {
            Contract.Requires(!string.IsNullOrEmpty(userId), "Unable to retrieve payment. User is not set.");

            var options = new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.AMAZONPAYMENTACCOUNT, userId);
            return await base.Get<AmazonPayment>(options);
        }

        /// <summary>
        /// Redeem amazon payments for current user
        /// </summary>
        /// <param name="payment"></param>
        /// <returns></returns>
        public async Task<bool> RedeemAmazonPayments(RedeemPayment payment)
        {
            Contract.Requires(payment != null, "Invalid payment. Redeem payment is null");
            Contract.Requires(!string.IsNullOrEmpty(payment.UserId), "Unable to process payment. User for payment is not set.");

            var options = new Options();
            var json = JsonConvert.SerializeObject(payment, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.AMAZONPAYMENTREDEEM, payment.UserId);

            var result = await base.Insert<RedeemPayment>(json, options);
            return result == null;
        }

        /****************** TODO: BELOW THIS NEEDS TO BE REMOVED OR REFACTORED **********************/


        public async Task<T> GetRegistrationStatusWithResponse<T>(IParams parameters, string Json = "", Options options = null)
        {
            /*
            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }

            if(!( (_parameters.ContainsKey("account_name") && _parameters["account_name"] != null) ||
                    (_parameters.ContainsKey("email") && _parameters["email"] !=null && IsValidEmail(_parameters["email"].ToString()))
            ))
            {
                throw new ArgumentException("account_name or valid email are required.");
            }

            IDictionary<string, string> _options = FormOptions(options);

            var url = new Uri(string.Format("{0}{1}", BaseUrl, Constants.ApiRoutes.MobileUsers.REGISTRATIONSTATUS));

            var response = await Get(url, requestData: json);

            

            System.Diagnostics.Debug.WriteLine("Url:" + url);

            if (response.IsSuccessStatusCode)
            {
                var content = response.Content == null ? "" : await response.Content.ReadAsStringAsync();
                
                System.Diagnostics.Debug.WriteLine("resp json:" + content);
                
                return JsonConvert.DeserializeObject<T>(content, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            return JsonConvert.DeserializeObject<T>(string.Empty);
            */

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }

            if (!((_parameters.ContainsKey("account_name") && _parameters["account_name"] != null) ||
                    (_parameters.ContainsKey("email") && _parameters["email"] != null && IsValidEmail(_parameters["email"].ToString()))
            ))
            {
                throw new ArgumentException("account_name or valid email are required.");
            }

            options = options ?? new Options();
            options.Url = string.Format("{0}", Constants.ApiRoutes.MobileUsers.REGISTRATIONSTATUS);

            return await Insert<T>(json, options);
        }

        /// <summary>
        /// Updates the schedule.
        /// </summary>
        /// <returns>The schedule.</returns>
        /// <param name="parameters">Parameters.</param>
        /// <param name="userId">User identifier.</param>
        /// <param name="Json">Json.</param>
        /// <param name="options">Options.</param>
        /// <typeparam name="T">The 1st type parameter.</typeparam>
        public async Task<T> UpdateSchedule<T>(IParams parameters, int userId = -1, string Json = "", Options options = null)
        {
            AreParamsValid(new List<string> { "id", "scheduleid" }, parameters, Json);

            string json = string.Empty;
            IDictionary<string, object> _parameters = null;
            if (parameters != null)
            {
                json = JsonConvert.SerializeObject(parameters, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
                _parameters = ConvertToDictionary(JObject.Parse(json).Descendants());
            }
            else
            {
                json = Json;
                _parameters = JsonConvert.DeserializeObject<IDictionary<string, object>>(Json, new JsonApiSerializerSettings { NullValueHandling = NullValueHandling.Ignore });
            }

            int userid = -1;
            try
            {
                userid = (int)_parameters["id"];
            }
            catch (Exception ex)
            {
                userid = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            int scheduleid = -1;
            try
            {
                scheduleid = (int)_parameters["scheduleid"];
            }
            catch (Exception ex)
            {
                scheduleid = -1;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            options = options ?? new Options();
            options.Url = string.Format(Constants.ApiRoutes.MobileUsers.SCHEDULESID, userId, scheduleid);

            return await base.Update<T>(json, options);
        }

        bool IsValidEmail(string email)
        {
            const string pattern = @"^(?!\.)(""([^""\r\\]|\\[""\r\\])*""|" + @"([-a-z0-9!#$%&'*+/=?^_`{|}~]|(?<!\.)\.)*)(?<!\.)" + @"@[a-z0-9][\w\.-]*[a-z0-9]\.[a-z][a-z\.]*[a-z]$";

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);

            return regex.IsMatch(email);
        }
    }
}

