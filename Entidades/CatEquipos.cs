namespace Entidades
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class CatEquipos
    {
        [Key]
        [Column(Order = 0)]
        [StringLength(18)]
        public string CodEquipo { get; set; }

        [StringLength(2)]
        public string LengEqui { get; set; }

        [StringLength(4)]
        public string TecObjAutGrp { get; set; }

        [StringLength(1)]
        public string CategoryEqui { get; set; }

        [StringLength(10)]
        public string TypeTechObj { get; set; }

        [StringLength(30)]
        public string ManufAsset { get; set; }

        [StringLength(20)]
        public string ManufModelNum { get; set; }

        [StringLength(10)]
        public string ConsecutiveNum { get; set; }

        [Key]
        [Column(Order = 1)]
        [StringLength(22)]
        public string ObjectNumber { get; set; }

        [StringLength(12)]
        public string MaintenancePlan { get; set; }

        [StringLength(12)]
        public string MeasuringPoint { get; set; }

        [StringLength(40)]
        public string DescripTechnical { get; set; }

        [StringLength(1)]
        public string StatusInactive { get; set; }

        [StringLength(5)]
        public string SystemStatus { get; set; }

        [StringLength(2)]
        public string LengDescrip { get; set; }

        [StringLength(10)]
        public string IndivStatusObject { get; set; }

        [StringLength(40)]
        public string ObjectStatus { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidDate { get; set; }

        [StringLength(10)]
        public string NumNextEquipUsage { get; set; }

        [StringLength(10)]
        public string MaintPlanningPlant { get; set; }

        [Column(TypeName = "date")]
        public DateTime? ValidFromDate { get; set; }

        [StringLength(18)]
        public string Superordinate { get; set; }

        [StringLength(10)]
        public string PlannerGroup { get; set; }

        [StringLength(2)]
        public string PmObjType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? IdWorkCenter { get; set; }

        [StringLength(25)]
        public string TechIdentNumber { get; set; }

        [StringLength(12)]
        public string AccountAssignment { get; set; }

        [StringLength(9)]
        public string CatalogProfile { get; set; }

        [StringLength(30)]
        public string TechnicalInformation { get; set; }

        [StringLength(40)]
        public string FunctionalLocation { get; set; }

        [StringLength(2)]
        public string CrObjType { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? ObjectIdPPWorkCenter { get; set; }

        [StringLength(4)]
        public string ControllingArea { get; set; }

        [StringLength(10)]
        public string CostCenter { get; set; }

        [StringLength(8)]
        public string MainWorkCenter { get; set; }

        [StringLength(4)]
        public string MainWCCategory { get; set; }

        [StringLength(10)]
        public string MainWCLocation { get; set; }

        [StringLength(3)]
        public string PersonResponsibleMWC { get; set; }

        [StringLength(7)]
        public string StandardTextKeyMWC { get; set; }

        [StringLength(4)]
        public string StandardValueKeyMWC { get; set; }

        [StringLength(3)]
        public string KeyPerformanceEfficRateMWC { get; set; }

        [StringLength(8)]
        public string WorkCenter { get; set; }

        [StringLength(4)]
        public string CategoryWorkCenter { get; set; }

        [StringLength(10)]
        public string LocationWorkCenter { get; set; }

        [StringLength(3)]
        public string PersonResponsibleWC { get; set; }

        [StringLength(7)]
        public string StandardTextKeyWC { get; set; }

        [StringLength(4)]
        public string StandardValueKeyWC { get; set; }

        [StringLength(3)]
        public string KeyPerformanceEfficRateWC { get; set; }

        [StringLength(50)]
        public string ObjectTypesResource { get; set; }

        public DateTime? FecUpdate { get; set; }

        public int? id { get; set; }
    }
}
