namespace Project1.Model
{
    public class Plant
    {
        public int PlantID { get; set; }
        public string PlantName { get; set; }
        public string PlantImage { get; set; }
        public PlantType Type { get; set; }
        public string PlantSpecies { get; set; }
        public string PlantDescription { get; set; }

        public Plant(int plantID, string plantName, string plantImage, PlantType type, string plantSpecies, string plantDescription)
        {
            PlantID = plantID;
            PlantName = plantName;
            PlantImage = plantImage;
            Type = type;
            PlantSpecies = plantSpecies;
            PlantDescription = plantDescription;
        }

        public enum PlantType
        {
            Vegetable,
            Fruit,
            Flower,
            Tree
        }
    }
}
