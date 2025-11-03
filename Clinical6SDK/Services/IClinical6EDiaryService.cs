using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Entry = Clinical6SDK.Models.Entry;

namespace Clinical6SDK.Services
{
    /// <summary>
    /// Clinical6 ediary service.
    /// </summary>
    public interface IClinical6EDiaryService : IClinical6Service
    {
        /// <summary>
        /// Saves the async.
        /// </summary>
        /// <returns>The async.</returns>
        /// <param name="eDiary">E diary.</param>
        Task<Entry> SaveAsync(Entry eDiary);

        /// <summary>
        /// Creates the entry async.
        /// </summary>
        /// <returns>The entry async.</returns>
        /// <param name="eDiaryEntry">E diary entry.</param>
        Task<Entry> InsertEntryAsync(Entry eDiaryEntry);

        /// <summary>
        /// Updates the entry async.
        /// </summary>
        /// <returns>The entry async.</returns>
        /// <param name="eDiaryEntry">E diary entry.</param>
        Task<Entry> UpdateEntryAsync(Entry eDiaryEntry);

        /// <summary>
        /// Gets the entry groups async.
        /// </summary>
        /// <returns>The entry groups async.</returns>
        Task<List<EntryGroup>> GetEntryGroupsAsync();

        /// <summary>
        /// Gets the templates async.
        /// </summary>
        /// <returns>The templates async.</returns>
        /// <param name="entryGroup">Entry group.</param>
        Task<List<EntryTemplate>> GetTemplatesAsync(EntryGroup entryGroup);
    }
}
