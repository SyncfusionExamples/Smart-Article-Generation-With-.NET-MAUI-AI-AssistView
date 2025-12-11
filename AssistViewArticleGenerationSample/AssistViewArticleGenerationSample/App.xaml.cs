using AssistViewArticleGenerationSample.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AssistViewArticleGenerationSample
{
    public partial class App : Application
    {
        public App()
        {
            InitializeComponent();
            Syncfusion.Licensing.SyncfusionLicenseProvider.RegisterLicense("Ngo9BigBOggjHTQxAR8/V1JFaF1cXGFCf0x3WmFZfVhgcl9CaVZSRGYuP1ZhSXxWd0djUH9ccHxRQGlaUkB9XEM=");
        }

        protected override Window CreateWindow(IActivationState? activationState)
        {
            return new Window(new MainPage());
        }
    }
}