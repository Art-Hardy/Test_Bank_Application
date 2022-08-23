using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    /// <summary>
    /// >> https://metanit.com/sharp/tutorial/3.30.php
    /// Депозитные счета имеют особенность: они оформляются на продолжительный период, что накладывает некоторые ограничения.
    /// Поэтому здесь переопределяются еще три метода.
    /// Допустим, что депозитный счет имеет срок в 30 дней, в пределах которого, клиент не может ни добавить на счет, ни вывести часть средств со счета, кроме закрытия всего счета.
    /// Поэтому при всех операциях проверяем количество прошедших дней для данного счета: сравниваем остаток деления количества дней на 30 дней.
    /// </summary>
    public class DepositAccount : Account
    {
        public DepositAccount(decimal sum, int percentage) : base(sum, percentage)
        {
        }

        protected internal override void Open()
        {
            base.OnOpened(new AccountEventArgs($"Открыт новый депозитный счёт! Id счёта: {this.Id}", this.Sum));
        }
        public override void Put(decimal sum)
        {
            if (_days % 30 == 0)
            {
                base.Put(sum); 
            }
            else
            {
                base.OnAdded(new AccountEventArgs("На счет можно положить только после 30-ти дневного периода.", 0));
            }
        }
        public override decimal Withdraw(decimal sum)
        {
            if (_days % 30 == 0)
            {
                return base.Withdraw(sum); 
            }
            else
            {
                base.OnWithdrawed(new AccountEventArgs($"Вывести средства можно только после 30-ти дневного периода.", 0));
                return 0;
            }
        }
        protected internal override void Calculate()
        {
            if (_days % 30 == 0)
            {
                base.Calculate();
            }
        }
    }
}
