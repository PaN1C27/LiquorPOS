using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LiquorPOS.Models;

/// <summary>
/// One entry in a tax breakdown for a single ticket line.
/// </summary>
public record TaxAmount(string ComponentName, decimal Amount);
