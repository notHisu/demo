
using System.Runtime.CompilerServices;

[assembly: InternalsVisibleTo("Clinical6SDK.Tests")]
namespace Clinical6SDK
{
	internal static class Constants
	{
		internal static class ApiRoutes
		{
			// Captive Reach API Documentation: 
			// https://{ACCOUNT-NAME}.captivereach.com/api-doc/1.0.html

			internal static class MobileUsers
			{
				public const string MOBILE_USERS = "/api/mobile_users";
				public const string SIGN_IN_GUEST = "/api/mobile_users/sign_in_guest";
				public const string SIGN_UP = "/api/mobile_users/sign_up";
				public const string SIGN_IN = "/api/mobile_users/sign_in";
				public const string SIGN_OUT = "/api/mobile_users/sign_out";
				public const string FORGOT_PASSWORD = "/api/mobile_users/forgot_password";
				public const string FORGOT_ACCOUNT = "/api/mobile_users/forgot_account_name";
				public const string RESET_PASSWORD = "/api/mobile_users/reset_password";
				public const string DISABLE = "/api/mobile_users/disable";
				public const string VERIFY = "/api/mobile_users/verify";
				public const string SET_PIN = "/api/mobile_users/set_pin";



                public const string INVITATION = "/v3/mobile_users/invitation";
                //public const string VALIDATEINVITATION = "/v3/mobile_users/invitation/validate";
                public const string VALIDATEINVITATION = "/v3/mobile_users/invitation/status";
                public const string AWARDEDBADGES = "/v3/mobile_users/{0}/awarded_badges";
                public const string AWARDEDBADGESID = "/v3/mobile_users/{0}/awarded_badges/{1}";


                public const string ADDENTRY = "/v3/mobile_users/{0}/ediary/entries";
                public const string SENDCONFIRMATION = "/v3/mobile_users/confirmation";
                public const string CONFIRMATION = "/v3/mobile_users/confirmation?confirmation_token={0}";
                public const string DATAGROUPS = "/v3/mobile_users/{0}/data_collection/captured_value_groups/{1}";
                public const string ENTRIES = "/v3/mobile_users/{0}/ediary/entries";
                public const string PROFILE = "/v3/mobile_users/{0}/profile";
                public const string SCHEDULES = "/v3/mobile_users/{0}/scheduler/personalized_rule_schedules";
                public const string SCHEDULESID = "/v3/mobile_users/{0}/scheduler/personalized_rule_schedules/{1}";
                public const string SESSIONS = "/v3/mobile_users/sessions";
                public const string REGISTRATIONSTATUS = "/v3/mobile_users/registration_validation";
                public const string DELETE = "/api/mobile_users/{0}";
                public const string REGISTER = "/v3/mobile_users/registrations";
                public const string PASSWORD = "/v3/mobile_users/password";
                public const string SITES = "/v3/mobile_users/sites";
                public const string USERSITES = "/v3/mobile_users/{0}/sites";



                public const string GETNOTIFICATIONS =   "/v3/mobile_users/{0}/notifications/deliveries";
                public const string REMOVENOTIFICATION = "/v3/mobile_users/{0}/notifications/deliveries/{1}";
                public const string UPDATENOTIFICATION = "/v3/mobile_users/{0}/notifications/deliveries/{1}";

                public const string AMAZONPAYMENTACCOUNT = "/v3/mobile_users/{0}/payment/account";
                public const string AMAZONPAYMENTREDEEM = "/v3/mobile_users/{0}/payment/redemption";
            }


            internal static class AppMenus
            {
                public const string App_MENUS = "/v3/navigation/app_menus";
            }


            internal static class DynamicContent
			{
				public const string SHOW = "/api/dynamic/{0}/show";
				public const string RANDOM = "/api/dynamic/{0}/random";
				public const string CONTENT_TYPE_ALL = "/api/dynamic/{0}";
				public const string CONTENT_TYPE_FAVORITES = "/api/dynamic/{0}/favorites";
				public const string CONTENT_TYPE_FILTER = "/api/dynamic/{0}/filter";
				public const string CONTENT_TYPE_UPDATE = "/api/dynamic/{0}/{1}";
                public const string VUFORIA_TARGET = "/api/dynamic/{0}/vuforia_target";
			}

			internal static class Devices
			{
				public const string DEVICES = "/v3/devices";
			}

            internal static class Econsent
            {
                public const string ECONSENT = "/v3/mobile_users/{0}/consent/available_strategies";

                public const string CONSENTSTRATEGIES = "/v3/consent/strategies";
                public const string CONSENTSTRATEGIE = "/v3/consent/strategies/{0}";
            }

            internal static class ConsentGrants
            {
                public const string CONSENTGRANTS = "/v3/consent/grants";
            }

            internal static class ConsentForms
            {
                public const string CONSENTFORMS = "/v3/consent/forms ";
            }

            internal static class Flow
            {
                public const string FLOWS = "/v3/data_collection/flow_process_values";
                public const string FLOW_PROCESS_VALUES = "/v3/ediary/entries/{0}/data_collection/flow_process_values";


                public const string FLOW_PROCESS = "/api/v2/data_collection/flow_processes/{0}";
                public const string FLOW_PROCESS_INPUT_DATA = "/api/v2/data_collection/flow_processes/{0}/captured_values/retrieve_all";
                //public const string FLOW_PROCESS_INPUT_DATA_BYID = "/api/collected_values/{0}";
                public const string FLOW_PROCESS_INPUT_DATA_BYID = "/api/v2/data_collection/flow_processes/{0}/captured_values/retrieve_input/{1}";
                public const string FLOW_PROCESS_TRANSITION = "/api/v2/data_collection/flow_processes/{0}/collect";
            }

            internal static class EDiary
            {
                public const string ENTRY_GROUPS = "/v3/ediary/entry_groups";
                public const string ENTRY_TEMPLATES = "/v3/ediary/entry_groups/{0}/ediary/entry_templates";
                public const string EDIARY_ENTRY = "/v3/ediary/entries";
                public const string EDIARY_UPDATE_ENTRY = "/v3/ediary/entries/{0}";
            }

            internal static class TrialSiteContacts
            {
                public const string SiteContacts = "/v3/trials/sites/{0}/trials/site_contacts";

            }

            internal static class Languages
            {
                public const string LANGUAGES = "/v3/languages";
                public const string LANGUAGES_V2 = "/api/languages";
                public const string TRANSLATIONS_V2 = "/api/languages/{0}";
                public const string TRANSLATIONS= "/v3/languages/{0}/translations";
            }

            internal static class Study
            {
                public const string SAVE_AS_DRAFT = "/v3/study/sub_task_drafts";
            }

            internal static class Consult
            {
                public const string JOIN_CONSULTATION = "/v3/video_consultation_join";
            }
        }

        internal static class ExceptionsMessages
        {
            public const string TOKEN_REQUIRED = "AuthToken is required";
            public const string FLOWNULL = "Flow must not to be null";
            public const string FLOWIDREQUIRED = "Flow id is required";
            public const string FLOW_INPUTID = "Flow Input id is required";
            public const string FLOW_TRANSITIONNULL = "Transition is required";
            public const string TEMPLATE_ID_REQUIRED = "Template id is required";
            public const string MOBILE_OWNER_ID_REQUIRED = "Mobile user id is required";
            public const string ID_REQUIRED = "Id is required";
            public const string COHORT_ID_REQUIRED = "Cohort id is required";

        }

        internal static class OptionsBaseHttpService
        {
            public const string Title = "title";
            public const string Obj = "obj";
            public const string TokenRequired = "tokenRequired";
        }

        internal static class SSOProviders
        {
            public const string SSOOPIONS = "v3/sso_options";
        }

        internal static class Contents
        {
            public const string CONTENTS = "/v3/dynamic_content/contents/{0}";
            public const string CONTENTS_WITH_Filter = "/v3/dynamic_content/contents?filters[content_type][permanent_link]={0}";
            public const string PUBLICCONTENTS = "/v3/dynamic_content/public_contents";
        }

        internal static class ExternalOAuth
        {
            public const string AUTHORIZE = "/v3/{0}/authorize";
            public const string VERIFY = "/v3/{0}/verify";
        }
    }
}