using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ChyaTusha.Scenes
{
    public abstract class Scene
    {
        protected List<string> _pics;

        public string this[int index]
        {
            get => _pics[index];
            set => _pics[index] = value;
        }
    }
}
