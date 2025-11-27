using System;

namespace Xamarin.Forms.Clinical6.UI.Controls
{
    public partial class SSOOptionsController : Button
    {
        public SSOOptionsController()
        {
            InitializeComponent();
        }

        void Handle_Clicked(object sender, EventArgs e)
        {
            var model = this.BindingContext;
        }
    }
}