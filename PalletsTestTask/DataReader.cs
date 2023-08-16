using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    public static class DataReader
    {
        private static Random random;

        public static Warehouse ReadFromFile()
        {
            Console.WriteLine("File name: ");
            string fileName = Console.ReadLine();
            StreamReader reader = new StreamReader(fileName);

            Warehouse warehouse = new Warehouse();
            List<Box> boxes = new List<Box>();
            Pallet currentPallete = null;

            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                var values = line.Split(';');

                if (values[0] == "P")
                {
                    if (currentPallete != null)
                        warehouse.AddPallet(currentPallete);

                    SizeParameters size = new SizeParameters(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                    currentPallete = new Pallet(warehouse.PalletsAmount(), size);
                }
                else if (values[0] == "B")
                {
                    SizeParameters size = new SizeParameters(float.Parse(values[1]), float.Parse(values[2]), float.Parse(values[3]));
                    DateTime manufactureDate, endDate;
                    Box box;

                    if (DateTime.TryParse(values[6], out endDate))
                        box = new Box(boxes.Count, size, float.Parse(values[4]), expireDate: endDate);
                    else if (DateTime.TryParse(values[5], out manufactureDate))
                        box = new Box(boxes.Count, size, float.Parse(values[4]), manufactureDate);
                    else
                        box = new Box(boxes.Count, size, float.Parse(values[4]));

                    boxes.Add(box);

                    if (currentPallete != null)
                    {
                        if (!currentPallete.AddBox(box))
                            Console.WriteLine($"Warning! Box {box.ID} was not added because of size difference");
                    }
                    else Console.WriteLine($"Warning! Box {box.ID} is not in any pallet");
                }
                else
                {
                    Console.WriteLine("Line was started with the wrong letter. Check your data");
                    throw new ArgumentOutOfRangeException("Wrong letter");
                }
            }

            warehouse.AddPallet(currentPallete);
            reader.Close();

            return warehouse;
        }

        public static Warehouse GenerateRandom()
        {
            random = new Random();
            //Generate boxes
            int boxesNumber = random.Next(3, 100);
            List<Box> generatedBoxes = new List<Box>();
            for (int i = 0; i < boxesNumber; i++)
            {
                SizeParameters size = new SizeParameters(
                    random.Next(1, 100),
                    random.Next(1, 100),
                    random.Next(1, 100)
                    );
                Box box = new Box(i, size, random.Next(1, 50), GenerateRandomDate(new DateTime(2023, 1, 1), DateTime.Today));
                generatedBoxes.Add(box);
            }

            //Generate pallets with this boxes
            int palettesNumber = random.Next(3, boxesNumber);
            int boxesAmount;// = (int)Math.Ceiling(1f * generatedBoxes.Count/palettesNumber);
            Warehouse warehouse = new Warehouse();
            for (int i = 0; i < palettesNumber; i++)
            {
                SizeParameters size = new SizeParameters(
                    random.Next(10, 1000),
                    random.Next(10, 1000),
                    random.Next(10, 1000)
                    );
                Pallet pallet = new Pallet(i, size);
                int count = 0, index = 0;
                boxesAmount = random.Next(1, Math.Max(1, generatedBoxes.Count - palettesNumber));
                while (count < boxesAmount && index < generatedBoxes.Count)
                {
                    if (pallet.AddBox(generatedBoxes[index]))
                    {
                        count++;
                        generatedBoxes.RemoveAt(index);
                    }
                    else index++;
                }

                warehouse.AddPallet(pallet);
            }

            return warehouse;
        }

        private static DateTime GenerateRandomDate(DateTime start, DateTime end)
        {
            int range = (end - start).Days;
            return start.AddDays(random.Next(range));
        }

    }
}
