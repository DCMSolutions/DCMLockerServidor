using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLockerServidor.Shared.Models
{
    public class EmpresaUrl
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Url { get; set; } = string.Empty;

        [Required]
        public int IdEmpresa { get; set; }

        [ForeignKey("IdEmpresa")]
        public Empresa? Empresa { get; set; }
    }
}
