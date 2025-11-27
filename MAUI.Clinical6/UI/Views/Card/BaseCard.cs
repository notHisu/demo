using System;
using System.Windows.Input;

namespace Xamarin.Forms.Clinical6.UI.Views.Card
{
    public class BaseCard : IMaterialCard
    {
        public string Id { get; set; }
        public string Type { get; set; }
        public int Priority { get; set; } = 0;
        public string AdditionalInfo { get; set; }
        public string Body { get; set; }
        public string ImageSource { get; set; }
        public string Label { get; set; }
        public string Title { get; set; }
        public bool IsVisible { get; set; } = false;
        public object ButtonReference { get; set; }
        public bool ButtonIsVisible { get; set; } = false;
        public string ButtonText { get; set; }

        public override bool Equals(Object obj)
        {
            BaseCard c = obj as BaseCard;
            if (c == null)
                return false;
            else
            {
                // Should only compare values to determine equality, not object identity.
                bool isEqual =
                       Id == c.Id
                    && Type == c.Type
                    && Body == c.Body
                    && ImageSource == c.ImageSource
                    && Label == c.Label
                    && Title == c.Title
                    && ButtonText == c.ButtonText;
                return isEqual;
            }
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
