using System.Threading.Tasks;
using Xamarin.Forms.Clinical6.UI.Views;

namespace Xamarin.Forms.Clinical6
{
    /// <summary>
    /// Main service.
    /// </summary>
    public interface IMainService
    {
        /// <summary>
        /// Sets the main page.
        /// </summary>
        /// <param name="MainPage"></param>
        /// <param name="DashBoardPage"></param>
        /// <param name="processBeforeStart"></param>
        /// <returns></returns>
        //AppInitPage SetMainPage(Page MainPage, Page DashBoardPage = null, bool processBeforeStart = false);
        /// <summary>
        /// Starts the flow process.
        /// </summary>
        /// <returns>The flow process.</returns>
        /// <param name="flowProccessId">Flow proccess identifier.</param>
        Task StartFlowProcess(int flowProccessId);
        /// <summary>
        /// Completeds the flow process.
        /// </summary>
        /// <returns>The flow process.</returns>
        Task CompletedFlowProcess();
        /// <summary>
        /// Completeds the upload flow process.
        /// </summary>
        /// <returns>The upload flow process.</returns>
        Task CompletedUploadFlowProcess();
    }
}