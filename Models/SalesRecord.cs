using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using WebApplicationCRUD.Models.Enums;

namespace WebApplicationCRUD.Models {
    public class SalesRecord {
        public int Id { get; set; }

        [Required(ErrorMessage = "A data é requerida")]
        [Display(Name = "Data"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime Date { get; set; }

        [Required(ErrorMessage = "O valor é requerido")]
        [Display(Name = "Valor"), DisplayFormat(DataFormatString = "R$ {0:F2}")]
        [Range(1000.00, 50000.00, ErrorMessage = "Informe um valor que {1:F2} e menor {2:F2}")]
        public double Amount { get; set; }

        [Required(ErrorMessage = "O status é requerido")]
        [Display(Name = "Status da venda")]
        public SaleStatus Status { get; set; }
        public Seller Seller { get; set; }

        public SalesRecord() { }

        public SalesRecord(int id, DateTime date, double amount, SaleStatus status, Seller seller) {
            Id = id;
            Date = date;
            Amount = amount;
            Status = status;
            Seller = seller;
        }
    }
}
