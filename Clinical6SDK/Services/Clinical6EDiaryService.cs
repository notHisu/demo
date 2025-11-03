using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Clinical6SDK.Common.Exceptions;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using Newtonsoft.Json;
using static Clinical6SDK.Constants;
using Entry = Clinical6SDK.Models.Entry;

namespace Clinical6SDK
{

    /// <summary>
    /// Clinical6 eDiary service.
    /// </summary>
    public class Clinical6EDiaryService : JsonApiHttpService, IClinical6EDiaryService
    {
        /// <summary>
        /// Saves a eDiary Entry.  If the id exists (>0) then it will invoke an update, else it will invoke an insert
        /// </summary>
        /// <returns>A task with an eDiary Entry object</returns>
        /// <param name="eDiary">An eDiary Entry to be inserted or saved</param>
        /// <example>
        /// <code>
        /// var clinical6EDiaryService = new Clinical6EDiaryService();
        /// var entry = new Entry(); // no id
        /// var result = await clinical6EDiaryService.SaveAsync(entry);
        /// if(response.Success) {
        ///     entry = result.Response;
        /// }
        /// 
        /// var existingEntry = new Entry();
        /// existingEntry.id = 5;
        /// var result = await clinical6EDiaryService.UpdateEntryAsync(existingEntry);
        /// if(response.Success) {
        ///     existingEntry = result.Response;
        /// }
        /// </code>
        /// </example>
        public async Task<Entry> SaveAsync(Entry eDiary)
        {

            if (!HasToken)
            {
                throw new ArgumentException(ExceptionsMessages.TOKEN_REQUIRED);
            }

            if (eDiary.Owner.Id <= 0)
            {
                throw new ArgumentException(ExceptionsMessages.TEMPLATE_ID_REQUIRED);
            }

            if (eDiary.Id > 0)
            {
                return await UpdateEntryAsync(eDiary);
            }
            else
            {
                return await InsertEntryAsync(eDiary);
            }

        }

        /// <summary>
        /// Creates a new eDiary Entry
        /// </summary>
        /// <returns>A task with an updated Entry (with a new id)</returns>
        /// <param name="eDiaryEntry">An eDiary Entry to be inserted</param>
        /// <example>
        /// <code>
        /// var clinical6EDiaryService = new Clinical6EDiaryService();
        /// var entry = new Entry();
        /// var result = await clinical6EDiaryService.CreateEntryAsync(entry);
        /// if(response.Success) {
        ///     entry = result.Response;
        /// }
        /// </code>
        /// </example>
        public async Task<Entry> InsertEntryAsync(Entry eDiaryEntry)
        {
            if (!HasToken)
            {
                //Instead of throwing ArgumentException,
                //we'll use our custom exception here Clinical6UnauthorizedException
                //when token is invalid
                //throw new ArgumentException(ExceptionsMessages.TOKEN_REQUIRED);
                throw new Clinical6UnauthorizedException(ExceptionsMessages.TOKEN_REQUIRED);
            }
            else if (eDiaryEntry.Template.Id <= 0)
            {
                throw new ArgumentException(ExceptionsMessages.TEMPLATE_ID_REQUIRED);
            }
            else if (eDiaryEntry.Owner.Id <= 0)
            {
                throw new ArgumentException(ExceptionsMessages.MOBILE_OWNER_ID_REQUIRED);
            }


            //var date = DateTime.Now.ToString("yyyy-MM-dd");

            //var paramaters = @"{
            //        'data' : {
            //                'type' : 'ediary_entries', +
            //                'attributes' :  {'date':" + date + "}" +
            //               @"'relationships' : {
            //                    'template' : {
            //                        'data' : {
            //                            'id'   : " + eDiaryEntry.Template.Id + @",
            //                            'type' : 'ediary__entry_templates'
            //                        }
            //                    },
            //                    'owner' : {
            //                        'data' : {
            //                            'id'   : " + eDiaryEntry.Owner.Id + @",
            //                            'type' : 'mobile_users'
            //                        }
            //                    }
            //                }
            //           }
            //        }";

            //var path = ApiRoutes.EDiary.EDIARY_ENTRY;

            //var url = new Uri(string.Format("{0}{1}", BaseUrl, path));

            //var optionsUrl = FormatFormOptions(url: path);
            //optionsUrl.Add("useNewApi", "true");

            eDiaryEntry.Date = DateTime.Now;

            return await Insert<Entry>(eDiaryEntry);
        }

        /// <summary>
        /// Updates an eDiary Entry
        /// </summary>
        /// <returns>A task with an updated Entry</returns>
        /// <param name="eDiaryEntry">An eDiary Entry object to be updated.</param>
        /// <example>
        /// <code>
        /// var clinical6EDiaryService = new Clinical6EDiaryService();
        /// var entry = new Entry();
        /// entry.id = 5;
        /// var result = await clinical6EDiaryService.UpdateEntryAsync(entry);
        /// if(response.Success) {
        ///     entry = result.Response;
        /// }
        /// </code>
        /// </example>
        public async Task<Entry> UpdateEntryAsync(Entry eDiaryEntry)
        {

            var jsonString = JsonConvert.SerializeObject(eDiaryEntry);

            if (!HasToken)
            {
                throw new Clinical6UnauthorizedException(ExceptionsMessages.TOKEN_REQUIRED);
            }

            if (eDiaryEntry.Owner.Id <= 0)
            {
                throw new ArgumentException(ExceptionsMessages.TEMPLATE_ID_REQUIRED);
            }

            //var path = string.Format(ApiRoutes.EDiary.EDIARY_UPDATE_ENTRY, eDiaryEntry.Id);
            //var url = new Uri(string.Format("{0}{1}", BaseUrl, path));

            //var optionsUrl = FormatFormOptions(url: path);
            //optionsUrl.Add("useNewApi", "true");

            //var paramaters = @"{
            //        'data' : {
            //                'type' : 'ediary_entries', +
            //                'attributes' :  {'date':" + eDiaryEntry.Date.Value.ToString("yyyy-mm-dd") + "}" +
            //               @"'relationships' : {
            //                    'template' : {
            //                        'data' : {
            //                            'id'   : " + eDiaryEntry.Template.Id + @",
            //                            'type' : 'ediary__entry_templates'
            //                        }
            //                    },
            //                    'owner' : {
            //                        'data' : {
            //                            'id'   : " + eDiaryEntry.Owner.Id + @",
            //                            'type' : 'mobile_users'
            //                        }
            //                    }
            //                }
            //           }
            //        }";

            return await Update<Entry>(eDiaryEntry);
        }

        /// <summary>
        /// Gets eDiary Entry Groups.
        /// </summary>
        /// <returns>A list of eDiary Entry Groups asynchronously.</returns>
        /// <example>
        /// <code>
        /// var clinical6EDiaryService = new Clinical6EDiaryService();
        /// 
        /// var result = await clinical6EDiaryService.GetEntryGroupsAsync();
        /// if(response.Success) {
        ///     List<EntryGroup> entryGroups = result.Response.toList();
        /// }
        /// </code>
        /// </example>
        public async Task<List<EntryGroup>> GetEntryGroupsAsync()
        {
            return await Get<List<EntryGroup>>();
        }

        /// <summary>
        /// Gets the eDiary Templates.
        /// </summary>
        /// <returns>A list of eDiary Templates asynchronously.</returns>
        /// <example>
        /// <code>
        /// var clinical6EDiaryService = new Clinical6EDiaryService();
        /// 
        /// var result = await clinical6EDiaryService.GetTemplatesAsync();
        /// if(response.Success) {
        ///     List<EntryTemplate> templates = result.Response.toList();
        /// }
        /// </code>
        /// </example>
        public async Task<List<EntryTemplate>> GetTemplatesAsync(EntryGroup entryGroup)
        {
            return await GetChildren<List<EntryTemplate>>(entryGroup, new EntryTemplate().Type);
        }
    }
}
