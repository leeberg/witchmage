using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Globalization;

namespace WitchMage
{
    class Util
    {

        public static Color ColorFromHex(string hexcode)
        {
            hexcode = hexcode.TrimStart('#');
            Color col; // from System.Drawing

            if (hexcode.Length == 6)
            {
                col = Color.FromArgb(255, // hardcoded opaque
                            int.Parse(hexcode.Substring(0, 2), NumberStyles.HexNumber),
                            int.Parse(hexcode.Substring(2, 2), NumberStyles.HexNumber),
                            int.Parse(hexcode.Substring(4, 2), NumberStyles.HexNumber));
            }
                
            else // assuming length of 8
            {
                col = Color.FromArgb(
                            int.Parse(hexcode.Substring(0, 2), NumberStyles.HexNumber),
                            int.Parse(hexcode.Substring(2, 2), NumberStyles.HexNumber),
                            int.Parse(hexcode.Substring(4, 2), NumberStyles.HexNumber),
                            int.Parse(hexcode.Substring(6, 2), NumberStyles.HexNumber));
            }

            return col;

        }

            


    }
}
