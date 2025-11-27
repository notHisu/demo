using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

namespace Clinical6SDK.Helpers
{
    public class ConsultationParticipant : ConsultationParticipantModel
    {
        /// <summary>
        /// Deletes a participant
        /// </summary>
        /// <returns></returns>
        public async Task<ConsultationParticipant> Delete()
        {
            var service = new JsonApiHttpService();
            return await service.Delete<ConsultationParticipant>(this);
        }

        /// <summary>
        /// Saves a participant (insert if id doesn't exist, update if it does)
        /// </summary>
        /// <returns></returns>
        public async Task<ConsultationParticipant> Save()
        {
            var service = new JsonApiHttpService();
            return Id != null && Id > 0
                ? await service.Update<ConsultationParticipant>(this)
                : await service.Insert<ConsultationParticipant>(this);
        }
    }
}
