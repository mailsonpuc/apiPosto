using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using puc.Context;
using puc.Models;

namespace puc.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/[controller]")]
#pragma warning disable CS1591
    public class VeiculoController : ControllerBase
    {
        private readonly AppDbContext _context;


        public VeiculoController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            var model = await _context.Veiculos.ToListAsync();
            return Ok(model);
        }



        [HttpPost]
        public async Task<ActionResult> Create(Veiculo model)
        {
            if (model.AnoFabricacao <= 0 || model.AnoModelo <= 0)
            {
                return BadRequest(new { message = "Ano de Fabricação e Ano do Modelo são obrigatorios e devem ser maiores do que zero." });
            }
            _context.Veiculos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = model.VeiculoId }, model);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Veiculos
            .Include(t => t.Usuarios).ThenInclude(t => t.Usuario)
            .Include(tabela => tabela.Consumos)
            .FirstOrDefaultAsync(c => c.VeiculoId == id);
            if (model == null)
                return NotFound(new { messagem = "Nenhum Veiculo encontrado" });

            GerarLinks(model);

            return Ok(model);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Veiculo model)
        {
            if (id != model.VeiculoId)
                return BadRequest(new { mensagem = $"ID do veículo não confere com o parâmetro da URL." });

            var modeloDb = await _context.Veiculos.FirstOrDefaultAsync(c => c.VeiculoId == id);

            if (modeloDb == null)
                return NotFound(new { mensagem = $"Veículo com id = {id} não encontrado." });

            // Atualiza as propriedades individualmente
            modeloDb.Placa = model.Placa;
            modeloDb.Marca = model.Marca;
            modeloDb.Modelo = model.Modelo;
            modeloDb.AnoFabricacao = model.AnoFabricacao;
            modeloDb.AnoModelo = model.AnoModelo;
            // Adicione outras propriedades conforme necessário

            await _context.SaveChangesAsync();
            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var veiculo = await _context.Veiculos.FirstOrDefaultAsync(v => v.VeiculoId == id);

            if (veiculo == null)
                return NotFound(new { mensagem = $"Veículo com id = {id} não encontrado." });

            _context.Veiculos.Remove(veiculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }




        private void GerarLinks(Veiculo model)
        {
            model.Links.Add(new DTOS.LinkDto(model.VeiculoId, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new DTOS.LinkDto(model.VeiculoId, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new DTOS.LinkDto(model.VeiculoId, Url.ActionLink(), rel: "delete", metodo: "DELETE"));
        }


        [HttpPost("{id}/usuarios")]
        public async Task<ActionResult> AddUsuario(int id, VeiculoUsuarios model)
        {
            if (id != model.VeiculoId)
            {
                return BadRequest();
            }

            _context.VeiculosUsuarios.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = model.VeiculoId }, model);
        }






        [HttpDelete("{id}/usuarios/{usuarioId}")]
        public async Task<ActionResult> DeleteUsuario(int id, int usuarioId)
        {
            var model = await _context.VeiculosUsuarios
            .Where(c => c.VeiculoId == id && c.UsuarioId == usuarioId)
            .FirstOrDefaultAsync();

            if (model is null)
                return NotFound();

            _context.VeiculosUsuarios.Remove(model);
            await _context.SaveChangesAsync();

            return NoContent();
        }

    }
}

