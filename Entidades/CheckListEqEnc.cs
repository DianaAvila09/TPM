using System;
using System.ComponentModel.DataAnnotations;

namespace Entidades
{
    public class CheckListEqEnc
    {

        public int IdChkEquipo { get; set; }
        public string ChkEquipo { get; set; }
        public int Planta { get; set; }
        public string CodDepartamento { get; set; }
        public string CentroCostos { get; set; }
        public string CodWorkCenter { get; set; }
        [Required(ErrorMessage = "Dato Obligatorio")]
        public string CodEquipo { get; set; }
        public string DescripEquipo { get; set; }

        [Required(ErrorMessage = "Dato Obligatorio")]
        public int IdCheckList { get; set; }
        public string CodChkList { get; set; }
        public string DescripChkList { get; set; }
        public string CodClasif { get; set; }
        public bool EqParado { get; set; }
        public bool Activo { get; set; }


        [Required(ErrorMessage = "Dato Obligatorio")]
        public int Frecuencia { get; set; }

        [Required(ErrorMessage = "Dato Obligatorio")]
        public string IdFrecuencia { get; set; }
        public string DesripFrencu { get; set; }
        public string UserAlta { get; set; }
        public DateTime FchAlta { get; set; }
        public string UserModif { get; set; }
        public DateTime FchModif { get; set; }
        public string UserActiva { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime IniProgram { get; set; }
        public string UserCancela { get; set; }
        public DateTime FchCancel { get; set; }


        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime? FchEjecucion { get; set; }
        public DateTime UltimaEjec { get; set; }
        public string UserEjecuto { get; set; }

        [DataType(DataType.MultilineText)]
        public string Observaciones { get; set; }
        public DateTime? FecProgramada { get; set; }
        public string Periodo { get; set; }

    }
}
