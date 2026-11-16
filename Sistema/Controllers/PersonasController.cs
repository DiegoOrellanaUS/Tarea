using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema.Dominio;
using System.ComponentModel.DataAnnotations;

[Route("api/[controller]")]
[ApiController]
public class PersonasController : ControllerBase
{
    private readonly SistemaContext _context;
    public PersonasController(SistemaContext context)
    {
        _context = context;
    }

    [HttpGet]
    [HttpGet("listar")]
    public async Task<ActionResult<IEnumerable<Persona>>> GetPersonas()
    {
        return await _context.Persona.AsNoTracking().OrderBy(persona => persona.Id).ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Persona>> GetPersona(int id)
    {
        var persona = await _context.Persona.FindAsync(id);
        return persona is null ? NotFound() : persona;
    }

    [HttpPost]
    public async Task<ActionResult<Persona>> PostPersona([FromBody] PersonaDatos datos)
    {
        if (await _context.Persona.AnyAsync(persona => persona.Ci == datos.Ci))
        {
            return BadRequest("Ya existe una persona con ese CI");
        }

        var persona = new Persona
        {
            Nombre = datos.Nombre,
            Ci = datos.Ci,
            Estado = datos.Estado ?? "Activo"
        };

        _context.Persona.Add(persona);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetPersona), new { id = persona.Id }, persona);
    }

    [HttpPost("crear")]
    public Task<ActionResult<Persona>> PostPersonaDesdeConsulta(
        [FromQuery, Required] string nombre, [FromQuery, Required] string ci)
    {
        return PostPersona(new PersonaDatos(nombre, ci));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> PutPersona(int id, PersonaDatos datos)
    {
        var persona = await _context.Persona.FindAsync(id);
        if (persona is null)
        {
            return NotFound();
        }

        if (await _context.Persona.AnyAsync(otra => otra.Id != id && otra.Ci == datos.Ci))
        {
            return BadRequest("Ya existe una persona con ese CI");
        }

        persona.Nombre = datos.Nombre;
        persona.Ci = datos.Ci;
        persona.Estado = datos.Estado ?? persona.Estado;
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> DeletePersona(int id)
    {
        var persona = await _context.Persona.FindAsync(id);
        if (persona is null)
        {
            return NotFound();
        }

        _context.Persona.Remove(persona);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    public record PersonaDatos(
        [Required] string Nombre,
        [Required] string Ci,
        string? Estado = null);
}
