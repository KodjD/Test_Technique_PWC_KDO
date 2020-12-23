using LinqToDB.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test_Technique_PWC.Model
{
    public class Entry
    {
        public int Id { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public List<Ledger> Leger { get; } = new List<Ledger>();
    }
}
