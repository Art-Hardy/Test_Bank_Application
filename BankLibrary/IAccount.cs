using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    /// <summary>
    /// https://metanit.com/sharp/tutorial/3.29.php
    /// </summary>
    public interface IAccount
    {
        /// <summary>
        /// Положить деньги на счёт.
        /// </summary>
        /// <param name="sum"></param>
        void Put(decimal sum);
        /// <summary>
        /// Взять со счёта.
        /// </summary>
        /// <param name="sum"></param>
        /// <returns></returns>
        decimal Withdraw(decimal sum);
    }
}
