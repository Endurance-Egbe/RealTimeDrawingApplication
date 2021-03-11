using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace View.ViewModels.Common.CustomColor
{
    public static class ColorCollectionsClass
    {
        public static ObservableCollection<CustomColor> ColorCollections { get; set; }
        public static ObservableCollection<CustomColor> LoadColorCollection()
        {
            return ColorCollections = new ObservableCollection<CustomColor>()
            {
                new CustomColor() { ColorName = "Red", ColorType = Brushes.Red },
                new CustomColor() { ColorName = "Purple", ColorType = Brushes.Purple },
                new CustomColor() { ColorName = "Green", ColorType = Brushes.Green },
                new CustomColor() { ColorName = "Blue", ColorType = Brushes.Blue },
                new CustomColor() { ColorName = "Yellow", ColorType = Brushes.Yellow },
                new CustomColor() { ColorName = "Black", ColorType = Brushes.Black },
                new CustomColor() { ColorName = "Pink", ColorType = Brushes.Pink },
                new CustomColor() { ColorName = "AliceBlue", ColorType = Brushes.AliceBlue },
                new CustomColor() { ColorName = "BlanchedAlmond", ColorType = Brushes.BlanchedAlmond },
                new CustomColor() { ColorName = "Brown", ColorType = Brushes.Brown },
                new CustomColor() { ColorName = "Cyan", ColorType = Brushes.Cyan },
                new CustomColor() { ColorName = "Firebrick", ColorType = Brushes.Firebrick },
                new CustomColor() { ColorName = "LightCoral", ColorType = Brushes.LightCoral },
                new CustomColor() { ColorName = "Maroon", ColorType = Brushes.Maroon },
                new CustomColor() { ColorName = "Wheat", ColorType = Brushes.Wheat },
                new CustomColor() { ColorName = "WhiteSmoke", ColorType = Brushes.WhiteSmoke },
                new CustomColor() { ColorName = "Gold", ColorType = Brushes.Gold },
                new CustomColor() { ColorName = "Goldenrod", ColorType = Brushes.Goldenrod },
                new CustomColor() { ColorName = "Gray", ColorType = Brushes.Gray },
                new CustomColor() { ColorName = "Honeydew", ColorType = Brushes.Honeydew },
                new CustomColor() { ColorName = "HotPink", ColorType = Brushes.HotPink },
                new CustomColor() { ColorName = "IndianRed", ColorType = Brushes.IndianRed },
                new CustomColor() { ColorName = "Ivory", ColorType = Brushes.Ivory },
                new CustomColor() { ColorName = "Khaki", ColorType = Brushes.Khaki },
                new CustomColor() { ColorName = "LemonChiffon", ColorType = Brushes.LemonChiffon },
                new CustomColor() { ColorName = "LightBlue", ColorType = Brushes.LightBlue },
                new CustomColor() { ColorName = "Lime", ColorType = Brushes.Lime },
                new CustomColor() { ColorName = "Aquamarine", ColorType = Brushes.Aquamarine },
                new CustomColor() { ColorName = "CadetBlue", ColorType = Brushes.CadetBlue },
                new CustomColor() { ColorName = "DodgerBlue", ColorType = Brushes.DodgerBlue },
                new CustomColor() { ColorName = "GhostWhite", ColorType = Brushes.GhostWhite },
                new CustomColor() { ColorName = "Orange", ColorType = Brushes.Orange },
            };
        }

    }
}
