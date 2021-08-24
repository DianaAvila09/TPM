namespace Entidades
{
    using System;
    using System.ComponentModel.DataAnnotations;

    public class Ticket
    {
        [Display(Name = "Id. Ticket")]
        public int IdTicket { get; set; }

        public string TipoTicket { get; set; }

        [Display(Name = "Planta")]
        public int IdPlanta { get; set; }

        public string Planta { get; set; }


        [StringLength(10)]
        public string CodDepartamento { get; set; }

        [StringLength(35)]
        public string Depto { get; set; }

        [StringLength(10)]
        public string CodWorkCenter { get; set; }

        [StringLength(18)]
        public string CodEquipo { get; set; }

        [StringLength(18)]
        [Required(ErrorMessage = "Seleccione un equipo")]
        public string CodSubEquipo { get; set; }

        public string SubEquipo { get; set; }

        [StringLength(40)]
        [DataType(DataType.MultilineText)]
        public string CodEquipoDescrip { get; set; }

        [DataType(DataType.MultilineText)]
        [Required(ErrorMessage = "Describa la falla")]
        public string FallaReportada { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchReporte { get; set; }

        [StringLength(5, ErrorMessage = "Máximo de 5 numeros")]
        [Required(ErrorMessage = "Escriba su num. de control")]
        public string UsuarioReporto { get; set; }

        public string NombreReporto { get; set; }

        [DataType(DataType.MultilineText)]
        public string Diagnostico { get; set; }

        [StringLength(5)]
        public string CodClasif { get; set; }

        public string ClasifDescrip { get; set; }

        [StringLength(10)]
        public string CodSistema { get; set; }

        public string CodSistemaDescrip { get; set; }

        [StringLength(10)]
        public string CodFalla { get; set; }

        public string CodFallaDescrip { get; set; }

        [StringLength(10)]
        public string Falla { get; set; }

        [StringLength(5)]
        public string CodTipoFalla { get; set; }

        public string CodTipoFallaDescrip { get; set; }

        [StringLength(2)]
        public string CausoParo { get; set; }

        public string HallazgoSeguridad { get; set; }

        public int TiempoReparacion { get; set; }

        [StringLength(3)]
        public string UnidadTiempoRep { get; set; }

        public string DescripcionUT { get; set; }

        [StringLength(30)]
        public string UsrAtendio { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchAtendido { get; set; }

        [StringLength(5)]
        public string CodStatus { get; set; }

        public string CodStatusDescrip { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FecStatus { get; set; }


        [DataType(DataType.MultilineText)]
        public string AccionPlan { get; set; }


        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchAccionplan { get; set; }

        [StringLength(10)]
        public string WorkerAsignado { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FecIniReparacion { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FecEntgregaReparacion { get; set; }
        public string UsrEntrReparacion { get; set; }

        [StringLength(30)]
        public string UsrAsigno { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchAsignacion { get; set; }

        [Key]
        [StringLength(15)]
        public string WOM { get; set; }
        [StringLength(10)]

        public string CodTipoWom { get; set; }

        public string CodTipoWomDescrip { get; set; }

        [StringLength(30)]
        public string UsrAsignoWom { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchWom { get; set; }

        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchaPromesa { get; set; }


        [DisplayFormat(DataFormatString = "{0: dd/MM/yyyy HH:mm tt}", ApplyFormatInEditMode = true)]
        public DateTime FchClose { get; set; }

        [StringLength(30)]
        public string UsrCerroTicket { get; set; }

        [DataType(DataType.MultilineText)]
        public string Notas { get; set; }

        [StringLength(30)]
        public string PCReporto { get; set; }

        [StringLength(10)]
        public string Sensor { get; set; }

        [StringLength(10)]
        public string CentroCostos { get; set; }
    }
}
