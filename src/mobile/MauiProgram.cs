using CommunityToolkit.Maui;
using IeuanWalker.Maui.StateButton;
using MauiChat.Helpers;
using MauiChat.Models;
using MauiChat.Services;
using MauiChat.ViewModels;
using Microsoft.Extensions.Logging;
using Microsoft.Maui.LifecycleEvents;
using SkiaSharp.Views.Maui.Controls.Hosting;
using System.Reflection.Metadata;
using System.Text.Json;

namespace MauiChat;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{ 
        var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
            .UseStateButton()
            .UseSkiaSharp()
            .UseMauiCommunityToolkit()
            .ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

#if DEBUG
		builder.Logging.AddDebug();
#endif
         
        Microsoft.Maui.Handlers.EntryHandler.Mapper.AppendToMapping(nameof(Entry), (handler, view) =>
        {
#if ANDROID
            handler.PlatformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
#endif
        });

        builder.Services.AddSingleton<ISignalRService, SignalRService>();
        builder.Services.AddTransient<MainPageViewModel>();
        builder.Services.AddSingleton<MainPage>();

        return builder.Build();
	}
}
