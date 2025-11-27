using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using JsonApiSerializer.JsonApi;

namespace Clinical6SDK.Helpers
{
    public class User : UserModel
    {
        /// <summary>
        /// Returns a list of consultations
        /// </summary>
        /// <returns></returns>
        public async Task<List<Consultation>> GetConsultations()
        {
            var service = new JsonApiHttpService();
            return await service.GetChildren<List<Consultation>>(this, new Consultation().Type);
        }

        /// <summary>
        /// Returns a list of notifications
        /// </summary>
        /// <returns></returns>
        public async Task<List<Notification>> GetNotifications()
        {
            var service = new JsonApiHttpService();
            return await service.GetChildren<List<Notification>>(this, new Notification().Type);
        }

        /// <summary>
        /// Returns a list of notifications
        /// </summary>
        /// <returns></returns>
        public async Task<DocumentRoot<StatusModel>> GetRegistrationStatus()
        {
            var service = new Clinical6MobileUserService();
            return await service.ValidateRegistrationStatus(new RegistrationValidationModel { Email = Email });
        }

        /// <summary>
        /// Saves user (insert if id doesn't exist, update if it does)
        /// </summary>
        /// <returns></returns>
        public async Task<User> Save()
        {
            var service = new JsonApiHttpService();
            return Id != null && Id > 0
                ? await service.Update<User>(this)
                : await service.Insert<User>(this);
        }

    }
}
