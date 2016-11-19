using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Hintme.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Builder;

namespace Hintme
{
    public class Program
    {
        public static void Main(string[] args)
        {
            //new GoogleSheetsRepository().SaveHint(new Hint
            //{
            //    Latitude = 54.69268,
            //    Longitude = 25.27259,
            //    Text = "Jurgis Kairys po Baltuoju tiltu skrido aukštyn kojomis. Tądien, 1999 metų rugsėjo 19-ą, jis per 20 min praskrido po visais didžiaisiais Vilniaus tiltais, net dešimčia jų. <br>Video:\nhttps://www.youtube.com/watch?v=zz5AcbaJzLo",
            //    Url = "http://istorijosmessengerbot.azurewebsites.net/kairys.jpg",
            //    Header = "Aukštyn kojomis po tiltu"
            //});
                


            var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .Build();

            host.Run();
        }
    }
}
