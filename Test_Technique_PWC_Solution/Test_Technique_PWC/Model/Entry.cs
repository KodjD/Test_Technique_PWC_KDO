using LinqToDB.Mapping;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Test_Technique_PWC.Model
{
    [Table(Name = "Entry")]
    public class Entry
    {
        [Column(Name = "Id")]
        [Key]
        public string Id { get; set; }

        [Column(Name = "Code")]
        public string Code { get; set; }

        [Column(Name = "IdNumber")]
        public string Number { get; set; }

        public List<Ledger> Leger { get; } = new List<Ledger>();
    }
}
