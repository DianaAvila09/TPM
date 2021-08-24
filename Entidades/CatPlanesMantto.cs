namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("CatPlanesMantto")]
    public partial class CatPlanesMantto
    {
        public int Id { get; set; }

        [StringLength(18)]
        public string CodEquipo { get; set; }

        [StringLength(10)]
        public string CodSistema { get; set; }

        [StringLength(3)]
        public string CodCiclo { get; set; }

        public int? Frecuencia { get; set; }

        public DateTime? FechaAlta { get; set; }

        [StringLength(30)]
        public string UsuarioAlta { get; set; }

        public DateTime? FechaCancelacion { get; set; }

        [StringLength(30)]
        public string UsuarioCancelo { get; set; }

        public bool? Estatus { get; set; }

        public DateTime? FecUltEjecucion { get; set; }

        [StringLength(10)]
        public string CentroCostos { get; set; }
    }
}
