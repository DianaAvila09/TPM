using System;
using System.ComponentModel.DataAnnotations;


namespace Entidades
{
    public class Componente
    {
        public int IdComponente { get; set; }

        [StringLength(100, ErrorMessage = "Máximo de 100 caracteres")]
        [Required(ErrorMessage = "Escriba una descripción")]
        public string DescripCompo { get; set; }
        public string CodDepartamento { get; set; }
        public string DescripDepto { get; set; }
        public string CodSistema { get; set; }
        public string DescripSist { get; set; }
        public bool StatusCompo { get; set; }
        public string Usuario { get; set; }
        public DateTime FchAlta { get; set; }
        public DateTime FchModif { get; set; }
        public string CentroCostos { get; set; }
        public string WorkCenter { get; set; }

    }
}
