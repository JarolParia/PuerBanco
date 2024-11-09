using Proyecto_Herramientas.Models;

namespace Proyecto_Herramientas.ViewModels
{
    public class ATMViewModel
    {
        public Account Account { get; set; }
        public User User { get; set; }

        public CheckingAccount CheckingAccount { get; set; }

        public SavingAccount SavingAccount { get; set; }

        public List<Movement> Movements { get; set; }
        public int AccountNumberL { get; set; }
    }
}
