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
}