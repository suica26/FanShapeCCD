using FK_CLI;
using System;

namespace FanShapeCCD
{
    static public class DistanceCalculation
    {
        //------------------------距離と最短点、媒介変数を求める関数----------------------------
        static public double Point_Line(fk_Vector P, fk_Vector LP, fk_Vector V, ref fk_Vector H, ref double t)
        {
            //媒介変数の計算
            t = (V * P - V * LP) / (V * V);
            //直線状の点
            H = LP + V * t;
            //最短距離の計算
            return (H - P).Dist();
        }

        static public double Point_Segment(fk_Vector P, fk_Vector S, fk_Vector E, ref fk_Vector H, ref double t)
        {
            //線分の方向ベクトル
            var V = E - S;
            double d = Point_Line(P, S, V, ref H, ref t);

            //始点よりも外側にある場合
            if(t < 0.0)
            {
                d = (P - S).Dist();
                H.Set(S.x, S.y, S.z);
                t = 0.0;
            }
            else if(t > 1.0) //終点よりも外側にある場合
            {
                d = (P - E).Dist();
                H.Set(E.x, E.y, E.z);
                t = 1.0;
            }

            return d;
        }

        static public double Line_Line(fk_Vector A, fk_Vector V, fk_Vector B, fk_Vector W, ref fk_Vector P, ref fk_Vector Q, ref double s, ref double t)
        {
            //内積値
            double VV = V * V;
            double VW = V * W;
            double WW = W * W;
            var AB = B - A;

            double tDeno = WW * VV - VW * VW;
            double tNume = V * AB * VW - W * AB * VV;

            t = tNume / tDeno;

            Q = B + W * t;

            s = V * (AB + W * t) / VV;

            P = A + V * s;

            return (P - Q).Dist();
        }

        static public double Segment_Segment(fk_Vector A, fk_Vector B, fk_Vector C, fk_Vector D, ref fk_Vector P, ref fk_Vector Q, ref double s, ref double t)
        {
            //線分ABの方向ベクトル
            fk_Vector V = B - A;
            //線分CDの方向ベクトル
            fk_Vector W = D - C;
            //最短距離を算出
            double d = Line_Line(A, V, C, W, ref P, ref Q, ref s, ref t);

            //最短点がどちらの線分上にも存在する場合には計算終了
            if ((0.0 <= s && s <= 1.0) && (0.0 <= t && t <= 1.0)) return d;

            //垂線の足が外にある事が判明
            //sを0～1の間にクランプして線分CDに垂線を降ろす
            s = fk_Math.Clamp(s, 0.0, 1.0);
            P = A + V * s; //点Pを計算
            d = Point_Line(P, C, W, ref Q, ref t);    //最短距離を計算しなおし
            if (0.0 <= t && t <= 1.0) return d;

            //tを0～1の間にクランプして線分ABに垂線を降ろす
            t = fk_Math.Clamp(t, 0.0, 1.0);
            Q = C + W * t; //点Qを計算
            d = Point_Line(Q, A, V, ref P, ref s);    //最短距離を計算しなおし
            if (0.0 <= s && s <= 1.0) return d;

            // 双方の端点が最短と判明
            s = fk_Math.Clamp(s, 0.0, 1.0);
            P = A + V * s;
            return d;
        }

        //---------------------------------距離だけを求める関数----------------------------------
        static public double Point_Line(fk_Vector P, fk_Vector LP, fk_Vector V)
        {
            //媒介変数の計算
            double t = (V * P - V * LP) / (V * V);
            //直線状の点
            fk_Vector H = LP + V * t;
            //最短距離の計算
            return (H - P).Dist();
        }

        static public double Point_Segment(fk_Vector P, fk_Vector S, fk_Vector E)
        {
            //線分の方向ベクトル
            var V = E - S;
            double t = 0.0;
            fk_Vector H = new fk_Vector();

            double d = Point_Line(P, S, V, ref H, ref t);

            //始点よりも外側にある場合
            if (t < 0.0)
            {
                d = (P - S).Dist();
            }
            else if (t > 1.0) //終点よりも外側にある場合
            {
                d = (P - E).Dist();
            }

            return d;
        }

        static public double Line_Line(fk_Vector A, fk_Vector V, fk_Vector B, fk_Vector W)
        {
            double VV = V * V;
            double VW = V * W;
            double WW = W * W;
            var AB = B - A;

            double tDeno = WW * VV - VW * VW;
            double tNume = V * AB * VW - W * AB * VV;
            double t = tNume / tDeno;

            fk_Vector Q = B + W * t;

            double s = V * (AB + W * t) / VV;

            fk_Vector P = A + V * s;

            return (P - Q).Dist();
        }

        static public double Segment_Segment(fk_Vector A, fk_Vector B, fk_Vector C, fk_Vector D)
        {
            //線分ABの方向ベクトル
            fk_Vector V = B - A;
            //線分CDの方向ベクトル
            fk_Vector W = D - C;

            fk_Vector P = new fk_Vector();
            fk_Vector Q = new fk_Vector();
            double s, t;
            s = t = 0.0;

            //最短距離を算出
            double d = Line_Line(A, V, C, W, ref P, ref Q, ref s, ref t);

            //最短点がどちらの線分上にも存在する場合には計算終了
            if ((0.0 <= s && s <= 1.0) && (0.0 <= t && t <= 1.0)) return d;

            //垂線の足が外にある事が判明
            //sを0～1の間にクランプして線分CDに垂線を降ろす
            s = fk_Math.Clamp(s, 0.0, 1.0);
            P = A + V * s; //点Pを計算
            d = Point_Line(P, C, W, ref Q, ref t);    //最短距離を計算しなおし
            if (0.0 <= t && t <= 1.0) return d;

            //tを0～1の間にクランプして線分ABに垂線を降ろす
            t = fk_Math.Clamp(t, 0.0, 1.0);
            Q = C + W * t; //点Qを計算
            d = Point_Line(Q, A, V, ref P, ref s);    //最短距離を計算しなおし
            if (0.0 <= s && s <= 1.0) return d;

            // 双方の端点が最短と判明
            s = fk_Math.Clamp(s, 0.0, 1.0);
            P = A + V * s;
            return d;
        }
    }
}
