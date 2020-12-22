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

namespace Test_Technique_PWC
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Voulez-vous effectuer des calculs ou quitter l’application ? ");            
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
                        Console.WriteLine("Merci et à bientôt");
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
            Console.WriteLine("Choisir entre ces 5 propositions");
            Console.WriteLine("1. Linq");
            Console.WriteLine("2. Lambda");
            Console.WriteLine("3. Sql");
            Console.WriteLine("4. Fibo");
            Console.WriteLine("5. P");
            int choix = 0;
            var filePath = "C:\\Users\\user\\source\\repos\\Test_Technique_PWC_KDO\\Test_Technique_PWC_Solution\\Test_Technique_PWC\\Db";
            List<string> entries = new List<string>();
            string dbpath = Path.Combine(filePath, "BaseTest.db");
            List<Ledger> ledgers = new List<Ledger>();
            var cnnStr = new SqliteConnection($"FileName ={dbpath};");
            if (Int32.TryParse(Console.ReadLine(), out choix) && Enumerable.Range(1, 5).Contains(choix))
            {
                using (SqliteConnection db = new SqliteConnection(($"FileName ={dbpath};")))
                {
                    db.Open();
                    SqliteCommand selectCommand = new SqliteCommand("SELECT * FROM Ledger", db);
                    SqliteDataReader query = selectCommand.ExecuteReader();
                    while (query.Read())
                    {
                        if (!query.IsDBNull(1))
                        {
                            ledgers.Add(new Ledger
                            {
                                Id = query.GetString(0),
                                EntryId = query.GetString(1),
                                AccountId = query.GetString(2),
                                Amount = float.Parse(query.GetString(3), CultureInfo.InvariantCulture.NumberFormat)
                            });
                        }
                    }
                    db.Close();
                    switch (choix)
                    {
                        case 1:
                            Console.WriteLine("Linq");
                            using (DataContext_DataTest dtc = new DataContext_DataTest())
                            {
                                ;
                            };
                            break;
                        case 2:
                            Console.WriteLine("Lambda ");
                            var entryImpactedAmount =
                                 from entry in ledgers
                                 group entry by entry.EntryId into entriesGroup
                                 select new
                                 {
                                     entry = entriesGroup.Key,
                                     TotalAmount = entriesGroup.Sum(x => x.Amount)
                                 };

                            break;
                        case 3:
                            Console.WriteLine("Sql");
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
                                    case  > 0:
                                        Console.WriteLine(item.entry+ " " + item.TotalAmount +" "+ "KO.U");
                                        break;
                                    case < 0:
                                        Console.WriteLine(item.entry + " " + item.TotalAmount + " " + "KO.D");
                                        break;
                                    case  0:
                                        Console.WriteLine(item.entry + " " + item.TotalAmount + " " + "OK");
                                        break;
                                    default:
                                        break;
                                }
                            }
                            break;
                        case 4:
                            Console.WriteLine("Fibo ");
                            break;
                        case 5:
                            Console.WriteLine("P");
                            FaireCalcul();
                            break;
                        default:
                            break;
                    }
                }
            }
            else
            {
                FaireCalcul();
            }
        }

        private static void FillObjects()
        {
            throw new NotImplementedException();
        }

        public static List<String> GetData()
        {
            List<string> entries = new List<string>();

            string dbpath = Path.Combine(Assembly.GetExecutingAssembly().Location, "sqliteSample.db");
            using (SqliteConnection db =
               new SqliteConnection($"Filename={dbpath}"))
            {
                db.Open();

                SqliteCommand selectCommand = new SqliteCommand
                    ("SELECT Text_Entry from MyTable", db);

                SqliteDataReader query = selectCommand.ExecuteReader();

                while (query.Read())
                {
                    entries.Add(query.GetString(0));
                }

                db.Close();
            }

            return entries;
        }

    }
}
