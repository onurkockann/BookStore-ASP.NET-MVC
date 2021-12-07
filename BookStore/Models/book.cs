namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class book
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public book()
        {
            orderedItems = new HashSet<orderedItem>();
        }

        [Key]
        [Column(TypeName = "numeric")]
        public decimal isbn { get; set; }

        [Required]
        [StringLength(50)]
        public string bName { get; set; }

        public int author { get; set; }

        public int genre { get; set; }

        public int publisher { get; set; }

        [StringLength(2500)]
        public string detail { get; set; }

        [StringLength(100)]
        public string bImage { get; set; }

        [Column(TypeName = "date")]
        public DateTime? bDate { get; set; }

        public double? price { get; set; }

        public int? stock { get; set; }

        public int? bodyCount { get; set; }

        public virtual author author1 { get; set; }

        public virtual genre genre1 { get; set; }

        public virtual publisher publisher1 { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderedItem> orderedItems { get; set; }
    }
}
