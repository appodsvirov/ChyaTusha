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
        public bool IsIntroComplete { get; set; } = false;
        public bool HasBlanket { get; set; } = false;
        public bool HasSwaddle { get; set; } = false;
        public bool HasBags { get; set; } = false;

        public int BathroomState = 0;
        public int WaterfallState = 0;
        public int CaveState = 0;
        public int ShitForestState = 0;
        public Bathroom Bathroom { get; set; } = new();
        public Waterfall Waterfall { get; set; } = new();
        public Cave Cave { get; set; } = new();
        public ShitForest ShitForest { get; set; } = new();


        public void Setup()
        {
            BathroomState = 0;
            WaterfallState = 0;
            CaveState = 0;
            ShitForestState = 0;

            IsIntroComplete = false;
            HasBlanket = false;
            HasSwaddle = false;
            HasBags = false;
        }
    }
}
