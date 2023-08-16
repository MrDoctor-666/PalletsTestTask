using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    public class Pallet
    {
        public int ID;
        public List<Box> boxes;

        private const float constantWeight = 30;
        private SizeParameters size;
        private DateTime expireDate;

        public float Volume
        {
            get
            {
                float volume = 0;
                foreach (Box box in boxes)
                    volume += box.Volume;

                return volume + size.height * size.length * size.width;
            }
        }
        public float Weight
        {
            get
            {
                float weight = 0;
                foreach (Box box in boxes)
                    weight += box.Weight;

                return weight + constantWeight;
            }
        }
        public DateTime ExpireDate => expireDate;

        public Pallet(int ID, SizeParameters size, List<Box> boxes = null)
        {
            this.ID = ID;
            this.size = size;

            if (boxes == null) this.boxes = new List<Box>();
            else this.boxes = boxes;

            expireDate = DateTime.MaxValue;
            foreach (Box box in this.boxes)
                CheckExpireDate(box);
        }

        public bool AddBox(Box box)
        {
            if (size.length >= box.Size.length && size.width >= box.Size.width)
            {
                boxes.Add(box);
                CheckExpireDate(box);
                return true;
            }

            return false;
        }

        private void CheckExpireDate(Box box)
        {
            if (box.ExpireDate < expireDate)
                expireDate = box.ExpireDate;
        }
    }
}
