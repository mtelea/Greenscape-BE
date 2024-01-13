using Microsoft.AspNetCore.Mvc;

namespace Project1.Model
{
    public class PointRequestModel
    {
        public required int Points { get; set; }
        public required string Operation { get; set; }
        public string Source { get; set; }
    }
}
