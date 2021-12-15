namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class order
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public order()
        {
            orderedItems = new HashSet<orderedItem>();
        }

        public int orderId { get; set; }

        public int userId { get; set; }

        public double totalPrice { get; set; }

        public DateTime dateAndTime { get; set; }

        [Required]
        [StringLength(1000)]
        public string address { get; set; }

        public int status { get; set; }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<orderedItem> orderedItems { get; set; }

        public virtual user user { get; set; }
    }
}
