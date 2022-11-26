using Microsoft.AspNetCore.Mvc;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
  [ApiController]
  [Route("[controller]")]
  public class TarefaController : ControllerBase
  {
    private readonly OrganizadorContext _context;

    public TarefaController(OrganizadorContext context)
    {
      _context = context;
    }

    #region Obter Tarefa or ID

    [HttpGet("{id}")]
    public IActionResult ObterPorId(int id)
    {
      var task = _context.Tarefas.Find(id); //Buscando tarefa

      //Verificando se tarefa 
      if (task is null)
      {
        return NotFound("Id não existente");
      }
      else
      {
        return Ok(task);
      }
    }
    #endregion

    #region Obter todas as tarefas
    [HttpGet("ObterTodos")]
    public IActionResult ObterTodos()
    {
      // TODO: Buscar todas as tarefas no banco utilizando o EF
      var task = _context.Tarefas.ToList();
      return Ok(task);
    }
    #endregion

    #region Obter tarefa por titulo
    [HttpGet("ObterPorTitulo")]
    public IActionResult ObterPorTitulo(string titulo)
    {
      var task = _context
        .Tarefas
        .Where(t => t.Titulo.Equals(titulo));

      return Ok(task);
    }
    #endregion

    #region Obter tarefa por data
    [HttpGet("ObterPorData")]
    public IActionResult ObterPorData(DateTime data)
    {
      var tarefa = _context.Tarefas.Where(x => x.Data.Date == data.Date);
      return Ok(tarefa);
    }
    #endregion

    #region Obter tarefa por status
    [HttpGet("ObterPorStatus")]
    public IActionResult ObterPorStatus(EnumStatusTarefa status)
    {
      // TODO: Buscar  as tarefas no banco utilizando o EF, que contenha o status recebido por parâmetro
      // Dica: Usar como exemplo o endpoint ObterPorData
      var tarefa = _context.Tarefas.Where(x => x.Status == status);
      return Ok(tarefa);
    }
    #endregion

    #region Criar tarefa
    [HttpPost]
    public IActionResult Criar(Tarefa tarefa)
    {
      if (tarefa.Data == DateTime.MinValue)
      {
        return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
      }
      else
      {
        if (ModelState.IsValid)
          _context.Tarefas.Add(tarefa);
        _context.SaveChanges();

        return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
      }

    }
    #endregion

    #region Atualizar tarefa
    [HttpPut("{id}")]
    public IActionResult Atualizar(int id, Tarefa tarefa)
    {
      var tarefaBanco = _context.Tarefas.Find(id);

      if (tarefaBanco is null)
        return NotFound("Id não pode ser nulo. Contate o administrador.");

      if (tarefa.Data == DateTime.MinValue)
        return BadRequest(new { Erro = "A data da tarefa não pode ser vazia. Contate o aministrador." });

      // TODO: Atualizar as informações da variável tarefaBanco com a tarefa recebida via parâmetro
      // TODO: Atualizar a variável tarefaBanco no EF e salvar as mudanças (save changes)
      if (tarefa.Id.Equals(tarefaBanco.Id))
      {
        tarefaBanco.Id = tarefa.Id;
        tarefaBanco.Descricao = tarefa.Descricao;
        tarefaBanco.Data = tarefa.Data;
        tarefaBanco.Status = tarefa.Status;
        tarefaBanco.Titulo = tarefa.Titulo;

        _context.Update(tarefaBanco);
        _context.SaveChanges();
      }
      else
      {
        return NotFound("Id divergente. Por favor contate o administrador.");
      }

      return Ok(tarefaBanco);
    }
    #endregion

    #region Deletar tarefa
    [HttpDelete("{id}")]
    public IActionResult Deletar(int id)
    {
      var tarefaBanco = _context.Tarefas.Find(id);

      if (tarefaBanco == null)
        return NotFound();

      // TODO: Remover a tarefa encontrada através do EF e salvar as mudanças (save changes)
      if (tarefaBanco.Id.Equals(id))
      {
        _context.Tarefas.Remove(tarefaBanco);
        _context.SaveChanges();
      }

      return NoContent();
    }
    #endregion
  }
}
