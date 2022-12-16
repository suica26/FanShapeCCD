using FK_CLI;
using System;
using System.Collections.Generic;

namespace FanShapeCCD
{
    static public class CrossPointsCalculation2D
    {
        static public class SameShape
        {
            //y = ax + b と y = cx + d について解く
            static public fk_Vector Line_Line(fk_Vector A, fk_Vector B, fk_Vector C, fk_Vector D)
            {
                var AB = B - A;
                var CD = D - C;
                double a = AB.y / AB.x;
                double b = B.y - a * B.x;
                double c = CD.y / CD.x;
                double d = D.y - c * D.x;

                //直線同士が平行な場合はnull
                if(a == c) return null;

                double x = (d - b) / (a - c);
                return new fk_Vector(x, a * x + b);
            }

            // 参考URL
            // https://qiita.com/zu_rin/items/09876d2c7ec12974bc0f
            static public fk_Vector Segment_Segment(fk_Vector A, fk_Vector B, fk_Vector C, fk_Vector D)
            {
                double deno = ((B - A) ^ (D - C)).z;
                //直線が平行な場合はnull
                if (deno == 0.0) return null;

                double s = ((C - A) ^ (D - C)).z / deno;
                double t = ((B - A) ^ (A - C)).z / deno;
                //線分が交差していない場合
                if (s < 0.0 || 1.0 < s || t < 0.0 || 1.0 < t) return null;

                return A + (B - A) * s;
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
