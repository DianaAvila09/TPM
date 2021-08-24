namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CatDepartamentos
    {
        public int Id { get; set; }

        [StringLength(4)]
        public string PlantaSatelite { get; set; }

        [StringLength(10)]
        public string CodDepartamento { get; set; }

        [StringLength(30)]
        public string Descrip { get; set; }

        [StringLength(10)]
        public string CentroCostos { get; set; }

        public bool? Estatus { get; set; }
    }
}
