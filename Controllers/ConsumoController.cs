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
    public class ConsumoController : ControllerBase
    {
        private readonly AppDbContext _context;


        public ConsumoController(AppDbContext context)
        {
            _context = context;
        }



        [HttpGet]
        public async Task<ActionResult> GetAll()
        {
            throw new NotImplementedException();

            /*
            var model = await _context.Consumos.ToListAsync();
            return Ok(model);
            */
        }



        [HttpPost]
        public async Task<ActionResult> Create(Consumo model)
        {

            _context.Consumos.Add(model);
            await _context.SaveChangesAsync();
            return CreatedAtAction("GetById", new { id = model.ConsumoId }, model);

        }


        [HttpGet("{id}")]
        public async Task<ActionResult> GetById(int id)
        {
            var model = await _context.Consumos
            .FirstOrDefaultAsync(c => c.ConsumoId == id);
            if (model == null)
                return NotFound(new { messagem = "Nenhum Consumo encontrado" });

            GerarLinks(model);
            return Ok(model);
        }



        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, Consumo model)
        {
            if (id != model.ConsumoId)
                return BadRequest(new { mensagem = $"ID do veículo não confere com o parâmetro da URL." });

            var modeloDb = await _context.Consumos.FirstOrDefaultAsync(c => c.ConsumoId == id);

            if (modeloDb == null)
                return NotFound(new { mensagem = $"Veículo com id = {id} não encontrado." });

            // Atualiza as propriedades individualmente
            modeloDb.Descricao = model.Descricao;
            modeloDb.Data = model.Data;
            modeloDb.Valor = model.Valor;
            modeloDb.Tipo = model.Tipo;

            // Adicione outras propriedades conforme necessário

            await _context.SaveChangesAsync();
            return NoContent();
        }




        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            var Consumo = await _context.Consumos.FirstOrDefaultAsync(v => v.ConsumoId == id);

            if (Consumo == null)
                return NotFound(new { mensagem = $"Veículo com id = {id} não encontrado." });

            _context.Consumos.Remove(Consumo);
            await _context.SaveChangesAsync();

            return NoContent();
        }



        private void GerarLinks(Consumo model)
        {
            model.Links.Add(new DTOS.LinkDto(model.ConsumoId, Url.ActionLink(), rel: "self", metodo: "GET"));
            model.Links.Add(new DTOS.LinkDto(model.ConsumoId, Url.ActionLink(), rel: "update", metodo: "PUT"));
            model.Links.Add(new DTOS.LinkDto(model.ConsumoId, Url.ActionLink(), rel: "delete", metodo: "DELETE"));
        }

    }
}