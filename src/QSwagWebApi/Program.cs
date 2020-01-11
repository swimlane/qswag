using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;

namespace QSwagWebApi
{
    public class Program
    {
        public static void Main(string[] args)
        {
          WebHost.CreateDefaultBuilder().Build().Run();
        }
    }
}
