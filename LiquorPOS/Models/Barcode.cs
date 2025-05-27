using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorPOS.Models
{
    [Table("barcodes")]
    public class Barcode
    {
        [Key]
        [Column("barcode")]
        public string BarcodeValue { get; set; } // Renamed to avoid conflict with class name

        [Column("code_num")]
        public string CodeNum { get; set; }

        [Column("qty")]
        public int? Quantity { get; set; }

        // Navigation property: Links back to the Product
        [ForeignKey("CodeNum")]
        public virtual Product Product { get; set; }
    }
}