using AcrylicUI.Forms;
using ASPForEnhance.Services;
using Microsoft.AspNetCore.Components.WebView.WindowsForms;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Web.WebView2.Core;
using Radzen;

namespace ASPForEnhance
{
    public partial class MainForm : AcrylicForm
    {
        public MainForm()
        {
            AutoScaleMode = AutoScaleMode.Dpi;
            InitializeComponent();
            this.MinimumSize = new Size(1600, 1300);
            this.Size = new Size(2000, 1300);
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

      
    }
}
