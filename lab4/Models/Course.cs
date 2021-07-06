namespace lab4.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Course")]
    public partial class Course
    {
        public int Id { get; set; }

        [Required]
        [StringLength(128)]
        public string LecturerId { get; set; }

        [Required]
        [StringLength(255)]
        public string Place { get; set; }

        public DateTime Datetime { get; set; }

        public int CategoryId { get; set; }
        public string Name = "";

        //public virtual Category Category { get; set; }

        //add List Category
        public List<Category> ListCategory = new List<Category>();
    }
}
