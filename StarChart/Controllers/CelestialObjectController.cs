using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using StarChart.Data;
using StarChart.Models;

namespace StarChart.Controllers
{
    [Route("")]
    [ApiController]
    public class CelestialObjectController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public CelestialObjectController(ApplicationDbContext context)
        {
            this._context = context;
        }


        [HttpGet("{id:int}", Name = "Name")]
        public IActionResult GetById(int id)
        {
 
            var celestialObject = _context.CelestialObjects.Find(id);
            if(celestialObject == null)
            {
                return NotFound();
            }

            foreach(var celestialObjectFromContext in _context.CelestialObjects)
            {
                if(celestialObjectFromContext.OrbitedObjectId == celestialObject.Id)
                {
                    if(celestialObject.Satellites == null)
                    {
                        celestialObject.Satellites = new List<CelestialObject>();
                    }
                    celestialObject.Satellites.Add(celestialObjectFromContext);
                }
            }

            return Ok(celestialObject);
        }

        [HttpGet("{name}")]
        public IActionResult GetByName(string name)
        {
            var celestialObjects = _context.CelestialObjects.Where(c => c.Name == name);

            if(celestialObjects == null || !celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {
                foreach (var celestialObjectFromContext in _context.CelestialObjects)
                {
                    if (celestialObjectFromContext.OrbitedObjectId == celestialObject.Id)
                    {
                        if (celestialObject.Satellites == null)
                        {
                            celestialObject.Satellites = new List<CelestialObject>();
                        }
                        celestialObject.Satellites.Add(celestialObjectFromContext);
                    }
                }
            }

                return Ok(celestialObjects);
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var celestialObjects = _context.CelestialObjects;

            if (celestialObjects == null || !celestialObjects.Any())
            {
                return NotFound();
            }
            foreach (var celestialObject in celestialObjects)
            {
                foreach (var celestialObjectFromContext in _context.CelestialObjects)
                {
                    if (celestialObject.Satellites == null)
                    {
                        celestialObject.Satellites = new List<CelestialObject>();
                    }
                    if (celestialObjectFromContext.OrbitedObjectId == celestialObject.Id)
                    {

                        celestialObject.Satellites.Add(celestialObjectFromContext);
                    }
                }
            }

            return Ok(celestialObjects);
        }

    }

}
