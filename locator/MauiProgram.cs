using Microsoft.Extensions.Logging;
using CommunityToolkit.Maui.Maps;

namespace locator
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            //key from dev account rob@bogle.tools

            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkitMaps("fkYHCmIsjNDXuOe28mvw~TAkfJSkQ-xmZl2P5Mfs_LA~AkCmFvJJ8Ei1DlY_ykD5hQyzPYTw0AeoVvCHUdoZWtKnXg9m50q7ZX1tjPamoXDi")
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseMauiMaps();
#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
