namespace BookStore.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class creditCard
    {
        public int creditCardId { get; set; }

        [Column(TypeName = "numeric")]
        public decimal cardNumber { get; set; }

        public int cvv { get; set; }

        [Column(TypeName = "date")]
        public DateTime expireDate { get; set; }

        public int? balance { get; set; }

        public int userId { get; set; }

        public virtual user user { get; set; }
    }
}
