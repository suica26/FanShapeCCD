using System;
using FK_CLI;

namespace FanShapeCCD
{
    public class MyFanShapeBV : MyBoundingVolume
    {
        //同心円(球)の中心点はpositionに格納
        public fk_Vector center; //扇形の中心軸
        public fk_Vector upVec; //3次元用　厚み方向の単位ベクトル
        public double cosVal;   //半角のコサイン値
        public double sRad, lRad, height;    //内円(球)半径、外円(球)半径、厚み

        public MyFanShapeBV() : base()
        {
            center = upVec = new fk_Vector();
            cosVal = sRad = lRad = height = 0.0;
        }
        public MyFanShapeBV(fk_Vector P, fk_Vector A, fk_Vector U, double theta, double s, double l, double h)
        {
            position = new fk_Vector(P.x, P.y, P.z);
            center = new fk_Vector(A.x, A.y, A.z);
            upVec = new fk_Vector(U.x, U.y, U.z);
            cosVal = Math.Cos(theta);
            sRad = s;
            lRad = l;
            height = h;
        }

        public override fk_Shape GetShape()
        {
            const int V_NUM = 32;

            var ifs = new fk_IndexFaceSet();
            var pos = new fk_Vector[4 * (V_NUM + 1)];
            var IFSet = new int[4 * 4 * V_NUM + 8];

            int i, j, index;

            for (i = 0; i <= V_NUM; i++)
            {
                double t = -1.0 + 2.0 / (double)V_NUM * (double)i;
                pos[i * 4] = GetPoint(0.0, t, 1.0);
                pos[i * 4 + 1] = GetPoint(0.0, t, -1.0);
                pos[i * 4 + 2] = GetPoint(1.0, t, -1.0);
                pos[i * 4 + 3] = GetPoint(1.0, t, 1.0);
            }

            for (i = 0; i < V_NUM; i++)
            {
                for (j = 0; j < 4; j++)
                {
                    index = i * 16 + j * 4;
                    IFSet[index] = 4 * i + j;
                    IFSet[index + 1] = 4 * (i + 1) + j;
                    if (j != 3)
                    {
                        IFSet[index + 2] = 4 * (i + 1) + j + 1;
                        IFSet[index + 3] = 4 * i + j + 1;
                    }
                    else
                    {
                        IFSet[index + 2] = 4 * (i + 1);
                        IFSet[index + 3] = 4 * i;
                    }
                }
            }

            index = i * 16;

            for (i = 0; i < 4; i++)
            {
                IFSet[index + i] = i;
                IFSet[index + i + 4] = pos.Length - i - 1;
            }

            ifs.MakeIFSet(4 * V_NUM + 2, 4, IFSet, 4 * (V_NUM + 1), pos);

            return ifs;
        }

        public override void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);

            var v = argModel.Vec;
            center.Set(v.x, v.y, v.z);

            var u = argModel.Upvec;
            upVec.Set(u.x, u.y, u.z);
        }

        public override bool PointInOutCheck(fk_Vector point)
        {
            var p = new fk_Vector(point.x, point.y, point.z);
            fk_Vector pVec;

            //三次元の判定
            if (height != 0.0)
            {
                //平面までの距離を計算
                pVec = p - position;
                double planeDist = upVec * pVec;
                if (Math.Abs(planeDist) > height) return false;
                //厚み平面の内部にあるなら、点Pを平面に投影
                else p += upVec * -planeDist;
            }

            //二次元判定
            //距離比較
            pVec = p - position;
            double dist = pVec.Dist();
            if (dist < sRad || dist > lRad) return false;
            //方向比較
            pVec.Normalize();
            if (cosVal > pVec * center) return false;

            return true;
        }

        //扇形内の点を返す関数
        //s は 0 ~ 1
        //t は -1 ~ 1
        //h は -1 ~ 1
        public fk_Vector GetPoint(double s, double t, double h)
        {
            fk_Vector point;

            double len = (lRad - sRad) * s;
            double angle = Math.Acos(cosVal) * t;
            double cos = Math.Cos(angle);
            double sin = Math.Sin(angle);

            //奥行き成分
            point = center * (sRad + len);
            //幅成分
            point = cos * point + (1.0 - cos) * (point * upVec) * upVec + sin * (upVec ^ point);
            //高さ成分
            point = point + upVec * height * h;

            return point;
        }
    }
}
