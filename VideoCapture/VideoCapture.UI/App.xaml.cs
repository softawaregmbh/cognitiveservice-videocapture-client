using CognitiveServices.ComputerVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using VideoCapture.Common;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public IServiceProvider ServiceProvider { get; private set; }

        public IConfiguration Configuration { get; private set; }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var builder = new ConfigurationBuilder()
                 .SetBasePath(Directory.GetCurrentDirectory())
                 .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                 .AddJsonFile("appsettings.dev.json", optional: true, reloadOnChange: true);

            this.Configuration = builder.Build();

            var serviceCollection = new ServiceCollection();
            this.ConfigureServices(serviceCollection);

            this.ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IVideoGrabber, VideoGrabber>();

            services.Configure<ComputerVisionSettings>(Configuration.GetSection(nameof(ComputerVisionSettings)));

            services.AddTransient<IImageAnalyzer, DemoAnalyzer.DemoAnalyzer>();
            services.AddTransient<IImageAnalyzer, ComputerVisionAnalyzer>();

            services.AddScoped<MainViewModel>(
                sp => new MainViewModel(
                    this.ServiceProvider.GetRequiredService<IVideoGrabber>(),
                    TimeSpan.FromSeconds(1),
                    this.ServiceProvider.GetServices<IImageAnalyzer>().ToArray()));
            services.AddTransient(typeof(MainWindow));
        }
    }
}
