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
        protected IServiceProvider ServiceProvider { get; set; }


        protected TestBase()
        {
            //loob uue teenuse kogu vajalikuks teenuste testimiseks
            var services = new ServiceCollection();
            //kutsub välja meetodi, mis lsab teenused ja vajalikud sõltuvused
            SetupServices(services);
            //selle kaudu saab teenuseid küsida 
            ServiceProvider = services.BuildServiceProvider();
        }


        // Registreerib testide jaoks vajalikud teenused ja andmebaasi
        public virtual void SetupServices(IServiceCollection services)
        {
            services.AddScoped<IRealEstateServices, RealEstateServices>();

            //registreerib andmebaasi konteksti
            services.AddDbContext<ShopTARpe25Context>(x =>
            {
                //seadistab kasutama mälusisest andmebaasi nimega TEST, et testimisel kasutada
                //ilm päris andmebaasita
                x.UseInMemoryDatabase("TEST");

                //eriab hoiatust, et mälusisena DB ei toeta tehinguid samamoodi nau päris andmebaas
                x.ConfigureWarnings(b => b.Ignore(InMemoryEventId.TransactionIgnoredWarning));
            });
            //kutsub välja macrode klassi
            RegisterMacros(services);
        }


        // Vabastab ressursid peale kasutamist
        public void Dispose()
        {

        }


        //abimeetodit, millega klass ja selle alamklassid saavad teenuse tööbi järgi küsida
        protected T Svc<T>()
        {
            //otsib teenusepakkuja tüüpi
            return ServiceProvider.GetService<T>();
        }

        //abimeetodit makrode leidmiseks ja registreerimiseks
        private void RegisterMacros(IServiceCollection services)
        {
            //saaks reflektsiooni otsingus kasutada 
            var macroBaseType = typeof(IMacros);
            
            //loeb kokku kõik tüübid sellest assemblyst, kus Macro on määretletud
            var macros = macroBaseType.Assembly.GetTypes()
                //valib tüübid, mis rakendavad IMacro liidest, aga ei ole ise liides
                //ega abstraktsed klassid 
                .Where(t => macroBaseType.IsAssignableFrom(t)
                && !t.IsInterface && !t.IsAbstract);
            // käib leitud makrode classid ükshaaval läbi
            foreach (var macro in macros)
            {
                //kasutab ühte macro ehk eksemplaari 
                services.AddSingleton(macro);
            }
        }
    }
}
