namespace Xamarin.Forms.Clinical6
{
    public interface IAppSpecificConfig
    {
        //NOTE: ContactDetailsPage
        bool PrimaryContactShown { get; set; }

        //NOTE: ProfilePage
        bool ProfileFirstNameShown { get; set; }
        bool ProfileLastNameShown { get; set; }
        bool ProfileStudyStartDateShown { get; set; }

        bool InitialAccountName { get; set; }

        //NOTE: FlowProcessService
        bool ActivityHistoryeProsShown { get; set; }

        //NOTE: FlowFieldGroup
        Thickness FlowFieldGroupMargin { get; set; }

        //NOTE: Maintain session while video consult
        bool VideoConsultSessionTimeOut { get; set; }
    }

    /// <summary>
    /// Should be inherited by other apps for specific overrides and/or certain customization
    /// </summary>
    public class AppSpecificConfig : IAppSpecificConfig
    {
        public virtual bool PrimaryContactShown { get; set; } = true;
        public virtual bool ProfileFirstNameShown { get; set; } = true;
        public virtual bool ProfileLastNameShown { get; set; } = true;
        public virtual bool ProfileStudyStartDateShown { get; set; } = true;
        public virtual Thickness FlowFieldGroupMargin { get; set; } = new Thickness(0, 0, 0, -30);
        public virtual bool ActivityHistoryeProsShown { get; set; } = false;
        public virtual bool VideoConsultSessionTimeOut { get; set; } = true;
        public virtual bool InitialAccountName { get; set; } = false;
    }
}