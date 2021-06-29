using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
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

        [Required(ErrorMessage = "O campo Nome deve ser preenchido")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo CPF deve ser preenchido")]
        public string CPF { get; set; }

        public string Descricao { get; set; }

        [Required(ErrorMessage = "O campo Valor deve ser preenchido")]
        [DataType(DataType.Currency)]
        public float Valor { get; set; }

        public Status Status { get; set; }

        [NotMapped]
        public IEnumerable<Venda> ListagemVenda { get; set; }


    }
}