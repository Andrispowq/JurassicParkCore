using System;

namespace JurassicPark.Core.OldModel.Dto
{
    public class Transaction
    {
        public long Id { get; set; }
        public DateTime CreatedAt { get; set; }
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
    }

    public class CreateTransactionDto
    {
        public TransactionType Type { get; set; }
        public decimal Amount { get; set; }
        public bool CanLose { get; set; }
    }
}