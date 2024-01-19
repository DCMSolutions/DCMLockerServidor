using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DCMLockerServidor.Shared
{
    public class EmpresaOld
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El nombre es obligatorio.")]
        public string? Nombre { get; set; }
        public string? Descripcion { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public string? CUIT { get; set; }
        public string? NombreContacto { get; set; }

    }
}
