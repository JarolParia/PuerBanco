using System;
using System.Collections.Generic;

namespace Proyecto_Herramientas.Models;

public partial class Movement
{
    public int IdTransaction { get; set; }

    public int? OriginAccount { get; set; }

    public int? DestinyAccount { get; set; }

    public int TransactionTId { get; set; }

    public decimal Amount { get; set; }

    public DateTime TransactionDate { get; set; }

    public virtual Account? DestinyAccountNavigation { get; set; } = null!;

    public virtual Account? OriginAccountNavigation { get; set; }

    public virtual TypeOfMovement TransactionT { get; set; } = null!;

    public string movementTypeName
    {
        get
        {
            return TransactionTId == 1 ? "Retiro" : TransactionTId == 2 ? "Deposito" : TransactionTId == 3 ? "Transferencia" : "Desconocido";
        }
    }
}
