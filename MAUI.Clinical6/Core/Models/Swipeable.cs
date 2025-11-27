namespace Xamarin.Forms.Clinical6.Core.Models
{
    public class Swipeable
    {
        public string LeftButtonText { get; set; } = "[NotSet]";
        public string LeftButtonImage { get; set; } = "";
        public string RightButtonText { get; set; } = "[NotSet]";
        public string RightButtonImage { get; set; } = "";

        public object Item { get; set; }

        public bool IsNewInfoAvailable { get; set; }

        public string Title { get; set; }

        public string Description { get; set; }

        public string ExtraInfo { get; set; }
    }
}