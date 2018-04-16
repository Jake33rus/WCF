namespace CommonLib.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Immovables:IId
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public int Id { get; set; }

        public int Type { get; set; }

        [Required]
        [StringLength(50)]
        public string Name { get; set; }

        public double Footage { get; set; }

        [Required]
        [StringLength(50)]
        public string Location { get; set; }

        public int Price { get; set; }

        public int? NumbRooms { get; set; }

        public int? NumbFloors { get; set; }

        public string Assigment { get; set; }

        public double? SizePlot { get; set; }

        public string TypeApart { get; set; }
    }
}
