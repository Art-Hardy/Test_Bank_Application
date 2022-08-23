using System;

namespace BankLibrary
{
    /// <summary>
    /// Тип счёта.
    /// </summary>
    public enum AccountType
    {
        Ordinary,
        Deposit
    }

    public class Bank<T> where T : Account
    {
        private T[] accounts;

        public string Name { get; private set; }

        public Bank(string name)
        {
            Name = name;
        }

        /// <summary>
        /// Создание счёта.
        /// </summary>
        public void Open(AccountType accountType, decimal sum,
            AccountStateHandler addSumHandler, AccountStateHandler withdrawSumHandler,
            AccountStateHandler calculationHandler, AccountStateHandler closeAccountHandler, AccountStateHandler openAccountHandler)
        {
            T newAccount = null;

            switch (accountType)
            {
                case AccountType.Ordinary:
                    {
                        newAccount = new DemandAccount(sum, 1) as T; // Параметризация (создание обобщённых классов) имеет ограничение в том, что
                        break;                                       // созданный объект необходимо привести к обобщённому типу - T.
                    }
                case AccountType.Deposit:
                    {
                        newAccount = new DemandAccount(sum, 1) as T;
                        break;
                    }
                default:
                    break;
            }

            if (newAccount == null)
            {
                throw new Exception("Ошибка создания счёта.");
            }
            // Добавляем новый счёт в массив счётов.
            if (accounts == null)
            {
                accounts = new T[] { newAccount };
            }
            else
            {
                // Так как массивы не расширяются автоматически, то для добавления нового аккаунта необходимо создать новый массив, увеличив количество элементов
                // на единицу.
                var tempAccounts = new T[accounts.Length + 1];
                Array.Copy(accounts, tempAccounts, accounts.Length);

                tempAccounts[^1] = newAccount; // тоже самое tempAccounts[tempAccounts.Length - 1] = newAccount;
                accounts = tempAccounts;
            }
            // Установка обработчиков событий счёта.
            newAccount.Added += addSumHandler;
            newAccount.Calculated += calculationHandler;
            newAccount.Closed += closeAccountHandler;
            newAccount.Opened += openAccountHandler;
            newAccount.Withdrawed += withdrawSumHandler;

            newAccount.Open();
        }
        /// <summary>
        /// Добавление средств на счёт.
        /// </summary>
        public void Put(decimal sum, int id)
        {
            var account = FindAccount(id);
            if (account == null)
            {
                throw new Exception("Счёт не найден!");
            }

            account.Put(sum);
        }
        /// <summary>
        /// Вывод средств.
        /// </summary>
        public void Withdraw(decimal sum, int id)
        {
            var account = FindAccount(id);
            if (account == null)
            {
                throw new Exception("Счёт не найден!");
            }

            account.Withdraw(sum);
        }
        /// <summary>
        /// Закрытие счёта.
        /// </summary>
        public void Close(int id)
        {
            var account = FindAccount(id, out var index);
            if (account == null)
            {
                throw new Exception("Счёт не найден!");
            }

            account.Close();

            if (accounts.Length <= 1)
            {
                accounts = null;
            }
            else
            {
                // Уменьшаем массив счётов, удаляя из него закрытый счёт.
                var tempAccounts = new T[accounts.Length - 1];
                for (int i = 0, j = 0; i < accounts.Length; i++)
                {
                    if (i != index)
                    {
                        tempAccounts[j++] = accounts[i];
                    }
                    accounts = tempAccounts;
                }
            }
        }
        /// <summary>
        /// Начисление процентов.
        /// </summary>
        public void CalculatePercentage()
        {
            if (accounts == null)
            {
                return;
            }
            foreach (var account in accounts)
            {
                account.IncrementDays();
                account.Calculate();
            }
            //for (int i = 0; i < accounts.Length; i++)     Так было написано на Metanit-e
            //{
            //    accounts[i].IncrementDays();
            //    accounts[i].Calculate();
            //}
        }
        /// <summary>
        /// 
        /// </summary>
        private Account FindAccount(int id)
        {
            foreach (var account in accounts)
            {
                if (account.Id == id)
                {
                    return account;
                }
            }
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        private Account FindAccount(int id, out int index)
        {
            for (int i = 0; i < accounts.Length; i++)
            {
                if (accounts[i].Id == id)
                {
                    index = i;
                    return accounts[i];
                }
            }
            index = -1;
            return null;
        }
    }
}
