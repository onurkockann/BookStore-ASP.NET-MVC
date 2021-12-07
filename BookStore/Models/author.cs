namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class author
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public author()
        {
            books = new HashSet<book>();
        }

        public int id { get; set; }

        [Required]
        [StringLength(100)]
        public string aName { get; set; }

        [StringLength(1200)]
        public string aDescription { get; set; }

        [StringLength(100)]
        public string aImage { get; set; }

        [StringLength(100)]
        public string aLink { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<book> books { get; set; }
    }
}
