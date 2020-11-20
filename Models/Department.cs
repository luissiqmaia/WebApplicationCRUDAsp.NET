using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationCRUD.Models {
    public class Department {
        public int Id { get; set; }


        [Display(Name = "Nome do departamento")]
        [Required(ErrorMessage = "Nome do departamento é requerido")]
        [StringLength(30, MinimumLength = 3, ErrorMessage = "O nome deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }



        public ICollection<Seller> Sellers { get; set; } = new List<Seller>();

        public Department() { }

        public Department(int id, string name) {
            Id = id;
            Name = name;
        }

        public void AddSeller(Seller seller) {
            Sellers.Add(seller);
        }

        public double TotalSales(DateTime initial, DateTime final) {
            return Sellers.Sum(seller => seller.TotalSales(initial, final));
        }
    }
}
