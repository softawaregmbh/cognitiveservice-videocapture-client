// <copyright file="App.xaml.cs" company="softaware gmbh">
// Copyright (c) softaware gmbh. All rights reserved.
// </copyright>

using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using CognitiveServices.ComputerVision;
using CognitiveServices.CustomVision;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using VideoCapture.Common;
using VideoCapture.Grabber;

namespace VideoCapture.UI
{
    /// <summary>
    /// Interaction logic for App.xaml.
    /// </summary>
    public partial class App : Application
    {
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        public IServiceProvider ServiceProvider { get; private set; }

        /// <summary>
        /// Gets the configuration.
        /// </summary>
        public IConfiguration Configuration { get; private set; }

        /// <summary>
        /// Raises the <see cref="E:System.Windows.Application.Startup" /> event.
        /// </summary>
        /// <param name="e">A <see cref="T:System.Windows.StartupEventArgs" /> that contains the event data.</param>
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

            var mainWindow = this.ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        private void ConfigureServices(IServiceCollection services)
        {
            services.Configure<VideoGrabberSettings>(this.Configuration.GetSection(nameof(VideoGrabberSettings)));
            services.AddSingleton<IVideoGrabber, VideoGrabber>();

            services.Configure<ComputerVisionSettings>(this.Configuration.GetSection(nameof(ComputerVisionSettings)));
            services.Configure<CustomVisionSettings>(this.Configuration.GetSection(nameof(CustomVisionSettings)));

            // services.AddTransient<IImageAnalyzer, ComputerVisionAnalyzer>();
            services.AddTransient<IImageAnalyzer, CustomVisionAnalyzer>();

            services.AddScoped<MainViewModel>(
                sp => new MainViewModel(
                    this.ServiceProvider.GetRequiredService<IVideoGrabber>(),
                    TimeSpan.FromSeconds(1),
                    this.ServiceProvider.GetServices<IImageAnalyzer>().ToArray()));
            services.AddTransient(typeof(MainWindow));
        }
    }
}
