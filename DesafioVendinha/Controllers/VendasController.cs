using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using DesafioVendinha.Data;
using DesafioVendinha.Models;

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
            vendas.ListagemVenda = await _context.Vendas.ToListAsync();
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
            try
            {
                if (ModelState.IsValid)
                {
                    venda.Status = Status.Pendente;
                    _context.Add(venda);
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

            venda.ListagemVenda = await _context.Vendas.ToListAsync();
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
