using System.Threading.Tasks;

namespace DiscordJunkDrawer.App
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}