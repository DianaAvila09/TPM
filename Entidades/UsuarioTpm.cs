using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class UsuarioTpm
    {

        public int Id { get; set; }

        [Required(ErrorMessage = "Ingrese su num de control")]
        [Display(Name = "Núm. de control: ")]
        [Range(1, 99999, ErrorMessage = "Dato fuera de rango")]
        public string NumControl { get; set; }
        public string Nombre { get; set; }

        [Required(ErrorMessage = "Ingrese un password")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
        public bool StatusEmpTpm { get; set; }

        [Required(ErrorMessage = "Seleccione un centro de costos")]
        public string CentroCostos { get; set; }
        public string DesripCCostos { get; set; }

        [Required(ErrorMessage = "Seleccione un Roll para el usuario")]
        public string ClaveRol { get; set; }
        public string DespcripRol { get; set; }
        public bool EstatusRol { get; set; }
        public DateTime FecAlta { get; set; }
        public string Agrego { get; set; }
        public bool CatTpm { get; set; }
        public bool EditarTicket { get; set; }
        public bool CatChecklist { get; set; }
        public bool CapturaChecklist { get; set; }
        public List<UsuarioCtroCostos> UsuarioCtroCostos { get; set; }

    }
}
