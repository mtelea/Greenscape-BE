namespace Project1.Model
{
    public class Plant
    {
        public int Id { get; set; }
        public string PlantName { get; set; }
        public PlantType Type { get; set; }
        public string PlantSpecies { get; set; }
        public string PlantDescription { get; set; }

        public Plant()
        {

        }

        public enum PlantType
        {
            Vegetable,
            Fruit,
            Flower
        }
    }
}
