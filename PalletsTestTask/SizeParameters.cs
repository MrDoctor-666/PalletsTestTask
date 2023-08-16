using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PalletsTestTask
{
    public struct SizeParameters
    {
        public float height;
        public float length;
        public float width;

        public SizeParameters(float height, float length, float width)
        {
            this.height = height;
            this.length = length;
            this.width = width;
        }
    }
}
