using FK_CLI;
using System;
using System.Collections.Generic;
using System.Xml.Serialization;

namespace FanShapeCCD
{
    static public class GetCrossPoints2D
    {
        static public class SameShape
        {
            static public fk_Vector LineLine()
            {
                return null;
            }

            static public fk_Vector SegmentSegment()
            {
                return null;
            }

            static public List<fk_Vector> CircleCircle()
            {
                return null;
            }

            static public List<fk_Vector> BoxBox()
            {
                return null;
            }
        }
        static public class OtherShape
        {
            static public List<fk_Vector> LineCircle()
            {
                return null;
            }

            static public List<fk_Vector> SegmentCircle()
            {
                return null;
            }

            static public List<fk_Vector> FanshapeBox()
            {
                return null;
            }

            static public List<fk_Vector> FanshapeCircle()
            {
                return null;
            }
        }
    }
}
