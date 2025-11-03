using System;
namespace Xamarin.Forms.Clinical6.Services
{
    public interface IManagerAppVersion
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <returns>The build number.</returns>
        string GetBuildNumber();

        /// <summary>
        /// Gets the OSV ersion.
        /// </summary>
        /// <value>The OSV ersion.</value>
        string OSVersion { get; }

        /// <summary>
        /// Gets the name of the app version.
        /// </summary>
        /// <value>The name of the app version.</value>
        string AppVersionName();

        /// <summary>
        /// Open a Url
        /// </summary>
        /// <param name="url"></param>
        bool OpenLink(string url);

        string HardwareModel();
    }
}
