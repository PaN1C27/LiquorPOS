using System; // Needed for DateTime
using System.Collections.Generic; // Needed for ICollection
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LiquorPOS.Models
{
    [Table("products")]
    public class Product
    {
        [Key]
        [Column("code_num")]
        public string? CodeNum { get; set; } // Assuming C;8;0 -> VARCHAR(8)

        [Column("brand")]
        public string? Brand { get; set; } // C;30;0

        [Column("descrip")]
        public string? Description { get; set; } // C;35;0 -> Renamed 'Descrip'

        [Column("type")]
        public string? Type { get; set; } // C;2;0

        [Column("size")]
        public string? Size { get; set; } // C;5;0

        [Column("cost")]
        public decimal? Cost { get; set; } // N;12;4

        [Column("last_cost")]
        public decimal? LastCost { get; set; } // N;12;4

        [Column("next_cost")]
        public decimal? NextCost { get; set; } // N;12;4

        [Column("cost_2")]
        public decimal? Cost2 { get; set; } // N;12;4

        [Column("cost_3")]
        public decimal? Cost3 { get; set; } // N;12;4

        [Column("price")]
        public decimal? Price { get; set; } // N;12;4

        [Column("min_price")]
        public decimal? MinPrice { get; set; } // N;12;4

        [Column("qty_on_hnd")]
        public int? QtyOnHnd { get; set; } // N;6;0

        [Column("warehouse1")]
        public int? Warehouse1 { get; set; } // N;6;0

        [Column("warehouse2")]
        public int? Warehouse2 { get; set; } // N;6;0

        [Column("qty_on_rsv")]
        public int? QtyOnRsv { get; set; } // N;6;0

        [Column("qty_on_ord")]
        public int? QtyOnOrd { get; set; } // N;6;0

        [Column("qty_case")]
        public int? QtyCase { get; set; } // N;6;0

        [Column("dist")]
        public string? Dist { get; set; } // C;15;0

        [Column("divi")]
        public string? Divi { get; set; } // C;15;0

        [Column("salesman")]
        public string? Salesman { get; set; } // C;20;0

        [Column("last_ord")]
        public DateTime? LastOrd { get; set; } // D;8;0

        [Column("last_rcv")]
        public DateTime? LastRcv { get; set; } // D;8;0

        [Column("cost_date")]
        public DateTime? CostDate { get; set; } // D;8;0

        [Column("sales")]
        public int? Sales { get; set; } // N;7;0

        [Column("p")]
        public bool? P { get; set; } // L;1;0

        [Column("p_price")]
        public decimal? PPrice { get; set; } // N;12;4

        [Column("reorder")]
        public int? Reorder { get; set; } // N;4;0

        [Column("last_edit")]
        public DateTime? LastEdit { get; set; } // D;8;0

        [Column("last_sale")]
        public DateTime? LastSale { get; set; } // D;8;0

        [Column("priory")]
        public int? Priory { get; set; } // N;8;0

        [Column("mtd")]
        public int? Mtd { get; set; } // N;6;0

        [Column("ytd")]
        public int? Ytd { get; set; } // N;8;0

        [Column("taxable")]
        public string? Taxable { get; set; } // C;1;0

        [Column("start_prom")]
        public DateTime? StartProm { get; set; } // D;8;0

        [Column("end_prom")]
        public DateTime? EndProm { get; set; } // D;8;0

        [Column("bin")]
        public string? Bin { get; set; } // C;6;0

        [Column("case_price")]
        public decimal? CasePrice { get; set; } // N;10;2

        [Column("std_qty")]
        public int? StdQty { get; set; } // N;2;0

        [Column("print_bar")]
        public string? PrintBar { get; set; } // C;1;0

        [Column("taxrate")]
        public decimal? TaxRate { get; set; } // N;6;4 -> Renamed

        [Column("taxable2")]
        public string? Taxable2 { get; set; } // C;1;0

        [Column("taxrate2")]
        public decimal? TaxRate2 { get; set; } // N;6;4 -> Renamed

        [Column("script_yn")]
        public string? ScriptYn { get; set; } // C;1;0

        [Column("flat_tax")]
        public decimal? FlatTax { get; set; } // N;8;4

        [Column("shelf_cap")]
        public int? ShelfCap { get; set; } // N;4;0

        [Column("qty_shelf")]
        public int? QtyShelf { get; set; } // N;4;0

        [Column("intnum")]
        public string? IntNum { get; set; } // C;10;0

        [Column("class")]
        public string? Class { get; set; } // C;1;0

        [Column("vendnum")]
        public string? VendNum { get; set; } // C;7;0

        [Column("par_qty")]
        public int? ParQty { get; set; } // N;6;0

        [Column("dep_b")]
        public decimal? DepB { get; set; } // N;7;3

        [Column("dep_c")]
        public decimal? DepC { get; set; } // N;7;3

        [Column("user1")]
        public string? User1 { get; set; } // C;7;0

        [Column("user2")]
        public string? User2 { get; set; } // C;7;0

        [Column("disc_ok")]
        public string? DiscOk { get; set; } // C;1;0

        [Column("order_lot")]
        public string? OrderLot { get; set; } // C;1;0

        [Column("dep_ord")]
        public string? DepOrd { get; set; } // C;1;0

        [Column("dep_ordamt")]
        public decimal? DepOrdAmt { get; set; } // N;7;3 -> Renamed

        [Column("notes")]
        public string? Notes { get; set; } // C;19;0

        [Column("note2")]
        public string? Note2 { get; set; } // C;19;0

        [Column("discpool")]
        public int? DiscPool { get; set; } // N;7;0 -> Renamed

        [Column("bmp")]
        public string? Bmp { get; set; } // M;10;0 -> TEXT -> string

        [Column("hocode")]
        public string? HoCode { get; set; } // C;8;0 -> Renamed

        [Column("pointx")]
        public int? PointX { get; set; } // N;2;0 -> Renamed

        [Column("itemnote")]
        public string? ItemNote { get; set; } // M;10;0 -> Renamed

        [Column("user1nam")]
        public string? User1Nam { get; set; } // C;25;0

        [Column("user2nam")]
        public string? User2Nam { get; set; } // C;25;0

        [Column("vendnam")]
        public string? VendNam { get; set; } // C;30;0

        [Column("typenam")]
        public string? TypeNam { get; set; } // C;30;0

        [Column("taxable3")]
        public string? Taxable3 { get; set; } // C;1;0

        [Column("taxrate3")]
        public decimal? TaxRate3 { get; set; } // N;6;4 -> Renamed

        // --- Navigation property: Links to the Barcodes table ---
        // This tells EF Core that one Product can have many Barcodes.
        public virtual ICollection<Barcode> Barcodes { get; set; } = new List<Barcode>();
    }
}