using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    public class Box
    {
        public int ID;

        private float weight;
        private SizeParameters size;
        private DateTime manufactureDate;
        private DateTime expireDate;

        public float Volume
        {
            get
            {
                return size.height * size.length * size.width;
            }
        }
        public float Weight { get => weight; }
        public SizeParameters Size { get => size; }
        public DateTime ExpireDate { get => expireDate; }

        public Box(int ID, SizeParameters size, float weight, DateTime? manufactureDate = null, DateTime? expireDate = null)
        {
            this.ID = ID;
            this.size = size;
            this.weight = weight;

            if (manufactureDate.HasValue)
            {
                this.manufactureDate = (DateTime)manufactureDate;
                this.expireDate = this.manufactureDate.AddDays(100);
            }
            else if (expireDate.HasValue)
            {
                this.expireDate = (DateTime)expireDate;
            }
            else
            {
                this.expireDate = DateTime.Today;
            }
        }
    }
}
