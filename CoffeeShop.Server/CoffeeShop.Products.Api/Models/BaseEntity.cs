using System.ComponentModel.DataAnnotations;
using System.Runtime.Serialization;

namespace CoffeeShop.Products.Api.Models
{
    public class BaseEntity
    {
        [Key]
        public virtual int Id { get; set; }

        [DataMember]
        public DateTime CreatedAt { get; set; }

        [DataMember]
        public DateTime? UpdatedAt { get; set; }
    }
}
