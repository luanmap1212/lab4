namespace lab4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Following")]
    public partial class Following
    {
        [Key]
        [Column(Order = 0)]
        public string FollowerID { get; set; }

        [Key]
        [Column(Order = 1)]
        public string FolloweeID { get; set; }
    }
}
