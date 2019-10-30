using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Factory
{
    [Serializable]
    public class Product
    {
        public int Id { get; private set; }
        public string Name { get; set; }
        public int ProduceCost { get; set; }
        public int Cost { get; set; }

        public Product(int id, string name, int cost, int produceCost)
        {
            Id = id;
            Name = name;
            Cost = cost;
            ProduceCost = produceCost;
        }
    }
}
