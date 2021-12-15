namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("cart")]
    public partial class cart
    {
        public int cartID { get; set; }

        [Column(TypeName = "numeric")]
        public decimal? bookID { get; set; }

        public int? userID { get; set; }

        public int? quantity { get; set; }

        public virtual book book { get; set; }

        public virtual user user { get; set; }
    }
}
