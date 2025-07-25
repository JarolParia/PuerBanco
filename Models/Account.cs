﻿using System;
using System.Collections.Generic;

namespace Proyecto_Herramientas.Models;

public partial class Account
{
    public int? AccountNumber { get; set; }

    public int UserId { get; set; }

    public int TypeId { get; set; }

    public decimal Balance { get; set; }

    public bool IsActive { get; set; }

    public DateTime CreatedAt { get; set; }

    public virtual CheckingAccount? CheckingAccount { get; set; }

    public virtual ICollection<Movement> MovementDestinyAccountNavigations { get; set; } = new List<Movement>();

    public virtual ICollection<Movement> MovementOriginAccountNavigations { get; set; } = new List<Movement>();


    public virtual SavingAccount? SavingAccount { get; set; }

    public virtual AccountType Type { get; set; } = null!;

    public virtual User User { get; set; } = null!;

    public string AccountTypeName
    {
        get
        {
            return TypeId == 1 ? "Ahorros" : TypeId == 2 ? "Corriente" : "Desconocido";
        }
    }
}
