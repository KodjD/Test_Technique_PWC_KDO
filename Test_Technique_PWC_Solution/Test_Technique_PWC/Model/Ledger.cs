using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Technique_PWC.Model
{
    public class Ledger
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public int EntryId { get; set; }
        public float Amount { get; set; }
    }
}
