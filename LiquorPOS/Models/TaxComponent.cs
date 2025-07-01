using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace LiquorPOS.Models;

public class TaxComponent
{
    public int ComponentId { get; set; }
    public string Name { get; set; } = null!;
    public decimal Rate { get; set; }

    public ICollection<ProductTaxComponent> ProductLinks { get; set; } = [];
}