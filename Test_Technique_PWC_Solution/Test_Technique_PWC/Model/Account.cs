using System.Collections.Generic;


namespace Test_Technique_PWC.Model
{
    public class Account
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public long Number { get; set; }
        public List<Ledger> Leger { get; } = new List<Ledger>(); 

    }
}
