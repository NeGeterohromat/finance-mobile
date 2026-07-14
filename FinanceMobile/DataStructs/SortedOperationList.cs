using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FinanceMobile.DataStructs
{
    public class SortedOperationList
    {
        private class ListNode
        {
            public Operation Operation;
            public ListNode Last;
            public ListNode Next;
        }

        private struct PeriodicOperation
        {
            public Operation Operation;
            public int PeriodInDays;
        }

        private ListNode First;

        private List<PeriodicOperation> periodicOperations = new();


        // Это было информационное необязательное поле. Я не придуал, как в нём учитывать периодические операции.
        //public int Count { get; private set; }

        public SortedOperationList(params Operation[] ops) 
        {
            if (ops.Length != 0)
            {
                First = new ListNode() { Operation = ops[0] };
                //Count = 1;
                for (int i = 1; i < ops.Length; i++)
                {
                    Add(ops[i]);
                }
            }
        }

        public void Delete(Operation op, int periodInDays = -1)
        {
            var current = First;
            while (current != null)
            {
                if (current.Operation.Date > op.Date) 
                {
                    throw new InvalidOperationException($"Operation with date {op.Date} and value {op.Value} not found");
                }
                if (current.Operation.Equals(op))
                {
                    current.Last?.Next = current.Next;
                    current.Next?.Last = current.Last;

                    if (periodInDays != -1)
                    {
                        if (!periodicOperations.Remove(new PeriodicOperation() { Operation = op, PeriodInDays = periodInDays }))
                            throw new InvalidOperationException($"Periodic Operation with date {op.Date}, value {op.Value} and period {periodInDays} not found");
                    }

                    if (current.Last is null)
                    {
                        First = current.Next;
                    }
                    return;
                }
                current = current.Next;
            }
            throw new InvalidOperationException($"Operation with date {op.Date} and value {op.Value} not found");
        }

        public void Add(Operation op, int periodInDays = -1)
        {
            // Если указан период, то добавляем периодическую операцию в список.
            if (periodInDays != -1)
            {
                if (periodInDays < 1) throw new InvalidOperationException($"Period must be positive number, not {periodInDays}");
                periodicOperations.Add(new PeriodicOperation()
                {
                    Operation = op,
                    PeriodInDays = periodInDays
                });
            }

            var current = First;
            if (current is null || current.Operation.Date > op.Date)
            {
                var newNode = new ListNode { Last = current?.Last, Next = current, Operation = op };

                // Обновляем следующую Node
                current?.Last = newNode;
                // Новая начальная Node
                First = newNode;

                //Count++;
                return;
            }
            while (!(current.Next is null))
            {
                if (current.Operation.Date > op.Date)
                {
                    var newNode = new ListNode { Last = current.Last, Next = current, Operation = op };

                    // Обновляем следующую Node
                    current.Last = newNode;
                    // Обновляем предыдущую Node
                    newNode.Last.Next = newNode;
                    
                    //Count++;
                    return;
                }
                current = current.Next;
            }
            // Если ни одного return, значит дата операции больше всех
            var lastNode = new ListNode { Last = current, Next = null, Operation = op };

            // Обновляем предыдущую Node
            lastNode.Last.Next = lastNode;

            //Count++;
        }

        public IEnumerable<Operation> Get(DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            var current = First;
            DateTime start = (dateStart ?? DateTime.MinValue);
            DateTime end = dateEnd ?? (First is null ? DateTime.MinValue : First.Operation.Date.AddDays(365*5));
            while (!(current is null))
            {
                if (current.Operation.Date >= start)
                {
                    if (current.Operation.Date > end) break;

                    yield return current.Operation;

                    foreach (var op in GetPeriodicList(current, end))
                    {
                        yield return op;
                    }
                }
                current = current.Next;
            }
        }

        private List<Operation> GetPeriodicList(ListNode current, DateTime end)
        {
            var currentDate = current.Operation.Date;
            var nextDate = current.Next is null ? end : current.Next.Operation.Date;
            var list = new List<Operation>();
            foreach (var op in periodicOperations)
            {
                int daysToCurr = (currentDate - op.Operation.Date).Days;
                int daysToNext = (nextDate - op.Operation.Date).Days;

                // Базовое событие ещё не произошло
                if (daysToCurr < 0)
                    continue;

                int firstDaysSpan = daysToCurr + (op.PeriodInDays - daysToCurr % op.PeriodInDays);

                for (int i = firstDaysSpan; i < daysToNext; i += op.PeriodInDays)
                {
                    list.Add(new Operation() { Value = op.Operation.Value, Date = op.Operation.Date.AddDays(i) });
                }
            }
            list.Sort((op1, op2) => op1.Date.CompareTo(op2.Date));
            return list;
        }
    }
}
