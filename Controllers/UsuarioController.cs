using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using puc.Context;
using puc.Models;
using puc.DTOS;
using Microsoft.AspNetCore.Authorization;
using System.IdentityModel.Tokens.Jwt;
using System.Text;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace puc.Controllers
{
    [ApiController]
    //[Authorize]
    [Route("api/[controller]")]
#pragma warning disable CS1591
    public class UsuarioController : ControllerBase
    {
        private readonly AppDbContext _context;
        public UsuarioController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Usuarios.ToListAsync();
            return Ok(model);
        }


        [AllowAnonymous]
        [HttpPost]
        public async Task<ActionResult> Create(UsuarioDto model)
        {
            Usuario novo = new Usuario()
            {
                Nome = model.Nome,
                Password = BCrypt.Net.BCrypt.HashPassword(model.Password),
                Perfil = model.Perfil
            };



            _context.Usuarios.Add(novo);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = novo.UsuarioId }, novo);
        }



        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Usuarios
            .FirstOrDefaultAsync(c => c.UsuarioId == id);
            if (model == null)
                return NotFound(new { messagem = "Nenhum Usuario encontrado" });

            // GerarLinks(model);
            return Ok(model);
        }




        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, UsuarioDto model)
        {
            if (id != model.UsuarioId)
                return BadRequest(new { mensagem = $"ID do Usuario não confere com o parâmetro da URL." });

            var modeloDb = await _context.Usuarios.AsNoTracking().FirstOrDefaultAsync(c => c.UsuarioId == id);

            if (modeloDb == null)
                return NotFound(new { mensagem = $"Usuario com id = {id} não encontrado." });

            // Atualiza as propriedades individualmente
            modeloDb.Nome = model.Nome;
            modeloDb.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            modeloDb.Perfil = model.Perfil;


            // Adiciona outras propriedades conforme necessário
            _context.Usuarios.Update(modeloDb);
            await _context.SaveChangesAsync();
            return NoContent();
        }



        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Usuario = await _context.Usuarios.FirstOrDefaultAsync(v => v.UsuarioId == id);

            if (Usuario == null)
                return NotFound(new { mensagem = $"Veículo com id = {id} não encontrado." });

            _context.Usuarios.Remove(Usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        [AllowAnonymous]
        [HttpPost("authenticate")]
        public async Task<ActionResult> Authenticate(AuthenticateDto model)
        {
            var usuarioDb = await _context.Usuarios.FindAsync(model.AuthenticateDtoId);

            if (usuarioDb == null || !BCrypt.Net.BCrypt.Verify(model.Password, usuarioDb.Password))
                return Unauthorized(new { messagem = "Sem Autorização para acessar." });

            var jwt = GenerateJwtToken(usuarioDb);

            return Ok(new { jwtToken = jwt });
        }


        private string GenerateJwtToken(Usuario model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes("Ry74cBQva5dThwbwchR9jhbtRFnJxWSZ");
            var claims = new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, model.UsuarioId.ToString()),
                new Claim(ClaimTypes.Role, model.Perfil.ToString())
            });

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddHours(8),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }


        // private void GerarLinks(Usuario model)
        // {
        //     model.Links.Add(new DTOS.LinkDto(model.UsuarioId, Url.ActionLink(), rel: "self", metodo: "GET"));
        //     model.Links.Add(new DTOS.LinkDto(model.UsuarioId, Url.ActionLink(), rel: "update", metodo: "PUT"));
        //     model.Links.Add(new DTOS.LinkDto(model.UsuarioId, Url.ActionLink(), rel: "delete", metodo: "DELETE"));
        // }


    }
}