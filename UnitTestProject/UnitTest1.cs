using Microsoft.VisualStudio.TestTools.UnitTesting;
using PalletsTestTask;
using System;
using System.Linq;

namespace UnitTestProject
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void TestPalleteCreation()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(5, 5, 5));

            Assert.AreEqual(pallet.Volume, 125);
            Assert.AreEqual(pallet.Weight, 30);
        }

        [TestMethod]
        public void BoxAddIncorrect()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            Box box = new Box(1, new SizeParameters(30, 30, 30), 30);

            Assert.IsFalse(pallet.AddBox(box));
        }

        [TestMethod]
        public void BoxAddCorrect()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            Box box = new Box(1, new SizeParameters(30, 30, 30), 30);

            Assert.IsFalse(pallet.AddBox(box));
        }

        [TestMethod]
        public void BoxAddEqualSize()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            Box box = new Box(1, new SizeParameters(20, 20, 20), 30);

            Assert.IsTrue(pallet.AddBox(box));
        }

        [TestMethod]
        public void VolumeCount()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 13.2f));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 5));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 11));

            Assert.AreEqual(8500, pallet.Volume);
        }

        [TestMethod]
        public void WeightCount()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 13.2f));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 5));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 11));

            Assert.AreEqual(71.2f, pallet.Weight);
        }

        [TestMethod]
        public void AutoExpireDateTest()
        {
            Box box = new Box(1, new SizeParameters(30, 30, 30), 30, DateTime.Parse("01.08.2023"));

            Assert.AreEqual(DateTime.Parse("09.11.2023"), box.ExpireDate);
        }

        [TestMethod]
        public void PalletExpireDate()
        {
            Pallet pallet = new Pallet(1, new SizeParameters(20, 20, 20));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("08.10.2023")));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("07.10.2023")));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("08.10.2024")));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("05.12.2023")));

            Assert.AreEqual(DateTime.Parse("07.10.2023"), pallet.ExpireDate);

        }

        [TestMethod]
        public void WarehouseGroupTest()
        {
            Warehouse warehouse = new Warehouse();
            Pallet pallet = new Pallet(1, new SizeParameters(10, 10, 10));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 25, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet);
            Pallet pallet2 = new Pallet(2, new SizeParameters(10, 10, 10));
            pallet2.AddBox(new Box(2, new SizeParameters(6, 6, 6), 12, expireDate: DateTime.Parse("07.10.2023")));
            warehouse.AddPallet(pallet2);
            Pallet pallet3 = new Pallet(3, new SizeParameters(10, 10, 10));
            pallet3.AddBox(new Box(3, new SizeParameters(8, 8, 8), 12, expireDate: DateTime.Parse("13.12.2023")));
            warehouse.AddPallet(pallet3);
            Pallet pallet4 = new Pallet(4, new SizeParameters(10, 10, 10));
            pallet4.AddBox(new Box(4, new SizeParameters(7, 7, 7), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet4);

            var sorted = warehouse.SortByDate();

            //Check groupping
            Assert.AreEqual(3, sorted.Count);
            Assert.AreEqual(1, sorted[DateTime.Parse("13.12.2023")].Count);
            Assert.AreEqual(1, sorted[DateTime.Parse("07.10.2023")].Count);
            Assert.AreEqual(2, sorted[DateTime.Parse("08.10.2023")].Count);

            //Check ordering
            Assert.AreEqual(42, sorted[DateTime.Parse("08.10.2023")][0].Weight);
            Assert.AreEqual(55, sorted[DateTime.Parse("08.10.2023")][1].Weight);
        }

        [TestMethod]
        public void WarehouseGetAmountTest()
        {
            Warehouse warehouse = new Warehouse();
            Pallet pallet = new Pallet(1, new SizeParameters(10, 10, 10));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet);
            Pallet pallet2 = new Pallet(2, new SizeParameters(10, 10, 10));
            pallet2.AddBox(new Box(2, new SizeParameters(6, 6, 6), 12, expireDate: DateTime.Parse("07.10.2023")));
            warehouse.AddPallet(pallet2);
            Pallet pallet3 = new Pallet(3, new SizeParameters(10, 10, 10));
            pallet3.AddBox(new Box(3, new SizeParameters(8, 8, 8), 12, expireDate: DateTime.Parse("13.12.2023")));
            warehouse.AddPallet(pallet3);
            Pallet pallet4 = new Pallet(4, new SizeParameters(10, 10, 10));
            pallet4.AddBox(new Box(4, new SizeParameters(7, 7, 7), 12, expireDate: DateTime.Parse("06.11.2023")));
            warehouse.AddPallet(pallet4);

            var pallets = warehouse.GetAmountByExpireDate();

            //Chosen pallets 1 3 4 by date and sorted in order 1 4 3 by volume
            Assert.AreEqual(1125, pallets[0].Volume);
            Assert.AreEqual(1343, pallets[1].Volume);
            Assert.AreEqual(1512, pallets[2].Volume);

        }

        [TestMethod]
        public void WarehouseGetAmountTestWithSameDate()
        {
            Warehouse warehouse = new Warehouse();
            Pallet pallet = new Pallet(1, new SizeParameters(10, 10, 10));
            pallet.AddBox(new Box(1, new SizeParameters(5, 5, 5), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet);
            Pallet pallet2 = new Pallet(2, new SizeParameters(10, 10, 10));
            pallet2.AddBox(new Box(2, new SizeParameters(6, 6, 6), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet2);
            Pallet pallet3 = new Pallet(3, new SizeParameters(10, 10, 10));
            pallet3.AddBox(new Box(3, new SizeParameters(8, 8, 8), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet3);
            Pallet pallet4 = new Pallet(4, new SizeParameters(10, 10, 10));
            pallet4.AddBox(new Box(4, new SizeParameters(7, 7, 7), 12, expireDate: DateTime.Parse("08.10.2023")));
            warehouse.AddPallet(pallet4);

            var pallets = warehouse.GetAmountByExpireDate();

            Assert.AreEqual(1125, pallets[0].Volume);
            Assert.AreEqual(1216, pallets[1].Volume);
            Assert.AreEqual(1343, pallets[2].Volume);

        }
    }
}
