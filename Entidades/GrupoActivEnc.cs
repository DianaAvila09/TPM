using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class GrupoActivEnc
    {

        public int IdGrupoAct { get; set; }

        [StringLength(20, ErrorMessage = "Máximo de 10 caracteres")]
        [Required(ErrorMessage = "Escriba una clave")]
        public string CodGrupo { get; set; }


        [StringLength(100, ErrorMessage = "Máximo de 50 caracteres")]
        [Required(ErrorMessage = "Escriba una descripción")]
        public string DescripGpo { get; set; }
        public bool EqParado { get; set; }
        public bool Activo { get; set; }
        public string CodDepartamento { get; set; }
        public string UserAlta { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchAlta { get; set; }

        public string UserModif { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchModif { get; set; }
        public string Llave { get; set; }
        public string CentroCostos { get; set; }

    }
}
