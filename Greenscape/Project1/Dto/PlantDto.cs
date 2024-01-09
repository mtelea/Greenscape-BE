using static Project1.Model.Plant;

namespace Project1.Dto
{
    public class PlantDto
    {
        public string PlantName { get; set; }
        public string? PlantImage { get; set; }
        public string Type { get; set; }
        public string PlantSpecies { get; set; }
        public string PlantDescription { get; set; }

        public PlantDto(string plantName, string? plantImage, string type, string plantSpecies, string plantDescription)
        {
            PlantName = plantName;
            PlantImage = plantImage;
            Type = type;
            PlantSpecies = plantSpecies;
            PlantDescription = plantDescription;
        }
    }
}
