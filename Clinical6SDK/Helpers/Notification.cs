using System;
using Newtonsoft.Json;
using Clinical6SDK.Models;
using Clinical6SDK.Services;
using System.Threading.Tasks;

namespace Clinical6SDK.Helpers
{
    public class Notification : NotificationModel
    {
        [JsonIgnore]
        public string TypeNotif
        {
            set
            {
                Type = value;
            }
            get
            {
                if (string.IsNullOrWhiteSpace(Type))
                    return "notification__deliveries";
                else
                    return Type;
            }
        }

        /// <summary>
        /// Deletes the notification
        /// </summary>
        /// <returns>The deleted notification.</returns>
        /// <param name="user">User.</param>
        public async Task<Notification> Delete(User user)
        {
            return await new Clinical6MobileUserService().RemoveNotification(this, user);
        }

        /// <summary>
        /// Saves the notification
        /// </summary>
        /// <returns>The updated notification</returns>
        /// <param name="user">User.</param>
        public async Task<Notification> Save(User user)
        {
            return await new Clinical6MobileUserService().UpdateNotification(this, user);
        }

        /// <summary>
        /// Sets the notification to the status "read".
        /// </summary>
        /// <returns>The read notificaiton.</returns>
        /// <param name="user">User.</param>
        public async Task<Notification> SetRead(User user)
        {
            Status = "read";
            return await new Clinical6MobileUserService().UpdateNotification(this, user);
        }
    }
}
