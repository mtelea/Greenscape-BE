using System;
using System.Collections.Generic;
using System.Data;
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
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Plant>> GetPlantById(int plantId)
        {
            var plant = await _context.Plant.FirstOrDefaultAsync(p => p.PlantID == plantId);
            if (plant == null)
            {
                return NotFound("Plant not found");
            }

            return Ok(plant);
        }


        [HttpPost("add")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult<Plant>> AddPlant([FromBody] PlantDto newPlant)
        {
            if (newPlant == null)
            {
                return BadRequest("Plant data is invalid");
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
        [Authorize(Roles = "Admin")]
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


    }
}
