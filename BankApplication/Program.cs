using System;
using BankLibrary;

namespace BankApplication
{
    class Program
    {
        // TO DO - добавить к этой задаче возможность просмотра конкретностого счета (вывод на экран).
        // Поменял в настройках гита имя и почту.
        static void Main(string[] args)
        {
            var bank = new Bank<Account>("ЮнитБанк");
            var alive = true;

            while (alive)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.DarkGreen; // Список команд зелёным цветом.
                Console.WriteLine(" 1. Открыть счет \n 2. Вывести средства \n 3. Добавить на счет \n 4. Закрыть счет \n 5. Пропустить день \n 6. Выйти из программы");
                Console.WriteLine("Введите номер пункта:");
                Console.ForegroundColor = color;
                try
                {
                    var command = int.Parse(Console.ReadLine());
                    switch (command)
                    {
                        case 1:
                            OpenAccount(bank);
                            break;
                        case 2:
                            Withdraw(bank);
                            break;
                        case 3:
                            Put(bank);
                            break;
                        case 4:
                            CloseAccount(bank);
                            break;
                        case 5:
                            break;
                        case 6:
                            alive = false;
                            continue;
                        default:
                            alive = false;
                            break;
                    }
                    bank.CalculatePercentage();
                }
                catch (Exception ex)
                {
                    color = Console.ForegroundColor;
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(ex.Message);
                    Console.ForegroundColor = color;
                }
            }
            Console.ReadKey();
        }

        private static void OpenAccount(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму для создания счета:");
            var sum = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Выберите тип счета:\n    1. До востребования\n    2. Депозит");
            var type = int.Parse(Console.ReadLine());

            var accountType = type == 2 ? AccountType.Deposit : AccountType.Ordinary;

            bank.Open(accountType, sum, AddSumHandler, WithdrawSumHandler, (o, e) => Console.WriteLine(e.Message), CloseAccountHandler, OpenAccountHandler);
        }
        private static void Withdraw(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму для вывода со счета:");
            var sum = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Введите id счета:");
            var id = int.Parse(Console.ReadLine());

            bank.Withdraw(sum, id);
        }
        private static void Put(Bank<Account> bank)
        {
            Console.WriteLine("Укажите сумму, чтобы положить на счет:");
            var sum = decimal.Parse(Console.ReadLine());

            Console.WriteLine("Введите Id счета:");
            var id = int.Parse(Console.ReadLine());

            bank.Put(sum, id);
        }
        private static void CloseAccount(Bank<Account> bank)
        {
            Console.WriteLine("Введите id счета, который надо закрыть:");
            var id = int.Parse(Console.ReadLine());

            bank.Close(id);
        }

        #region Обработчики событий.
        /// <summary>
        /// Обработчик открытия счета.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void OpenAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        /// <summary>
        /// Обработчик добавления денег на счет.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void AddSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        /// <summary>
        /// Обработчик вывода средств.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void WithdrawSumHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
        /// <summary>
        /// Обработчик закрытия счета.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private static void CloseAccountHandler(object sender, AccountEventArgs e)
        {
            Console.WriteLine(e.Message);
            if (e.Sum > 0)
            {
                Console.WriteLine("Идем тратить деньги");
            }
        } 
        #endregion
    }
}
