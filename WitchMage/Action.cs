using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WitchMage
{
    public class Action
    {
        public int id;
        public string type;
        public string formItemNameToEnable;
        public string formItemNameToDisable;
        public bool runAsAdmin = true;
        public string actionParam1;
        public string actionParam2;
        public string actionParam3;

    }
}
