using System;
using System.Collections.Generic;
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

        private ListNode First;

        public int Count { get; private set; }

        public SortedOperationList(params Operation[] ops) 
        {
            if (ops.Length != 0)
            {
                First = new ListNode() { Operation = ops[0] };
                Count = 1;
                for (int i = 1; i < ops.Length; i++)
                {
                    Add(ops[i]);
                }
            }
        }

        public void Add(Operation op)
        {
            var current = First;
            if (current is null || current.Operation.Date > op.Date)
            {
                var newNode = new ListNode { Last = current?.Last, Next = current, Operation = op };

                // Обновляем следующую Node
                current?.Last = newNode;
                // Новая начальная Node
                First = newNode;

                Count++;
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
                    
                    Count++;
                    return;
                }
                current = current.Next;
            }
            // Если ни одного return, значит дата операции больше всех
            var lastNode = new ListNode { Last = current, Next = null, Operation = op };

            // Обновляем предыдущую Node
            lastNode.Last.Next = lastNode;

            Count++;
        }

        public IEnumerable<Operation> Get(DateTime? dateStart = null, DateTime? dateEnd = null)
        {
            var current = First;
            DateTime start = (dateStart ?? DateTime.MinValue);
            DateTime end = (dateEnd ?? DateTime.MaxValue);
            while (!(current is null))
            {
                if (current.Operation.Date >= start)
                {
                    if (current.Operation.Date > end) break;

                    yield return current.Operation;
                }
            }
        }
    }
}
