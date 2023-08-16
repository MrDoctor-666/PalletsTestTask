using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    public class Warehouse
    {
        List<Pallet> pallets = new List<Pallet>();

        public void AddPallet(Pallet pallet) => pallets.Add(pallet);
        public int PalletsAmount() => pallets.Count;

        public Dictionary<DateTime, List<Pallet>> SortByDate()
        {
            var groupedPallets = pallets
                .GroupBy(p => p.ExpireDate)
                .OrderBy(p => p.Key)
                .ToDictionary(p => p.Key, p => p.ToList());

            foreach (var key in groupedPallets.Keys.ToList())
                groupedPallets[key] = groupedPallets[key].OrderBy(p => p.Weight).ToList();

            return groupedPallets;
        }

        public List<Pallet> GetAmountByExpireDate(int amount = 3)
        {
            var ordered = pallets.OrderByDescending(p => p.ExpireDate).ThenBy(p => p.Volume).Take(amount).OrderBy(p => p.Volume);

            return ordered.ToList();
        }

    }
}
