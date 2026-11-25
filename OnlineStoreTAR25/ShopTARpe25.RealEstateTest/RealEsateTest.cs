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
        //mul on vaja testida DetailsAsync meetodit
    
    }
}   