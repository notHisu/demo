using System.Threading.Tasks;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using JsonApiSerializer.JsonApi;

namespace Clinical6SDK.Services
{
    public interface IConsultationService : IClinical6Service
    {
        Task<DocumentRoot<Consultation>> Join(Consultation consultation, ConsultationParticipant participant);
    }
}
