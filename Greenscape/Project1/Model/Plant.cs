using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Project1.Model
{
    public class Plant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int PlantID { get; set; }
        public string? PlantName { get; set; }
        public string? PlantImage { get; set; }
        public string? Type { get; set; }
        public string? PlantSpecies { get; set; }
        public string? PlantDescription { get; set; }

        public Plant(int plantID, string plantName, string plantImage, string type, string plantSpecies, string plantDescription)
        {
            PlantID = plantID;
            PlantName = plantName;
            PlantImage = plantImage;
            Type = type;
            PlantSpecies = plantSpecies;
            PlantDescription = plantDescription;
        }

        public Plant(string plantName, string plantImage, string type, string plantSpecies, string plantDescription)
        {
            PlantName = plantName;
            PlantImage = plantImage;
            Type = type;
            PlantSpecies = plantSpecies;
            PlantDescription = plantDescription;
        }

        public Plant()
        {
        }


    }
}
