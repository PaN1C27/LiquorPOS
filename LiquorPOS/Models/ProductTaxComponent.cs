using LiquorPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class ProductTaxComponent
{
    public string CodeNum { get; set; } = null!;
    public int ComponentId { get; set; }

    public Product Product { get; set; } = null!;
    public TaxComponent TaxComponent { get; set; } = null!;
}