﻿using LinqToDB.Mapping;
using System;
using System.Collections.Generic;
using System.Text;

namespace Test_Technique_PWC.Model
{
    public class Ledger
    {
        public string Id { get; set; }
        public string AccountId { get; set; }
        public string EntryId { get; set; }
        public float Amount { get; set; }
    }
}
