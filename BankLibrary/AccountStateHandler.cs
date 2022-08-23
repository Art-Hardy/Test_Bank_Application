using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    // Для реакции на изменения состояния счеты мы будем использовать событийную модель, то есть обрабатываться различные изменения счета через события.

    // Будет использоваться для создания событий.
    public delegate void AccountStateHandler(object sender, AccountEventArgs e);

    /// <summary>
    /// Определен для обработки событий.
    /// </summary>
    public class AccountEventArgs
    {
        /// <summary>
        /// Сообщение.
        /// </summary>
        public string Message { get; private set; }
        /// <summary>
        /// Сумма, на которую изменился счёт.
        /// </summary>
        public decimal Sum { get; private set; }

        public AccountEventArgs(string mes, decimal sum)
        {
            Message = mes;
            Sum = sum;
        }
    }
}
