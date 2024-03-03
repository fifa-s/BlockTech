using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;

namespace BlockTech;

class Program
{
    static void Main(string[] args)
    {
        Logger.SetLevel(LoggerLevel.Debug);

        NativeWindowSettings window_settings = new NativeWindowSettings()
        {
                ClientSize = new Vector2i(800, 600),
                Title = "LearnOpenTK - Creating a Window",
                // This is needed to run on macos
                Flags = ContextFlags.ForwardCompatible,
        }; 
        using(Game game = new Game(GameWindowSettings.Default, window_settings)) {
            game.Run();
        }
    }
}