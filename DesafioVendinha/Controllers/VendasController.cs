using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioVendinha.Data;
using DesafioVendinha.Models;
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


        // GET: vendas
        public async Task<IActionResult> Index()
        {
            var vendas = new Venda();
            vendas.ListagemVenda = await _context.Vendas.OrderByDescending(o => o.Valor).ToListAsync();
            ViewBag.TotalPedidos = string.Format("{0:c}", vendas.ListagemVenda.Where(w => w.Status == Status.Pendente).Sum(w => w.Valor));

            return View(vendas);
        }

        // GET: vendas/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: vendas/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Venda venda)
        {
            venda.ListagemVenda = await _context.Vendas.ToListAsync();

            try
            {
                var count = venda.ListagemVenda.Where(w => w.CPF == venda.CPF && w.Status == Status.Pendente).Count();

                if(count == 0)
                {
                    if (ModelState.IsValid)
                    {
                        venda.Status = Status.Pendente;
                        _context.Add(venda);
                        await _context.SaveChangesAsync();

                        return RedirectToAction("Index");
                    }
                }
                else
                {
                    throw new System.Exception("Existe Divida");
                }
            }
            catch (Exception  ex)
            {
                ViewData["erro"] = ex.Message;
            }

            
            ViewBag.TotalPedidos = string.Format("{0:c}", venda.ListagemVenda.Where(w => w.Status == Status.Pendente).Sum(w => w.Valor));


            return View("Index", venda);
        }

        [HttpPost]
        public async Task<IActionResult> Delete(int id)
        {
            var deletaVenda = await _context.Vendas.FirstOrDefaultAsync(s => s.VendaID == id);
            _context.Vendas.Remove(deletaVenda);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }
    }
}
