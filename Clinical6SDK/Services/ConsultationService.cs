using System.Threading.Tasks;
using JsonApiSerializer.JsonApi;
using Clinical6SDK.Helpers;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
    public class ConsultationService : JsonApiDocumentRootHttpService, IConsultationService
    {
        /// <summary>
        /// Joins the consultation given a consultation and participant
        /// </summary>
        /// <param name="consultation"></param>
        /// <param name="participant"></param>
        /// <returns></returns>
        public async Task<DocumentRoot<Consultation>> Join(Consultation consultation, ConsultationParticipant participant)
        {
            Options options = new Options{ Url = Constants.ApiRoutes.Consult.JOIN_CONSULTATION };
            return await Insert<Consultation>(new ConsultationJoin { Consultation = consultation, Participant = participant }, options);
        }
    }
}
