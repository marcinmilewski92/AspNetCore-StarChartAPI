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

        [HttpPost]
        public IActionResult Create([FromBody] CelestialObject celestialObject)
        {
            var newCelestialObject = _context.CelestialObjects.Add(celestialObject);
            _context.SaveChanges();

            return CreatedAtRoute("GetById", new { id = newCelestialObject.Entity.Id}, newCelestialObject.Entity);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, CelestialObject celestialObject)
        {
            var actualCelestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (actualCelestialObject == null)
            {
                return NotFound();
            }

            actualCelestialObject.Name = celestialObject.Name;
            actualCelestialObject.OrbitalPeriod = celestialObject.OrbitalPeriod;
            actualCelestialObject.OrbitedObjectId = celestialObject.OrbitedObjectId;

            _context.Update(actualCelestialObject);
            _context.SaveChanges();
            return NoContent();
        }

        [HttpPatch("{id}/{name}")]
        public IActionResult RenameObject(int id, string name)
        {
            var actualCelestialObject = _context.CelestialObjects.FirstOrDefault(c => c.Id == id);
            if (actualCelestialObject == null) {
                return NotFound();
            }
            actualCelestialObject.Name = name;
            _context.CelestialObjects.Update(actualCelestialObject);
            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {
            var celestialObjects =_context.CelestialObjects.Where(c => (c.Id == id || c.OrbitedObjectId == id));
            if (celestialObjects == null || !celestialObjects.Any())
            {
                return NotFound();
            }
            _context.CelestialObjects.RemoveRange(celestialObjects);
            _context.SaveChanges();
            return NoContent();
        }

    }

}
