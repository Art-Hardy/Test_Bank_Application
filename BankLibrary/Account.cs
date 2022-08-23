using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BankLibrary
{
    /// <summary>
    /// >> https://metanit.com/sharp/tutorial/3.30.php
    /// Поскольку как такого банковского счета нет, а есть конкретные счета - депозит, до востребования и т.д., то данный класс является абстрактным.
    /// В то же время он реализует интерфейс IAccount.
    /// Он определяет ряд событий, которые вызываются при изменении состояния.
    /// Для определения событий применяется ранее созданный делегат AccountStateHandler.
    /// Почти все методы являются виртуальными. Прежде всего определяются методы генерации.
    /// Также надо отметить, что большинство членов класса имеют модификатор protected internal, то есть они будут видны только внутри проекта BankLibrary.
    /// 
    /// Абстрактный класс Account определяет полиморфный интерфейс, который наследуется или переопределяется производными классами.
    /// </summary>
    public abstract class Account : IAccount
    {
        #region События
        /// <summary>
        /// Событие, возникающее при выводе денег.
        /// </summary>
        protected internal event AccountStateHandler Withdrawed;
        /// <summary>
        /// Событие, возникающее при добавлении на счёт.
        /// </summary>
        protected internal event AccountStateHandler Added;
        /// <summary>
        /// Событие, возникающее при открытии счёта.
        /// </summary>
        protected internal event AccountStateHandler Opened;
        /// <summary>
        /// Событие, возникающее при закрытии счёта.
        /// </summary>
        protected internal event AccountStateHandler Closed;
        /// <summary>
        /// Событие, возникающее при начислении процентов.
        /// </summary>
        protected internal event AccountStateHandler Calculated;
        #endregion

        static int counter = 0;
        // Время с момента открытия счёта.        
        protected int _days = 0;
        
        /// <summary>
        /// Текущая сумма на счету.
        /// </summary>
        public decimal Sum { get; private set; }
        /// <summary>
        /// Процент начислений.
        /// </summary>
        public int Percentage { get; private set; }
        /// <summary>
        /// Уникальный идентификатор счёта.
        /// </summary>
        public int Id { get; private set; }

        /// <summary>
        /// Конструктор.
        /// </summary>
        /// <param name="sum">Сумма.</param>
        /// <param name="percentage">Проценты.</param>
        public Account(decimal sum, int percentage)
        {
            Sum = sum;
            Percentage = percentage;
            Id = ++counter;
        }

        /// <summary>
        /// Вызовавет событие, которое представляет делегат AccountStateHandler и передает ему аргументы события - объект AccountEventArgs.
        /// </summary>
        /// <param name="e"></param>
        /// <param name="handler"></param>
        private void CallEvent(AccountEventArgs e, AccountStateHandler handler)
        {
            if (e != null)
            {
                handler?.Invoke(this, e);
            }
        }

        #region Вызов отдельных событий
        // Для вызова отдельных событий с помощью метода CallEvent предусмотрены методы OnAdded, OnCalculated, OnClosed, OnOpened, OnWithdrawed.
        // Подобная организация вызова событий позволит проще переопределить модель вызова в классах-наследниках и передать различные для каждого класса-наследника объекты AccountEventArgs.
        protected virtual void OnOpened(AccountEventArgs e) => CallEvent(e, Opened);
        protected virtual void OnWithdrawed(AccountEventArgs e) => CallEvent(e, Withdrawed);
        protected virtual void OnAdded(AccountEventArgs e) => CallEvent(e, Added);
        protected virtual void OnClosed(AccountEventArgs e) => CallEvent(e, Closed);
        protected virtual void OnCalculated(AccountEventArgs e) => CallEvent(e, Calculated);
        #endregion

        public virtual void Put(decimal sum)
        {
            Sum += sum;
            OnAdded(new AccountEventArgs($"На счёт поступило {sum}", sum));
        }
        /// <summary>
        /// Метод снятия со счёта.
        /// </summary>
        /// <param name="sum">Сумма снятия.</param>
        /// <returns>Возвращает сколько со счёта снято.</returns>
        public virtual decimal Withdraw(decimal sum)
        {
            decimal result = 0;
            if (Sum >= sum)
            {
                Sum -= sum;
                result = sum;
                OnWithdrawed(new AccountEventArgs($"Сумма {sum} снята со счёт {Id}", sum));
            }
            else
            {
                OnWithdrawed(new AccountEventArgs($"Недостаточно денег на счёте {Id}", 0));
            }
            return result;
        }
        /// <summary>
        /// Открытие счёта.
        /// </summary>
        protected internal virtual void Open() => OnOpened(new AccountEventArgs($"Открыт новый счёт! Id счёта: {Id}", Sum));
        /// <summary>
        /// Закрытие счёта.
        /// </summary>
        protected internal virtual void Close() => OnClosed(new AccountEventArgs($"Счёт {Id} закрыт. Итоговая сумма: {Sum}", Sum));
        /// <summary>
        /// Увеличивает количество дней.
        /// </summary>
        protected internal void IncrementDays() => _days++;
        /// <summary>
        /// Начисляет проценты.
        /// </summary>
        protected internal virtual void Calculate()
        {
            decimal increment = Sum * Percentage / 100;
            Sum += increment;
            OnCalculated(new AccountEventArgs($"Начислены проценты в размере: {increment}", increment));
        }
    }
}
