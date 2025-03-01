﻿
using System.ComponentModel.DataAnnotations.Schema;


namespace ProfileAss.Model
{
    [Table("basket")]
    public class Basket
    {
        public int Id { get; set; }

        // Link to Profile 
        public int ProfileId { get; set; }
        [ForeignKey("ProfileId")]
        public Profile Profile { get; set; }

        // Products in the basket 
        public List<BasketItem> BasketItems { get; set; } = new();
    }
}
