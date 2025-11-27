using System;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Xamarin.Forms;
using Xamarin.Forms.Clinical6.Services;
using Xamarin.Forms.Clinical6.Android.Services;

[assembly: Dependency(typeof(ManagerAppVersion))]
namespace Xamarin.Forms.Clinical6.Android.Services
{
    /// <summary>
    /// 
    /// </summary>
    public class ManagerAppVersion : IManagerAppVersion
    {
        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <returns>The build number.</returns>
        public string GetBuildNumber()
        {
            Context context = Controls.Instance;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);
            return info.VersionCode.ToString();
        }

        /// <summary>
        /// Gets the OSV Version.
        /// </summary>
        /// <value>The OSV Version.</value>
        public string OSVersion
        {
            get
            {
                return Build.VERSION.Release;
            }
        }

        /// <summary>
        /// Gets the name of the app version.
        /// </summary>
        /// <value>The name of the app version.</value>
        public string AppVersionName()
        {
            Context context = Controls.Instance;
            PackageManager manager = context.PackageManager;
            PackageInfo info = manager.GetPackageInfo(context.PackageName, 0);
            return info.VersionName;
        }

        public bool OpenLink(string link)
        {
            var context = Controls.Instance; ;
            if (context == null)
            {
                System.Diagnostics.Debug.WriteLine("Context is null");
                return false;
            }

            try
            {
                var uri = global::Android.Net.Uri.Parse(link);
                Intent browserIntent = new Intent(Intent.ActionView, uri);
                browserIntent.SetFlags(ActivityFlags.ClearTop);
                browserIntent.SetFlags(ActivityFlags.NewTask);
                context.StartActivity(browserIntent);
                return true;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.StackTrace);
                return false;
            }
        }

        public string HardwareModel()
        {
            return string.Empty;
        }
    }
}
