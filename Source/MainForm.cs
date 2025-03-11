using AcrylicUI.Forms;
using ASPForEnhance.Services;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Radzen;
using System.Reflection;

namespace ASPForEnhance
{
    public partial class MainForm : AcrylicForm
    {
        public MainForm()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            Text = $"ASP for Enhance.com - {GetAssemblyVersion()}";
            MinimumSize = new Size(1600, 1300);
            Size = new Size(2000, 1300);
            var services = new ServiceCollection();
            services.AddSingleton<ServerService>();
            services.AddScoped<DialogService>();
            services.AddScoped<NotificationService>();
            services.AddScoped<ContextMenuService>();
            services.AddScoped<TooltipService>();
#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif
            services.AddWindowsFormsBlazorWebView();
            blazorWebView1.HostPage = "wwwroot\\index.html";
            blazorWebView1.Services = services.BuildServiceProvider();
            blazorWebView1.RootComponents.Add<Index>("#app");
       

        }

        private string GetAssemblyVersion()
        {
            var version = Assembly.GetExecutingAssembly().GetName().Version;
            return version != null ? version.ToString() : "Unknown Version";
        }
    }
}
