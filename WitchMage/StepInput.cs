using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchMage
{
    public class StepInput
    {

        public int stepID;
        public string id;
        public string type;
        public int sizeWidth;
        public int sizeHeight;
        public int locationX;
        public int locationY;
        public string text;
        public bool isVisible;
        public bool isReadOnly;
        public int fontSize = 12;
        public bool hasBorder;
        public int actionID;
        public string imagePath;
        public bool isEnabled = true;
        public bool bringToFront = true;
        public bool isChecked = false;


    }
}
