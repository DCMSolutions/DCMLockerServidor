using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLockerServidor.Shared
{
    public class Local
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "La empresa es obligatoria.")]
        [Range(1, int.MaxValue, ErrorMessage = "La empresa es obligatoria.")]
        public int IdEmpresa { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string Nombre { get; set; }
        public string Email { get; set; }
        public string Direccion { get; set; }
        public string Telefono { get; set; }
    }
}
