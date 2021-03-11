using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;
using View.ViewModels.ComponentServices;

namespace View.ViewModels.ShapeServices
{
    public class ShapeServices
    {
        public FrameworkElement GetDefaultControl(ComponentEnum controlEnum)
        {
            if (controlEnum == ComponentEnum.TextBox)
            {
                return new TextBoxComponent();
            }
            else
            {

                return new ShapeComponent(GetDefaultShapeGeometry(controlEnum), controlEnum);
            }
        }
        public Geometry GetDefaultShapeGeometry(ComponentEnum control)
        {

            switch (control)
            {
                case ComponentEnum.Ecllipse:

                    return new EllipseGeometry(new Point(25, 25), 25, 25);
                case ComponentEnum.Triangle:

                    return Geometry.Parse("M25, 0 L50, 50 L0, 50Z");
                case ComponentEnum.Rectangle:
                    return Geometry.Parse("M0,0 L50,0 L50,50 L0,50Z");
                default:
                    return LineGeometry.Parse("M0,0 L50,0");

            }
        }
    }
}

