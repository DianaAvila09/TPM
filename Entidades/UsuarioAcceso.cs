using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidades
{
    public class UsuarioAcceso
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Dato Obligatorio, ingrese su num de control")]
        [Range(1, 99999, ErrorMessage = "Dato fuera de rango")]
        public string NumControl { get; set; }
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Dato Obligatorio")]
        [Display(Name = "Password: ")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool CatTpm { get; set; }
        public bool EditarTicket { get; set; }
        public bool CatChecklist { get; set; }
        public bool CapturaChecklist { get; set; }
        public string cCostos { get; set; }
    }
}
