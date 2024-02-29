using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartGroceries.Models
{
    public enum Unit
    {
        Weight = 0,
        Volume = 1,
        Piece = 2
    }

    public partial class Helpers
    {
        public static string UnitToString(Unit unit)
        {
            switch (unit)
            {
                case Unit.Piece: return "Piece";
                case Unit.Weight: return "kg";
                case Unit.Volume: return "l";
                default: return "";
            }
        }
    }
}
