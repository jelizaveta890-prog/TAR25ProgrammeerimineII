using Microsoft.EntityFrameworkCore.Metadata;
using ShopTARpe25.Core.Dto;
using ShopTARpe25.Core.ServiceInterface;

namespace ShopTARpe25.RealEstateTest
{
    public class RealEstateTest : TestBase
    {
        [Fact]
        public async Task ShouldNot_AddEmptyRealEstate_WhenReturnResult()
        {
            //Arrange
            RealEstateDto dto = new()
            {
                Area = 120.5,
                Address = "Test location",
                RoomNumber = 3,
                BuildingType = "house",
                CreatedAt = DateTime.UtcNow,
                ModifiedAt = DateTime.UtcNow,
            };

            //Act Siin tuleb kutsuda RealEstate Create meetodi välja
            var result = await Svc<IRealEstateServices>().Create(dto);

            //Assert
            Assert.NotNull(result);

        }

        //teha test, kus otsib ankeeti id alusel, aga seda ei ole andmebaasis
        //tuli vaade, mis ei sobi rakenduse jaoks.
        [Fact]
        public async Task ShouldNot_GetByIdRealEstate_WhenReturnsNotEqual()
        {
            //Arrange   
            Guid databaseGuid = Guid.Parse("fe2faf0f-f8c1-42b3-a907-9b23d0666fc0");
            Guid wrongGuid = Guid.NewGuid();

            //Act
            await Svc<IRealEstateServices>().DetailAsync(databaseGuid);

            //Assert
            //vastupidist loogikat tuleb kasutada 
            Assert.NotEqual(wrongGuid, databaseGuid);
        }

        [Fact]
        public async Task Should_GetByIdRealEstate_WhenReturnsEqual()
        {
            // Arrange
            //see guid on juba andmebaasis olemas ankeedi juures
            Guid databaseGuid = Guid.Parse("fe2faf0f-f8c1-42b3-a907-9b23d0666fc0");
            //kui vajutad nupule, siis see guid saadetakse andmebaasi, mida võrreldakse
            Guid guid = Guid.Parse("fe2faf0f-f8c1-42b3-a907-9b23d0666fc0");

            // Act
            await Svc<IRealEstateServices>().DetailAsync(guid);

            // Assert
            Assert.Equal(databaseGuid, guid);
        }

        //kustutamise meetodit on vaja testida. Andmebaasis on ankeed, mida
        //tahan ära kustutada
        [Fact]
        public async Task Should_DeleteByIdRealEstate_WhenDeleteRealEstate()
        {
            //Arrange
            RealEstateDto dto = MockRealEstateData();

            //Act
            var addRealEstate = await Svc<IRealEstateServices>().Create(dto);
            var deleteRealEstate = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate.Id);

            //Assert
            //võrdlen Id-sid, kas kustutasin selle ära, mis oli andmebaasis
            Assert.Equal(deleteRealEstate.Id, addRealEstate.Id);

        }
        private RealEstateDto MockRealEstateData()
        {
            return new RealEstateDto
            {
                Area = 88.65,
                Address = "TestAdress",
                RoomNumber = 1,
                BuildingType = "TestBuildingType",
                CreatedAt = DateTime.Now,
                ModifiedAt = DateTime.Now,
            };
        }
    }
}