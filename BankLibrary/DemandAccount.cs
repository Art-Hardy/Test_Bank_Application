using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    /// <summary>
    /// >> https://metanit.com/sharp/tutorial/3.30.php
    /// Представляет счёт до востребования.
    /// В данном классе переопределен один метод Open.
    /// При желании можно переопределить и больше функционала, но ограничимся в данном случае этим.
    /// </summary>
    public class DemandAccount : Account
    {
        public DemandAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }

        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый счёт до востребования! Id счёта: {this.Id}", this.Sum));
        }
    }
}
