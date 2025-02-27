
using System.ComponentModel.DataAnnotations.Schema;

namespace ProfileAss.Model
{
    [Table("person")] //Add this attribute
    public class Profile
    {

        public  int Id { get; set; }
        
        public string firstname { get; set; }

      
        public string lastname { get; set; }

   
        public string email { get; set; }

       
        public string bio { get; set; }



        // Change this to specify the inverse property
        public virtual ICollection<Basket> Baskets { get; set; } = new List<Basket>();

    }
}
