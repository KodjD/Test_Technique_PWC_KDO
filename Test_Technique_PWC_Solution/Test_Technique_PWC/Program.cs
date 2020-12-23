using LinqToDB;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Threading;
using Test_Technique_PWC.Model;
using System.Globalization;
using Test_Technique_PWC.DataContext;
using System.Threading.Tasks;

namespace Test_Technique_PWC
{
    class Program
    {
        private static List<Account> accounts = new List<Account>();
        private static List<Ledger> ledgers = new List<Ledger>();
        private static List<Entry> entries = new List<Entry>();
        static void Main(string[] args)
        {
            GetData();
            Console.WriteLine("*******Voulez-vous effectuer des calculs ou quitter l’application ? *********");
            DoAction();
        }

        private static void DoAction()
        {
            var response = Console.ReadLine().ToUpper();
            if (response != "C" && response != "Q")
            {
                Console.WriteLine("Entrez 'c' pour le calcul et 'q' pour quitter ? ");
                DoAction();
            }
            else
            {
                switch (response)
                {
                    case "C":
                        FaireCalcul();
                        break;
                    case "Q":
                        Console.WriteLine("*******Merci et à bientôt*******");
                        Thread.Sleep(4000);
                        Environment.Exit(0);
                        break;
                    default:
                        break;
                }
            }
        }

        private static void FaireCalcul()
        {
            Console.WriteLine("******* Choisir entre ces 5 propositions *******");
            Console.WriteLine("1. Linq");
            Console.WriteLine("2. Lambda");
            Console.WriteLine("3. Sql");
            Console.WriteLine("4. Fibo");
            Console.WriteLine("5. P");
            int choix = 0;
            if (Int32.TryParse(Console.ReadLine(), out choix) && Enumerable.Range(1, 5).Contains(choix))
            {
                switch (choix)
                {
                    case 1:
                        Console.WriteLine("*******Linq*******");
                        var distinctAccountAmount =
                              from ledger in ledgers
                              group ledger by ledger.AccountId into AccountGroup
                              select new
                              {
                                  AccountId = AccountGroup.Key,
                                  AccountName = (from account in accounts
                                                where account.Id == AccountGroup.FirstOrDefault().AccountId
                                                select account.Name).FirstOrDefault().ToString(),
                                  Amount = Math.Abs(AccountGroup.Sum(x => x.Amount))                        
                              };
                        foreach (var account in distinctAccountAmount)
                        {
                            Console.WriteLine(string.Format("{0}|{1}|{2}",account.AccountId.ToString(),account.AccountName, account.Amount.ToString()));
                        }
                        FaireCalcul();
                        break;
                    case 2:
                        Console.WriteLine("*******Lambda******* ");
                        var impactedAccount = 
                            ledgers.GroupBy(l => l.EntryId)
                            .Select(le => new
                            {
                                EntryId = le.Key,
                                EntryName = entries
                                            .Where(eg => eg.Id == le.First().EntryId)
                                            .Select(e => e.Name.ToString())
                                            .FirstOrDefault(),
                                NbAccountImpacted = le.Count(),
                            });
                        foreach (var ledger in impactedAccount)
                        {
                            Console.WriteLine(string.Format("{0}|{1}|{2}", ledger.EntryId, ledger.EntryName, ledger.NbAccountImpacted));
                        }
                        FaireCalcul();
                        break;
                    case 3:
                        Console.WriteLine("*******Sql*******");
                        var entryAmountSum =
                            from entry in ledgers
                            group entry by entry.EntryId into entriesGroup
                            select new
                            {
                                entry = entriesGroup.Key,
                                TotalAmount = entriesGroup.Sum(x => x.Amount)
                            };
                        foreach (var item in entryAmountSum)
                        {
                            switch (item.TotalAmount)
                            {
                                case > 0:
                                    Console.WriteLine(string.Format("{0}|{1}|KO.U", item.entry, item.TotalAmount));
                                    break;
                                case < 0:
                                    Console.WriteLine(string.Format("{0}|{1}|KO.D", item.entry, item.TotalAmount));
                                    break;
                                case 0:
                                    Console.WriteLine(string.Format("{0}|{1}|OK", item.entry, item.TotalAmount));
                                    break;
                                default:
                                    break;
                            }
                        }
                        FaireCalcul();
                        break;
                    case 4:
                        Console.WriteLine("*******Fibo*******");
                        int number = 0;
                        if (Int32.TryParse(Console.ReadLine(), out number) && number >= 0)
                        {
                            var res = Fibonacci(number);
                            Console.WriteLine(res);
                        }
                        FaireCalcul();
                        break;
                    case 5:
                        Console.WriteLine("*******P*******");
                        FaireCalcul();
                        break;
                    default:
                        break;
                }
            }
            else
            {
                FaireCalcul();
            }
        }

        public static void GetData()
        {
            var filePath = "C:\\Users\\user\\source\\repos\\Test_Technique_PWC_KDO\\Test_Technique_PWC_Solution\\Test_Technique_PWC\\Db";
            string dbpath = Path.Combine(filePath, "BaseTest.db");
            using (SqliteConnection db = new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();
                Task taskAccount = Task.Factory.StartNew(() =>
                {
                    SqliteCommand selectCommandAccount = new SqliteCommand("SELECT * from Account", db);
                    SqliteDataReader queryAccount = selectCommandAccount.ExecuteReader();
                    while (queryAccount.Read())
                    {
                        if (!queryAccount.IsDBNull(1))
                        {
                            accounts.Add(new Account
                            {
                                Id = Int32.Parse(queryAccount.GetString(0)),
                                Number = long.Parse(queryAccount.GetString(1)),
                                Name = queryAccount.GetString(2),
                            });
                        }
                    }

                });
                Task taskLedger = Task.Factory.StartNew(() =>
                {
                    SqliteCommand selectCommandLedger = new SqliteCommand("SELECT * from Ledger", db);
                    SqliteDataReader queryLedger = selectCommandLedger.ExecuteReader();
                    while (queryLedger.Read())
                    {
                        if (!queryLedger.IsDBNull(1))
                        {
                            ledgers.Add(new Ledger
                            {
                                Id = Int32.Parse(queryLedger.GetString(0)),
                                EntryId = Int32.Parse(queryLedger.GetString(1)),
                                AccountId = Int32.Parse(queryLedger.GetString(2)),
                                Amount = float.Parse(queryLedger.GetString(3), CultureInfo.InvariantCulture.NumberFormat)
                            });
                        }
                    }
                });
                Task taskEntry = Task.Factory.StartNew(() =>
                {
                    SqliteCommand selectCommandEntry = new SqliteCommand("SELECT * from Entry", db);
                    SqliteDataReader queryEntry = selectCommandEntry.ExecuteReader();

                    while (queryEntry.Read())
                    {
                        if (!queryEntry.IsDBNull(1))
                        {
                            entries.Add(new Entry
                            {
                                Id = Int32.Parse(queryEntry.GetString(0)),
                                Code = queryEntry.GetString(1),
                                Name = queryEntry.GetString(2),
                            });
                        }
                    }

                });
                Task.WaitAll(taskAccount, taskEntry, taskLedger);
                Console.WriteLine("All threads complete");
                Console.WriteLine("Data loaded");
                db.Close();
            }
        }

        public static int Fibonacci(int n)
        {
            if ((n == 0) || (n == 1))
            {
                return n;
            }
            return Fibonacci(n - 1) + Fibonacci(n - 2);
        }

    }
}
