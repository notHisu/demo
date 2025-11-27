using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

namespace Clinical6SDK.Helpers
{
    public class StudySubTaskOccurrence : StudySubTaskOccurrenceModel
    {
        public async Task<SubTaskDraft> SaveAsDraft()
        {
            var service = new StudyService();
            return await service.SaveAsDraft(this);
        }
    }
}
