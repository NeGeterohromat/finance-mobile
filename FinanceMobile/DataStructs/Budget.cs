using Avalonia.Remote.Protocol;
using FinanceMobile.Databases;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class Budget
    {
        public const string IncomesName = "Incomes";
        public const string ExpensesName = "Expenses";

        // Singletone
        public static Budget BudgetInstance = new();

        private Section incomes;
        private Section expenses;
        private Dictionary<string,double> accounts;
        private Dictionary<string, Deposite> deposites;
        private DatabaseService databaseService;

        private enum AccountType
        {
            Cash,
            Card,
            Deposite,
            Credit
        }

        private Dictionary<AccountType, string> databaseAccountTypes = new()
        {
            {AccountType.Cash, "cash" },
            {AccountType.Card,"card" },
            {AccountType.Deposite, "savings" },
            {AccountType.Credit, "credit" }
        };

        private Dictionary<string, AccountType> databaseAccountTypesReverse = new();

        private Dictionary<string, string> databaseOperationTypeNames = new()
        {
            {IncomesName,"income" },
            {ExpensesName,"expense" }
        };
        private Dictionary<string, string> databaseOperationTypeNamesReverse;

        private Dictionary<string, Section> sections;

        public Budget() 
        {
            accounts = new();
            incomes = new(IncomesName);
            expenses = new(ExpensesName);
            deposites = new();
            sections = new()
            {
                {IncomesName, incomes },
                {ExpensesName, expenses }
            };

            databaseOperationTypeNamesReverse = databaseOperationTypeNames.ToDictionary(pair => pair.Value, pair => pair.Key);
            databaseAccountTypesReverse = databaseAccountTypes.ToDictionary(pair => pair.Value, pair => pair.Key);

            databaseService = DatabaseService.DatabaseServiceInstance;

            LoadDataFromDB();
        }

        public void AddAccount(string name, double initialBalance = 0)
        {
            if (deposites.ContainsKey(name) || accounts.ContainsKey(name)) throw new InvalidOperationException($"Account or Deposite with name {name} already exists");
            accounts[name] = initialBalance;

            // захардкожено
            var selectedType = AccountType.Card;

            databaseService.SaveAccount(new Account()
            {
                Name = name,
                Type = databaseAccountTypes[selectedType],
                Balance = initialBalance
            });
        }

        public void RewriteAccountBalance(string name, double balance)
        {
            if (accounts.ContainsKey(name))
            {
                accounts[name] = balance;

                var acc = databaseService.GetAccount(name);
                acc.Balance = balance;
                databaseService.SaveAccount(acc);
            }
            else throw new InvalidOperationException($"Account with name {name} does not exists");
        }

        public double GetAccountsSum() => accounts.Values.Sum();

        // Учитываем только actual, за всё время ввода данных.
        public double GetAccountsAndBudgetDifference() => GetAccountsSum() - GetIncomeExpenseDifference();

        public void AddDeposite(string name)
        {
            if (deposites.ContainsKey(name) || accounts.ContainsKey(name)) throw new InvalidOperationException($"Deposite or Account with name {name} already exists");
            deposites[name] = new(name);

            databaseService.SaveAccount(new Account()
            {
                Name = name,
                Type = databaseAccountTypes[AccountType.Deposite],
                Balance = 0
            });
        }

        public void AddRefillToDeposite(string name, Operation refill, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.Refill, refill, isPlanned);

            var depo = databaseService.GetAccount(name);
            depo.Balance = deposites[name].GetDepositeResult(null, isPlanned);
            databaseService.SaveAccount(depo);
        }

        public void AddPercentsToDeposite(string name, Operation percents, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.Percents, percents, isPlanned);

            var depo = databaseService.GetAccount(name);
            depo.Balance = deposites[name].GetDepositeResult(null, isPlanned);
            databaseService.SaveAccount(depo);
        }

        public void AddWriteDownsToDeposite(string name, Operation writeDowns, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.WriteDowns, writeDowns, isPlanned);

            var depo = databaseService.GetAccount(name);
            depo.Balance = deposites[name].GetDepositeResult(null, isPlanned);
            databaseService.SaveAccount(depo);
        }

        // Счёт на депозите всегда считается с самого начала этого депозита.
        public double GetDepositeResult(string name, DateTime? dateEnd = null, bool isPlanned = false)
        {
            return deposites[name].GetDepositeResult(dateEnd, isPlanned);
        }

        public IEnumerable<string> GetAccountNames()
        {
            foreach (var  account in accounts)
            {
                yield return account.Key;
            }
        }

        public IEnumerable<string> GetDepositeNames()
        {
            foreach (var depo in deposites)
            {
                yield return depo.Key;
            }
        }

        public void AddCategory(string sectionName, string categoryName)
        {
            // Безопасный код для билда, потом удалить. (Exceptions в билде могут сломать приложение)
            if (sections[sectionName].GetCategoryNames().Contains(categoryName)) return;
            // До сюда

            var section = sections[sectionName];
            section.AddCategory(categoryName);

            databaseService.SaveCategory(new Databases.Category()
            {
                Name = categoryName,
                Type = databaseOperationTypeNames[sectionName],
                Color = "" // Пока хз, как будет реализован цвет
            });
        }

        public string GetAccountID(string name)
        {
            return databaseService.GetAccount(name).Id;
        }

        public void AddOperation(string sectionName, string categoryName, DateTime date, string accountID, double value, bool isPlanned = false,
            int periodInDays = -1, DateTime? endDate = null)
        {
            var section = sections[sectionName];
            section.AddOperation(categoryName, new Operation() { Date = date, Value = value, AccountId = accountID }, isPlanned, periodInDays);

            if (periodInDays >= 1)
            {
                var periodicOperation = new Databases.PeriodicOperation()
                {
                    StartDate = date,
                    EndDate = endDate,
                    Type = databaseOperationTypeNames[sectionName],
                    CategoryId = databaseService.GetCategoryID(categoryName, databaseOperationTypeNames[sectionName]),
                    AccountId = "", // Пока хз, как работать с аккаунтами.
                    Amount = value,
                    Description = "",
                    IntervalDays = periodInDays
                };
                databaseService.SavePeriodicOperation(periodicOperation);

                // для десктопа сохраняю по их правилам, сам игнорирую эти записи
                var repeatUntil = endDate ?? date.AddDays(SortedOperationList.DefaultMaxDays);
                for (DateTime i = date; i < repeatUntil; i = i.AddDays(periodInDays))
                    databaseService.SaveOperation(new Databases.Operation()
                    {
                        Date = date,
                        Type = databaseOperationTypeNames[sectionName],
                        Status = isPlanned ? "planned" : "actual",
                        CategoryId = databaseService.GetCategoryID(categoryName, databaseOperationTypeNames[sectionName]),
                        AccountId = accountID, 
                        Amount = value,
                        Description = "",
                        ReccuringId = periodicOperation?.Id
                    });
            } else if (periodInDays == -1)
            {
                databaseService.SaveOperation(new Databases.Operation()
                {
                    Date = date,
                    Type = databaseOperationTypeNames[sectionName],
                    Status = isPlanned ? "planned" : "actual",
                    CategoryId = databaseService.GetCategoryID(categoryName, databaseOperationTypeNames[sectionName]),
                    AccountId = accountID,
                    Amount = value,
                    Description = "",
                });
            } else throw new InvalidOperationException($"Period must be positive number, not {periodInDays}");
        }

        public IEnumerable<Operation> GetOperations(string sectionName, string categoryName,
            DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false)
        {
            foreach (var op in sections[sectionName].GetOperations(categoryName, dateStart, dateEnd, isPlanned))
                yield return op;
        }

        public double GetCategorySum(string sectionName, string categoryName, DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false)
        {
            return sections[sectionName].GetCategorySum(categoryName, dateStart, dateEnd, isPlanned);
        }

        public double GetSectionSum(string sectionName, DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false)
        {
            return sections[sectionName].GetSum(dateStart, dateEnd, isPlanned);
        }

        public double GetIncomeExpenseDifference(DateTime? dateStart = null, DateTime? dateEnd = null, bool isPlanned = false)
        {
            return incomes.GetSum(dateStart, dateEnd, isPlanned) - expenses.GetSum(dateStart, dateEnd, isPlanned);
        }

        public IEnumerable<string> GetCategoryNames(string sectionName)
        {
            foreach (var cat in sections[sectionName].GetCategoryNames())
            {
                yield return cat;
            }
        }

        private void LoadDataFromDB()
        {
            var categories = databaseService.GetCategoryList().ToDictionary(c => c.Id, c => c);
            var operations = databaseService.GetOperationList().Where(o => o.ReccuringId is null);
            var periodicOperations = databaseService.GetPeriodicoperationList();
            var accounts = databaseService.GetAccountList();

            foreach (var category in categories.Values)
            {
                sections[databaseOperationTypeNamesReverse[category.Type]].AddCategory(category.Name);
            }

            foreach (var operation in operations)
            {
                sections[databaseOperationTypeNamesReverse[operation.Type]].AddOperation(categories[operation.CategoryId].Name,
                    new Operation() { Date = operation.Date, Value = operation.Amount, AccountId = operation.AccountId },
                    operation.Status == "planned");
            }

            // пока в UI есть добавление операций только в actual, периодические тоже буду добавлять в actual
            bool periodicOperationsIsPlanned = false;
            foreach (var operation in periodicOperations)
            {
                sections[databaseOperationTypeNamesReverse[operation.Type]].AddOperation(categories[operation.CategoryId].Name,
                    new Operation() { Date = operation.StartDate, Value = operation.Amount },
                    periodicOperationsIsPlanned,
                    operation.IntervalDays,
                    operation.EndDate);
            }

            // Пока нет UI для добавления счёта, этот код - пустой. Раскоммментируете, когда появится UI для добавления счетов.
            /* 
            foreach (var acc in accounts)
            {
                if (databaseAccountTypesReverse[acc.Type] == AccountType.Deposite)
                {
                    deposites[acc.Name] = new Deposite(acc.Name);
                    deposites[acc.Name].AddOperation(Deposite.Refill,new Operation() { AccountId = acc.Id, Value = acc.Balance, Date = DateTime.Now });
                }
                else
                {
                    this.accounts[acc.Name] = acc.Balance;
                }
            }
            */

            // Заглушка, добавляющая счета

            deposites["депозит1"] = new Deposite("депозит1");
            deposites["депозит1"].AddOperation(Deposite.Refill, new Operation() { AccountId = "aa", Value = 300, Date = DateTime.Now });

            this.accounts["Карта"] = 1000;
            this.accounts["Наличные"] = 100;
        }
    }
}
