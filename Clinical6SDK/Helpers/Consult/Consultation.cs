using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using JsonApiSerializer.JsonApi;
using Newtonsoft.Json;

namespace Clinical6SDK.Helpers
{
    public class Consultation : ConsultationModel
    {
        [JsonIgnore]
        public bool _selected;

        [JsonIgnore]
        public bool Selected
        {

            get => _selected = new List<string> { "active", "ready", "pending" }.Contains(Status);

            set { _selected = value; }
        }

        [JsonIgnore]
        private Dictionary<string, VideoStatus> _videoStatusMap = new Dictionary<string, VideoStatus>
        {
            ["active"] = VideoStatus.Active,
            ["ready"] = VideoStatus.Active,
            ["cancelled"] = VideoStatus.Cancelled,
            ["completed"] = VideoStatus.Completed,
            ["finish"] = VideoStatus.Completed,
            ["pending"] = VideoStatus.Pending,
        };

        [JsonIgnore]
        public VideoStatus VideoStatus => _videoStatusMap[Status];

        /// <summary>
        /// Deletes a consultation
        /// </summary>
        /// <returns></returns>
        public async Task<Consultation> Delete()
        {
            var service = new JsonApiHttpService();
            return await service.Delete<Consultation>(this);
        }

        /// <summary>
        /// Joins a video consultation
        /// </summary>
        /// <param name="participant"></param>
        /// <returns></returns>
        public async Task<DocumentRoot<Consultation>> Join(ConsultationParticipant participant = null)
        {
            var currentParticipant = participant ?? Participants.FirstOrDefault(p => p.User.Id == ClientSingleton.Instance.User.Id);

            // If the participant doesn't exist, create one using the logged in user
            if (currentParticipant == null)
            {
                var newParticipant = new ConsultationParticipant
                {
                    Consultation = this,
                    User = ClientSingleton.Instance.User
                };
                currentParticipant = await newParticipant.Save();
            }
            if (Participants.FirstOrDefault(p => p.Id == currentParticipant.Id) == null)
            {
                Participants.Add(currentParticipant);
            }

            var service = new ConsultationService();
            return await service.Join(this, currentParticipant);
        }

        /// <summary>
        /// Saves a video consultation (insert if id doesn't exist, update if it does)
        /// </summary>
        /// <returns></returns>
        public async Task<Consultation> Save()
        {
            var service = new JsonApiHttpService();
            return Id != null && Id > 0
                ? await service.Update<Consultation>(this)
                : await service.Insert<Consultation>(this);
        }
    }

    public enum VideoStatus
    {
        Pending = 0,
        Active = 1,
        Completed = 2,
        Cancelled = 3
    }
}
