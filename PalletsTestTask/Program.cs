using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    class Program
    {
        static void Main(string[] args)
        {
            bool readed = false;
            int value;
            Warehouse warehouse = new Warehouse();

            while (!readed)
            {
                Console.WriteLine("Select input type (0 - generate, 1 - read from file): ");
                string line = Console.ReadLine();

                if (int.TryParse(line, out value))
                {
                    if (value == 0)
                    {
                        warehouse = DataReader.GenerateRandom();
                        readed = true;
                    }
                    else if (value == 1)
                    {
                        try
                        {
                            warehouse = DataReader.ReadFromFile();
                            readed = true;
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Something went wrong while reading. Exception {ex.Message}");
                            readed = false;
                        }
                    }
                }
            }


            //Write
            var sorted = warehouse.SortByDate();
            foreach(var key in sorted.Keys.ToList())
            {
                Console.WriteLine($"Expire Date {key}");
                foreach(Pallet pallet in sorted[key])
                {
                    Console.WriteLine($"    Pallete {pallet.ID} with volume {pallet.Volume} and weight {pallet.Weight}");
                    foreach(Box box in pallet.boxes)
                    {
                        Console.WriteLine($"        Box {box.ID} with volume {box.Volume} and weight {box.Weight}");
                    }
                }
                Console.WriteLine();
            }

            Console.WriteLine("\nThree chosen pallets with max expire date, sorted by volume: ");

            var chosen = warehouse.GetAmountByExpireDate();

            foreach (Pallet pallet in chosen)
            {
                Console.WriteLine($"   Pallete {pallet.ID} with volume {pallet.Volume} and expire date {pallet.ExpireDate}");
            }

            Console.ReadLine();
        }
    }
}
