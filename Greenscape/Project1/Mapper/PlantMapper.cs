using Project1.Dto;
using Project1.Model;

namespace Project1.Mapper
{
    public class PlantMapper
    {
        public Plant MapDtoToPlant(PlantDto plantDto)
        {
            return new Plant
            {
                PlantName = plantDto.PlantName,
                PlantImage = plantDto.PlantImage,
                Type = plantDto.Type,
                PlantSpecies = plantDto.PlantSpecies,
                PlantDescription = plantDto.PlantDescription
            };
        }
    }
}
