using System.ComponentModel.DataAnnotations.Schema;

namespace Models.Db
{
    public class OrderIngredient
    {
        public long Id { get; set; }

        public string Title { get; set; }

        public int Weight { get; set; }

        // TODO: Price for 100g, or 1kg
        // public float Price { get; set; }

        [ForeignKey(nameof(Product))]
        public long ProductId { get; set; }

        public virtual OrderProduct Product { get; set; }
    }
}