namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class orderedItem
    {
        [Key]
        public int orderedItemsId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal bookId { get; set; }

        public int orderId { get; set; }

        public int? bookQty { get; set; }

        public virtual book book { get; set; }

        public virtual order order { get; set; }
    }
}
