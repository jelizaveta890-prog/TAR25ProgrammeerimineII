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
        //mul on kaks ankeeti loodud ja ühe kustutan ära 
        //assertis võdlen, kas need kaks on ühesugused
        [Fact]
        public async Task ShouldNot_DeleteByIdRealEstate_WhenDidNotDeleteRealEstate()
        {
            //Arrange
            var dto = MockRealEstateData();

            //Act

            var addRealEstate1 = await Svc<IRealEstateServices>().Create(dto);
            var addRealEstate2 = await Svc<IRealEstateServices>().Create(dto);

            var result = await Svc<IRealEstateServices>().Delete((Guid)addRealEstate2.Id);

            //Assert
            Assert.NotEqual(addRealEstate1.Id, result.Id);
        }
        //uuendab andmeid 
        [Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateData()
        {
            // Arrange
            var guid = new Guid("fe2faf0f-f8c1-42b3-a907-9b23d0666fc0");

            RealEstateDto dto = MockRealEstateData();
           
            RealEstateDto domain = new();

            domain.Id = guid;
            domain.Area = 200.0;
            domain.Address = "Update Location";
            domain.RoomNumber = 5;
            domain.BuildingType = "Villa";
            domain.CreatedAt = DateTime.UtcNow;
            domain.ModifiedAt = DateTime.UtcNow;

            // act
            await Svc<IRealEstateServices>().Update(dto);

            // Assert, 5tk teha
            Assert.Equal(domain.Id, guid);
            Assert.NotEqual(dto.Area, domain.Area);
            Assert.NotEqual(dto.RoomNumber, domain.RoomNumber);
            //Võrrelda RoomNumber ja kasutada DoesNotMatch
            //DoesNotMatch tahab string andmetüübe
            Assert.DoesNotMatch(dto.RoomNumber.ToString(), domain.RoomNumber.ToString());
            Assert.DoesNotMatch(dto.Address, domain.Address);
        }
        //[Fact]
        public async Task Should_UpdateRealEstate_WhenUpdateDataVersion2()
        {

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