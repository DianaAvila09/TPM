namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CatClasifFallas
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int IdClasif { get; set; }

        [StringLength(5)]
        public string CodClasif { get; set; }

        [StringLength(35)]
        public string Descripcion { get; set; }

        public bool? Status { get; set; }
    }
}
