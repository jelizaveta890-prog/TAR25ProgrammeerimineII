using Microsoft.Extensions.DependencyInjection;
using ShopTARpe25.ApplicationServices.Services;
using ShopTARpe25.Core.ServiceInterface;
using ShopTARpe25.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using ShopTARpe25.RealEstateTest.Macros;

namespace ShopTARpe25.RealEstateTest
{
    // Abstraktne baasklass testidele – loob ühtse keskkonna, et koodi mitte korrata
    public abstract class TestBase 
    {
       // Hoiab valmis ehitatud teenuste konteinerit, kust testid saavad vajalikke objekte küsida
        protected IServiceProvider ServiceProvider { get; set; }


        // Konstruktor: käivitub automaatselt iga testi alguses ja seab teenused valmis
        protected TestBase()
        {
            var services = new ServiceCollection();
            SetupServices(services);
            // Ehitab registreeritud teenustest töötava konteineri
            ServiceProvider = services.BuildServiceProvider();
        }


        // Registreerib testide jaoks vajalikud teenused ja andmebaasi
        public virtual void SetupServices(IServiceCollection services)
        {
            // Seob liidese IRealEstateServices tema tegeliku teostusega (RealEstateServices)
            services.AddScoped<IRealEstateServices, RealEstateServices>();
            // Seadistab päris SQL-andmebaasi asemele ajutise mälupõhise (In-Memory) andmebaasi
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                x.UseInMemoryDatabase("TEST");
                // Eirab transaktsioonide hoiatusi, kuna mälupõhine andmebaas neid ei toeta
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });

            RegisterMacros(services);
        }


        // Vabastab ressursid pärast testi lõppu
        public void Dispose()
        {

        }


        // Abimeetod, millega saab konteinerist kiiresti soovitud teenuse kätte (nt Svc<IRealEstateServices>())
        protected T Svc<T>()
        {
            return ServiceProvider.GetService<T>();
        }


        // Otsib projektist automaatselt üles kõik IMacros liidest kasutavad abiklassid ja registreerib need
        private void RegisterMacros(IServiceCollection services)
        {
            var macroBaseType = typeof(IMacros);
            // Leiab kõik konkreetsed klassid (mitte liidesed ega abstraktsed klassid), mis pärinevad IMacros liidesest
            var macros = macroBaseType.Assembly.GetTypes()
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
            // Registreerib iga leitud makroklassi ühekordse instantsina (Singleton)
            foreach (var macro in macros)
            {
                services.AddSingleton(macro);
            }
        }
    }
}
