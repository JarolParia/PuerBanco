using System;
using System.Collections.Generic;

namespace Proyecto_Herramientas.Models;

public partial class CheckingAccount
{
    public int AccountNumber { get; set; }

    public decimal OverDraftLimit { get; set; }

    public virtual Account AccountNumberNavigation { get; set; } = null!;
}
