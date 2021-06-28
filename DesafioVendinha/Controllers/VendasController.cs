using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioVendinha.Data;
using DesafioVendinha.Models;
using System.Net;
using System;

namespace DesafioVendinha.Controllers
{
    public class VendasController : Controller
    {
        private readonly VendaContexto _context;

        public VendasController(VendaContexto context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> MudaStatus(int id)
        {
            var atualizarVenda = await _context.Vendas.FirstOrDefaultAsync(s => s.VendaID == id);

            atualizarVenda.Status = Status.Pago;
            _context.Vendas.Update(atualizarVenda);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // GET: Clientes
        public async Task<IActionResult> Index()
        {
            var clientes = new Venda();
            clientes.ListagemVenda = await _context.Vendas.ToListAsync();
            ViewBag.TotalPedidos = clientes.ListagemVenda.Sum(w => w.Valor);

            return View(clientes);
        }

        // GET: Clientes/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Vendas
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.VendaID == id);

            if (cliente == null)

            {
                return NotFound();
            }

            return View(cliente);
        }

        // GET: Clientes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Clientes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venda cliente)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    cliente.Status = Status.Pendente;
                    _context.Add(cliente);
                    await _context.SaveChangesAsync();

                    return RedirectToAction("Index");
                }
            }
            catch (DbUpdateException /* ex */)
            {
                //Logar o erro (descomente a variável ex e escreva um log
                ModelState.AddModelError("", "Não foi possível salvar. " +
                    "Tente novamente, e se o problema persistir " +
                    "chame o suporte.");
            }
            return View(cliente);
        }

        // GET: Clientes/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Vendas.FindAsync(id);
            if (cliente == null)
            {
                return NotFound();
            }
            return View(cliente);
        }

        // POST: Clientes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var atualizarEstudante = await _context.Vendas.SingleOrDefaultAsync(s => s.Valor == id);
            if (await TryUpdateModelAsync<Venda>(
                atualizarEstudante,
                "",
                s => s.Nome, s => s.CPF, s => s.Descricao, s => s.Valor))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction("Index");
                }
                catch (DbUpdateException /* ex */)
                {
                    //Logar o erro (descomente a variável ex e escreva um log
                    ModelState.AddModelError("", "Não foi possível salvar. " +
                        "Tente novamente, e se o problema persistir " +
                        "chame o suporte.");
                }
            }
            return View(atualizarEstudante);
        }

        // GET: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var cliente = await _context.Vendas
                .FirstOrDefaultAsync(m => m.VendaID == id);
            if (cliente == null)
            {
                return NotFound();
            }

            return View(cliente);
        }

        // POST: Clientes/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }
            var estudante = await _context.Vendas
                .AsNoTracking()
                .SingleOrDefaultAsync(m => m.VendaID == id);

            if (estudante == null)
            {
                return NotFound();
            }
            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "A exclusão falhou. Tente novamente e se o problema persistir " +
                    "contate o suporte.";
            }
            return View(estudante);
        }
    }
}
