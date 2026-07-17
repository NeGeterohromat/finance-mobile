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

            databaseService = DatabaseService.DatabaseServiceInstance;

            LoadDataFromDB();
        }

        public void AddAccount(string name, double initialBalance = 0)
        {
            if (accounts.ContainsKey(name)) throw new InvalidOperationException($"Account with name {name} already exists");
            accounts[name] = initialBalance;

            // TODO сохранение в бд
        }

        public void RewriteAccountBalance(string name, double balance)
        {
            if (accounts.ContainsKey(name))
            {
                // TODO сохранение в бд
                accounts[name] = balance;
            }
            else throw new InvalidOperationException($"Account with name {name} does not exists");
        }

        public double GetAccountsSum() => accounts.Values.Sum();

        // Учитываем только actual, за всё время ввода данных.
        public double GetAccountsAndBudgetDifference() => GetAccountsSum() - GetIncomeExpenseDifference();

        public void AddDeposite(string name)
        {
            if (deposites.ContainsKey(name)) throw new InvalidOperationException($"Deposite with name {name} already exists");
            deposites[name] = new(name);
        }

        public void AddRefillToDeposite(string name, Operation refill, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.Refill, refill, isPlanned);
        }

        public void AddPercentsToDeposite(string name, Operation percents, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.Percents, percents, isPlanned);
        }

        public void AddWriteDownsToDeposite(string name, Operation writeDowns, bool isPlanned = false)
        {
            deposites[name].AddOperation(Deposite.WriteDowns, writeDowns, isPlanned);
        }

        // Счёт на депозите всегда считается с самого начала этого депозита.
        public double GetDepositeResult(string name, DateTime? dateEnd = null, bool isPlanned = false)
        {
            return deposites[name].GetDepositeResult(dateEnd, isPlanned);
        }

        public void AddCategory(string sectionName, string categoryName)
        {
            var section = sections[sectionName];
            section.AddCategory(categoryName);

            databaseService.SaveCategory(new Databases.Category()
            {
                Name = categoryName,
                Type = databaseOperationTypeNames[sectionName],
                Color = "" // Пока хз, как будет реализован цвет
            });
        }

        public void AddOperation(string sectionName, string categoryName, DateTime date, double value, bool isPlanned = false,
            int periodInDays = -1, DateTime? endDate = null)
        {
            var section = sections[sectionName];
            section.AddOperation(categoryName, new Operation() { Date = date, Value = value }, isPlanned, periodInDays);

            if (periodInDays == -1)
            {
                databaseService.SaveOperation(new Databases.Operation()
                {
                    Date = date,
                    Type = databaseOperationTypeNames[sectionName],
                    Status = isPlanned ? "planned" : "actual",
                    CategoryId = databaseService.GetCategoryID(categoryName, databaseOperationTypeNames[sectionName]),
                    AccountId = "", // Пока хз, как работать с аккаунтами.
                    Amount = value,
                    Description = ""
                });
            } else if (periodInDays > 1)
            {
                databaseService.SavePeriodicOperation(new Databases.PeriodicOperation()
                {
                    StartDate = date,
                    EndDate = endDate,
                    Type = databaseOperationTypeNames[sectionName],
                    //Status = isPlanned ? "planned" : "actual",
                    CategoryId = databaseService.GetCategoryID(categoryName, databaseOperationTypeNames[sectionName]),
                    AccountId = "", // Пока хз, как работать с аккаунтами.
                    Amount = value,
                    Description = "",
                    IntervalDays = periodInDays
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
            var operations = databaseService.GetOperationList();

            foreach (var category in categories.Values)
            {
                sections[databaseOperationTypeNamesReverse[category.Type]].AddCategory(category.Name);
            }

            foreach (var operation in operations)
            {
                sections[databaseOperationTypeNamesReverse[operation.Type]].AddOperation(categories[operation.CategoryId].Name,
                    new Operation() { Date = operation.Date, Value = operation.Amount },
                    operation.Status == "planned");
            }
        }
    }
}
