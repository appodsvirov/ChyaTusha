using ChyaTusha.Scenes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChyaTusha
{
    public class Plot
    {
        //public bool 
        public int BathroomState = 0;
        public int WaterfallState = 0;
        public Bathroom Bathroom { get; set; } = new();
        public Waterfall Waterfall { get; set; } = new();


        public void Setup()
        {
            BathroomState = 0;
            WaterfallState = 0;
        }
    }
}
