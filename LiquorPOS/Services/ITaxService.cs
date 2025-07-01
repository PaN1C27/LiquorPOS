using LiquorPOS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquorPOS.Services;

/// <summary>
/// Returns the *exact* tax dollars that apply to ONE line item.
/// Each element in the enumerable is a separate tax bucket
/// (e.g., “MD General 6 %”, “MD Alcohol 9 %”).
/// </summary>
public interface ITaxService
{
    IEnumerable<TaxAmount> GetTaxBreakdown(
        Product product,
        int quantity,
        DateTime saleDate);
}
