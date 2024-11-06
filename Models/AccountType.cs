using System;
using System.Collections.Generic;

namespace Proyecto_Herramientas.Models;

public partial class AccountType
{
    public int TypeId { get; set; }

    public string TypeName { get; set; } = null!;

    public virtual ICollection<Account> Accounts { get; set; } = new List<Account>();
}
