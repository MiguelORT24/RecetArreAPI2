using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RecetArreAPI2.Context;
using RecetArreAPI2.DTOs.Ratings;
using RecetArreAPI2.Models;

namespace RecetArreAPI2.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RatingsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IMapper mapper;
        private readonly UserManager<ApplicationUser> userManager;

        public RatingsController(
            ApplicationDbContext context,
            IMapper mapper,
            UserManager<ApplicationUser> userManager)
        {
            this.context = context;
            this.mapper = mapper;
            this.userManager = userManager;
        }

        // GET: api/ratings/receta/{recetaId} - obtener todas las ratings
        [HttpGet("receta/{recetaId}/todos")]
        public async Task<ActionResult<IEnumerable<RatingDto>>> GetRatingsPorReceta(int recetaId)
        {
            var recetaExiste = await context.Recetas.AnyAsync(r => r.Id == recetaId);
            if (!recetaExiste)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }

            var ratings = await context.Ratings
                .Include(r => r.CalificadoPorUsuario)
                .Where(r => r.RecetaId == recetaId)
                .OrderByDescending(r => r.CalificadoUtc)
                .ToListAsync();

            return Ok(mapper.Map<List<RatingDto>>(ratings));
        }

        // GET: api/ratings/receta/{recetaId}/usuario - obtener la calificación del usuario actual
        [HttpGet("receta/{recetaId}/usuario")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingDto>> GetRatingUsuarioActual(int recetaId)
        {
            var recetaExiste = await context.Recetas.AnyAsync(r => r.Id == recetaId);
            if (!recetaExiste)
            {
                return NotFound(new { mensaje = "Receta no encontrada" });
            }

            var usuarioId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }

            var rating = await context.Ratings
                .Include(r => r.CalificadoPorUsuario)
                .FirstOrDefaultAsync(r => r.RecetaId == recetaId && r.CalificadoPorUsuarioId == usuarioId);

            if (rating == null)
            {
                return NotFound();
            }

            return Ok(mapper.Map<RatingDto>(rating));
        }

        // GET: api/ratings/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<RatingDto>> GetRating(int id)
        {
            var rating = await context.Ratings
                .Include(r => r.CalificadoPorUsuario)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (rating == null)
            {
                return NotFound(new { mensaje = "Rating no encontrado" });
            }

            return Ok(mapper.Map<RatingDto>(rating));
        }

        // POST: api/ratings
        [HttpPost]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<ActionResult<RatingDto>> CreateRating(RatingCreacionDto ratingCreacionDto)
        {
            var recetaExiste = await context.Recetas.AnyAsync(r => r.Id == ratingCreacionDto.RecetaId);
            if (!recetaExiste)
            {
                return BadRequest(new { mensaje = "La receta especificada no existe" });
            }

            var usuarioId = userManager.GetUserId(User);
            if (string.IsNullOrEmpty(usuarioId))
            {
                return Unauthorized(new { mensaje = "Usuario no autenticado" });
            }

            var ratingExistente = await context.Ratings
                .FirstOrDefaultAsync(r => r.RecetaId == ratingCreacionDto.RecetaId && r.CalificadoPorUsuarioId == usuarioId);

            if (ratingExistente != null)
            {
                // Actualizar la calificación existente
                ratingExistente.Calificacion = ratingCreacionDto.Calificacion;
                ratingExistente.CalificadoUtc = DateTime.UtcNow;
                context.Ratings.Update(ratingExistente);
            }
            else
            {
                // Crear nueva calificación
                var rating = mapper.Map<Rating>(ratingCreacionDto);
                rating.CalificadoUtc = DateTime.UtcNow;
                rating.CalificadoPorUsuarioId = usuarioId;
                context.Ratings.Add(rating);
            }

            await context.SaveChangesAsync();

            var ratingGuardado = await context.Ratings
                .Include(r => r.CalificadoPorUsuario)
                .FirstOrDefaultAsync(r => r.RecetaId == ratingCreacionDto.RecetaId && r.CalificadoPorUsuarioId == usuarioId);

            return CreatedAtAction(nameof(GetRating), new { id = ratingGuardado.Id }, mapper.Map<RatingDto>(ratingGuardado));
        }

        // PUT: api/ratings/{id}
        [HttpPut("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> UpdateRating(int id, RatingModificacionDto ratingModificacionDto)
        {
            var rating = await context.Ratings.FirstOrDefaultAsync(r => r.Id == id);

            if (rating == null)
            {
                return NotFound(new { mensaje = "Rating no encontrado" });
            }

            var usuarioId = userManager.GetUserId(User);
            if (rating.CalificadoPorUsuarioId != usuarioId)
            {
                return Forbid();
            }

            mapper.Map(ratingModificacionDto, rating);
            context.Ratings.Update(rating);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Rating actualizado exitosamente", data = mapper.Map<RatingDto>(rating) });
        }

        // DELETE: api/ratings/{id}
        [HttpDelete("{id}")]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> DeleteRating(int id)
        {
            var rating = await context.Ratings.FirstOrDefaultAsync(r => r.Id == id);

            if (rating == null)
            {
                return NotFound(new { mensaje = "Rating no encontrado" });
            }

            var usuarioId = userManager.GetUserId(User);
            if (rating.CalificadoPorUsuarioId != usuarioId)
            {
                return Forbid();
            }

            context.Ratings.Remove(rating);
            await context.SaveChangesAsync();

            return Ok(new { mensaje = "Rating eliminado exitosamente" });
        }
    }
}
