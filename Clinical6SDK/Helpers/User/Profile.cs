using System;
using System.Threading.Tasks;
using Clinical6SDK.Models;
using Clinical6SDK.Services;

namespace Clinical6SDK.Helpers
{
    public class Profile : ProfileModel
    {
        public string GetFullName()
        {
            //  this.firstName + this.middleInitial + this.lastName;
            return string.Format("{0} {1} {2}", FirstName, MiddleInitial, LastName);
        }

        public string GetFullAddress()
        {
            // `${this.street} ${this.city}, ${this.state} ${this.zipCode}`;
            return string.Format("{0} {1}, {2} {3}", Street, City, State, ZipCode);
        }

        public async Task<Profile> Save()
        {
            var service = new Clinical6MobileUserService();
            return await service.UpdateProfile(this);
        }

    }
}
