using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Clinical6SDK.Services;
using JsonApiSerializer;
using Newtonsoft.Json;
using Xamarin.Forms.Clinical6.Core.Models;
using Xamarin.Forms.Clinical6.Core.Models.Requests;
using Clinical6SDK.Models;
using C6Device = Clinical6SDK.Helpers.Device;
using Clinical6SDK.Helpers;

namespace Xamarin.Forms.Clinical6.Core.Services
{
    public interface IApiRequestFactory
    {
        // Flow Process Service
        Task<IParams> GetFlowProcesses(int expectedFlowProcessCount);
        Task<IParams> GetFlowProcess(IHavePermanentLink summary);
        Task<IParams> GetCompletedFlows(IHavePermanentLink flowSummary, int patientId);
        Task<IParams> GetCompletedFlowValues(IHavePermanentLink flowSummary, int patientId, int completedFlowId);
        Task<IParams> GetContainer(IHavePermanentLink flowSummary);
        Task<IParams> SaveAnswers(IHavePermanentLink flowSummary, IDictionary<int, string> answers, int patientId);
        Task<IParams> SaveAnswersWithFileUpload(FileUploadParams fileupload, Photo photo);

        // User Service
        //IParams GetAccessTokenRequest();
        C6Device GetNewDevice();
        IParams GetRegistrationStatusRequest(string email);
        Task<Invitation> CreatePinRequest(string verificationcode, string pin, string pinConfirmation);
        Task<Session> ValidateEmailAndPinRequest(string email, string pin);
        Task<Invitation> VerifyEmailRequest(string invitationToken, string email = "");
        Task<IParams> TransitionUserStatusRequest();
    }
    public class ApiRequestFactory : IApiRequestFactory
    {
        private readonly ICacheService _cacheService;
        private readonly IDeviceInfoService _deviceInfoService;

        public ApiRequestFactory(
            IDeviceInfoService deviceInfoService, 
            ICacheService cacheService
        ) //, IPushTokenStore pushTokenStore)
        {
            _cacheService = cacheService;
            _deviceInfoService = deviceInfoService;
            //_pushTokenStore = pushTokenStore;
        }

        /*
         * 
         * Flow Service
         * 
         */
        public Task<IParams> GetFlowProcesses(int expectedFlowProcessCount)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> GetFlowProcess(IHavePermanentLink summary)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> GetCompletedFlows(IHavePermanentLink flowSummary, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> GetCompletedFlowValues(IHavePermanentLink flowSummary, int patientId, int completedFlowId)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> GetContainer(IHavePermanentLink flowSummary)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> SaveAnswers(IHavePermanentLink flowSummary, IDictionary<int, string> answers, int patientId)
        {
            throw new NotImplementedException();
        }

        public Task<IParams> SaveAnswersWithFileUpload(FileUploadParams fileupload, Photo photo)
        {
            throw new NotImplementedException();
        }

        /*
         * 
         * User Service - Login
         * 
         */
         /// <summary>
         /// Gets the access token request.
         /// In Use
         /// </summary>
         /// <returns>The access token request.</returns>
        public Clinical6SDK.Helpers.Device GetNewDevice()
        {
            var tech = _deviceInfoService.DeviceTechnology == Technology.Ios ? "ios" : "android";
            var pushToken = string.Empty; // await _pushTokenStore.GetPushToken();
            //EnvironmentConfig.MobileApplicationKey,
            //_deviceInfoService.AppVersion,
            return new C6Device
            {
                MobileApplicationKey = EnvironmentConfig.MobileApplicationKey,
                Technology = tech,
                AppVersion = _deviceInfoService.AppVersion,
                PushId = pushToken
            };
        }


        /// <summary>
        /// Gets the registration status payload
        /// In Use
        /// </summary>
        /// <param name="email"></param>
        /// <returns></returns>
        public IParams GetRegistrationStatusRequest(string email)
        {
            return new RequestModel<Models.Requests.Account>
            {
                Data = new Models.Requests.DataModel<Models.Requests.Account>
                {
                    Attributes = new Models.Requests.Account
                    {
                        Email = email
                    }
                }
            };
        }
        
        public async Task<Session> ValidateEmailAndPinRequest(string email, string pin)
        {
            return new Session
            {
                Email = email,
                Password = pin,
                Device = new C6Device { Id = await _cacheService.GetDeviceId() }
            };
        }
        
        public async Task<Invitation> VerifyEmailRequest(string invitationToken, string email = "")
        {
            return new Invitation
            {
                InvitationToken = invitationToken,
                Device = new C6Device
                {
                    Id = await _cacheService.GetDeviceId()
                },
                Email = email
            };
        }
        
        
        public async Task<Invitation> CreatePinRequest(string verificatinCode, string pin, string pinConfirmation) 
        {
            return new Invitation
            {
                InvitationToken = verificatinCode,
                Password = pin,
                Device = new C6Device
                {
                    Id = await _cacheService.GetDeviceId()
                }
            };
        }
        
        public async Task<IParams> TransitionUserStatusRequest()
        {
            var userId = await _cacheService.GetUserId();
            return new Models.Requests.UserTransitionRequest
            {
                Status = new Models.Requests.UserTransitionRequestStatus
                {
                    Object = userId.Value,
                    Owner = userId.Value.ToString(),
                    OwnerType = "mobile_user",
                    Transition = "activate",
                    ObjectType = "mobile_user"
                }
            };
        }
    }
}
