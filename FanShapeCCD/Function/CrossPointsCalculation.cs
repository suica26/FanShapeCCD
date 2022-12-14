using FK_CLI;
using System;
using System.Collections.Generic;

namespace FanShapeCCD
{
    static public class CrossPointsCalculation2D
    {
        static public class SameShape
        {
            static public fk_Vector Line_Line()
            {
                return null;
            }

            static public fk_Vector Segment_Segment()
            {
                return null;
            }

            static public List<fk_Vector> Circle_Circle()
            {
                return null;
            }

            static public List<fk_Vector> Box_Box()
            {
                return null;
            }
        }
        static public class OtherShape
        {
            static public List<fk_Vector> Line_Circle()
            {
                return null;
            }

            static public List<fk_Vector> Segment_Circle()
            {
                return null;
            }

            static public List<fk_Vector> FanShape_Box()
            {
                return null;
            }

            static public List<fk_Vector> FanShape_Circle()
            {
                return null;
            }
        }
    }
}
