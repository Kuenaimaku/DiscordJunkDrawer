using System.Threading.Tasks;

namespace DiscordJunkDrawer
{
    class Program
    {
        public static Task Main(string[] args)
            => Startup.RunAsync(args);
    }
}