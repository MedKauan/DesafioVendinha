using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace DesafioVendinha.Models
{
    public enum Status
    {
        Pendente, Pago
    }

    public class Venda
    {
        public int VendaID { get; set; }
        public string Nome { get; set; }
        public string CPF { get; set; }
        public string Descricao { get; set; }
        public float Valor { get; set; }
        public Status Status { get; set; }

        [NotMapped]
        public IEnumerable<Venda> ListagemVenda { get; set; }


    }
}