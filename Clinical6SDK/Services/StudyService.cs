using System.Threading.Tasks;
using Clinical6SDK.Helpers;
using Clinical6SDK.Models;
using Clinical6SDK.Services.Requests;

namespace Clinical6SDK.Services
{
    public class StudyService : JsonApiHttpService, IStudyService
    {
        /// <summary>
        /// Saves the occurrence as a draft (does not change status to completed)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public async Task<SubTaskDraft> SaveAsDraft(StudySubTaskOccurrence obj)
        {
            Options options = new Options{ Url = Constants.ApiRoutes.Study.SAVE_AS_DRAFT };
            return await this.Insert<SubTaskDraft>(new SubTaskDraft { SubTaskOccurrence = obj }, options);
        }
    }
}
