using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Project1.Model
{
    public class PointsHistory
    {
        [Key]
        public int EntryID { get; set; }
        [ForeignKey("ApplicationUser")]
        public string UserID { get; set; }
        public int PointsModified { get; set; }
        public DateTime EntryDate { get; set; }
        public string? Source { get; set; }

        public ApplicationUser ApplicationUser { get; set; }
    }
}
