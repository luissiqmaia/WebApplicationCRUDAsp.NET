using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace WebApplicationCRUD.Models {
    public class Seller {
        public int Id { get; set; }


        [Display(Name = "Nome"), Required(ErrorMessage = "O nome é requerido")]
        [StringLength(60, MinimumLength = 6, ErrorMessage = "O nome deve conter entre {2} e {1} caracteres")]
        public string Name { get; set; }


        [Required(ErrorMessage = "O e-mail é requerido")]
        [Display(Name = "E-mail"), DataType(DataType.EmailAddress)]
        [EmailAddress(ErrorMessage = "Informe um e-mail válido")]
        public string Email { get; set; }


        [Required(ErrorMessage = "A data de nascimento é requerida")]
        [Display(Name = "Nascimento"), DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy}")]
        public DateTime birthDate { get; set; }


        [Required(ErrorMessage = "O salário base é requerido")]
        [Display(Name = "Salário base"), DisplayFormat(DataFormatString = "R$ {0:F2}")]
        [Range(1000.00, 50000.00, ErrorMessage = "Informe um salário maior que {1:F2} e menor {2:F2}")]
        public double baseSalary { get; set; }


        [Display(Name = "Departamentos")]
        public Department Department { get; set; }

        [Required(ErrorMessage = "A identificação do depertamento é requerida")]
        [Display(Name = "Identificação do Departamento")]
        public int DepartmentId { get; set; }


        [Display(Name = "Vendedores")]
        public ICollection<SalesRecord> Sales { get; set; } = new List<SalesRecord>();


        public Seller() { }

        public Seller(int id, string name, string email, DateTime birthDate, double baseSalary, Department department) {
            Id = id;
            Name = name;
            Email = email;
            this.birthDate = birthDate;
            this.baseSalary = baseSalary;
            Department = department;
        }

        public void AddSales(SalesRecord sr) {
            Sales.Add(sr);
        }

        public void RemoveSales(SalesRecord sr) {
            Sales.Remove(sr);
        }

        public double TotalSales(DateTime initial, DateTime final) {
            return Sales.Where(sr => sr.Date >= initial && sr.Date <= final)
                .Sum(sr => sr.Amount);
        }
    }
}
