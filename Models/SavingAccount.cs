using System;
using System.Collections.Generic;

namespace Proyecto_Herramientas.Models;

public partial class SavingAccount
{
    public int AccountNumber { get; set; }

    public decimal InterestRate { get; set; }

    public virtual Account AccountNumberNavigation { get; set; } = null!;
}
