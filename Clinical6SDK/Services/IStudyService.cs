using System.Threading.Tasks;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;

namespace Clinical6SDK.Services
{
    public interface IStudyService : IClinical6Service
    {
        Task<SubTaskDraft> SaveAsDraft(StudySubTaskOccurrence obj);
    }
}
