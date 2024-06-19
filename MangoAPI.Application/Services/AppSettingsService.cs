using System;
using System.IO;

namespace MangoAPI.Application.Services;

public static class AppSettingsService
{
    private const string AppSettingsPath = "../../../../MangoAPI.Presentation/appsettings.json";
    private const string _appSettingsPath = "../../../../MangoAPI.Presentation/appsettings.json";

    public static string GetAppSettingsPath()
    {
        var a = _appSettingsPath;
        Console.Write(a);
        var path = Path.Combine(AppContext.BaseDirectory, AppSettingsPath);
        return path;
    }
}
