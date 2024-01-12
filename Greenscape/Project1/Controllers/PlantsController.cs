using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Project1.Data;
using Project1.Dto;
using Project1.Mapper;
using Project1.Model;

namespace Project1.Controllers
{
    [Route("/plants")]
    [ApiController]
    public class PlantsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly PlantMapper _plantMapper;

        public PlantsController(AppDbContext context, PlantMapper plantMapper)
        {
            _context = context;
            _plantMapper = plantMapper;
        }

        [HttpGet("get/{plantId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Plant>> GetPlantById(int plantId)
        {
            var plant = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plant == null)
            {
                return NotFound("Plant not found");
            }

            return Ok(plant);
        }

        // TODO: Image upload on add plant
        [HttpPost("add")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<Plant>> AddPlant([FromBody] PlantDto newPlant)
        {
            if (newPlant == null)
            {
                return BadRequest("Plant data is invalid");
            }

            var validPlantTypes = new List<string> { "legume", "fructe", "flori" };

            if (!validPlantTypes.Contains(newPlant.Type?.ToLower()))
            {
                return BadRequest("Plant type not valid!");
            }

            var existingPlant = await _context.Plant.FirstOrDefaultAsync(p => p.PlantName == newPlant.PlantName);
            if (existingPlant != null)
            {
                return Conflict("A plant with the same name already exists");
            }

            _context.Plant.Add(_plantMapper.MapDtoToPlant(newPlant));
            await _context.SaveChangesAsync();

            return Ok(newPlant);
        }

        [HttpDelete("delete/{plantId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> DeletePlant(int plantId)
        {
            var plantToRemove = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plantToRemove == null)
            {
                return NotFound("Plant not found");
            }

            _context.Plant.Remove(plantToRemove);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpGet("getAll")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetAllPlants()
        {
            var plants = await _context.Plant.ToListAsync();

            if (plants == null || plants.Count == 0)
            {
                return NotFound("No plants found");
            }

            return Ok(plants);
        }

        /*        [HttpPut("update/{plantId}/name")]
                [Authorize(Roles = "Admin, User")]
                public async Task<IActionResult> UpdatePlantName(int plantId, [FromBody] string newName)
                {
                    var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
                    if (plantToUpdate == null)
                    {
                        return NotFound("Plant not found");
                    }

                    plantToUpdate.PlantName = newName;
                    await _context.SaveChangesAsync();

                    return Ok("Updated successfully");
                }*/

        [HttpPost("update/{plantId}")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdatePlant(int plantId, [FromBody] Plant newPlant) {
            var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }

            var validPlantTypes = new List<string> { "legume", "fructe", "flori" };

            if (!validPlantTypes.Contains(newPlant.Type.ToLower()))
            {
                return BadRequest("Plant type not valid!");
            }

            plantToUpdate.PlantName = newPlant.PlantName;
            plantToUpdate.Type = newPlant.Type;
            plantToUpdate.PlantSpecies = newPlant.PlantSpecies;
            plantToUpdate.PlantDescription = newPlant.PlantDescription;

            await _context.SaveChangesAsync();

            return Ok("Updated successfully");
        }

        [HttpPost("update/{plantId}/image")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdatePlantImage(int plantId, IFormFile picture)
        {
            var allowedExtensions = new[] { ".png", ".jpg", ".jpeg" };
            var fileExtension = Path.GetExtension(picture.FileName).ToLower();

            if (!allowedExtensions.Contains(fileExtension))
            {
                return BadRequest(new { Message = "Invalid file format. Only PNG, JPG, and JPEG files are allowed." });
            }

            var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);

            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }
            if (picture == null || picture.Length == 0)
            {
                return BadRequest(new { Message = "Invalid picture file." });
            }

            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }

            if (plantToUpdate.PlantImage != null) 
            {
                var oldFilePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", plantToUpdate.PlantImage);
                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }
            }

            string newFileName = Guid.NewGuid().ToString();

            var filePath = Path.Combine("wwwroot/images/plants", $"{newFileName}{Path.GetExtension(picture.FileName)}");
            var filePathToSave = Path.Combine("images/plants", $"{newFileName}{Path.GetExtension(picture.FileName)}");

            using (var fileStream = System.IO.File.Create(filePath))
            {
                await picture.CopyToAsync(fileStream);
            }
            plantToUpdate.PlantImage = filePathToSave;

            try
            {
                await _context.SaveChangesAsync();
                return Ok(new { Message = "Plant picture set successfully." });
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine(e);
                return BadRequest(new { Message = "Setting plant picture failed." });

            }
        }

        /*[HttpPut("update/{plantId}/type")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdatePlantType(int plantId, [FromBody] string newType)
        {
            var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }

            var validPlantTypes = new List<string> { "legume", "fructe", "flori" };

            if (!validPlantTypes.Contains(newType.ToLower()))
            {
                return BadRequest("Plant type not valid!");
            }

            plantToUpdate.Type = newType;
            await _context.SaveChangesAsync();

            return Ok("Updated successfully");
        }*/

        /*[HttpPut("update/{plantId}/species")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdatePlantSpecies(int plantId, [FromBody] string newSpecies)
        {
            var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }

            plantToUpdate.PlantSpecies = newSpecies;
            await _context.SaveChangesAsync();

            return Ok("Updated successfully");
        }*/

        /*[HttpPut("update/{plantId}/description")]
        [Authorize(Roles = "Admin, User")]
        public async Task<IActionResult> UpdatePlantDescription(int plantId, [FromBody] string newDescription)
        {
            var plantToUpdate = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plantToUpdate == null)
            {
                return NotFound("Plant not found");
            }

            plantToUpdate.PlantDescription = newDescription;
            await _context.SaveChangesAsync();

            return Ok("Updated successfully");
        }*/


        [HttpGet("getByType/legume")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetLegume()
        {
            var legume = await _context.Plant.Where(p => p.Type == "legume").ToListAsync();

            if (legume == null || legume.Count == 0)
            {
                return NotFound("Nu s-au gasit legume");
            }

            return Ok(legume);
        }

        [HttpGet("getByType/fructe")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetFructe()
        {
            var fructe = await _context.Plant.Where(p => p.Type == "fructe").ToListAsync();

            if (fructe == null || fructe.Count == 0)
            {
                return NotFound("Nu s-au gasit fructe");
            }

            return Ok(fructe);
        }

        [HttpGet("getByType/flori")]
        [Authorize(Roles = "Admin, User")]
        public async Task<ActionResult<IEnumerable<Plant>>> GetFlori()
        {
            var flori = await _context.Plant.Where(p => p.Type == "flori").ToListAsync();

            if (flori == null || flori.Count == 0)
            {
                return NotFound("Nu s-au gasit flori");
            }

            return Ok(flori);
        }



    }
}
