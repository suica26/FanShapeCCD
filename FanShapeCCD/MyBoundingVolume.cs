using FK_CLI;
using System;
using System.Runtime.CompilerServices;

namespace FanShapeCCD
{
    public abstract class MyBoundingVolume
    {
        public fk_Vector position;

        public MyBoundingVolume() { position = new fk_Vector(); }
        public MyBoundingVolume(fk_Vector p) { position = new fk_Vector(p.x, p.y, p.z); }

        abstract public fk_Shape GetShape();
        virtual public void SyncModel(fk_Model argModel) 
        {
            var p = argModel.Position;
            position.Set(p.x, p.y, p.z);
        }

        abstract public bool PointInOutCheck(fk_Vector point);
    }

    public class MySphereBV : MyBoundingVolume
    {
        public double rad
        {
            get { return rad; }
            set { if (value < 0) rad = 0; }
        }

        public MySphereBV() : base() { rad = 0.0; }
        public MySphereBV(fk_Vector p, double r) : base(p) { rad = r; }

        override public fk_Shape GetShape() { return new fk_Sphere(8, rad); }

        override public void SyncModel(fk_Model argModel) { base.SyncModel(argModel); }

        public override bool PointInOutCheck(fk_Vector point)
        {
            double dist = (position - point).Dist();
            if (dist > rad) return false;
            return true;
        }
    }

    public class MyCapsuleBV : MyBoundingVolume
    {
        public double rad;
        public fk_Vector start, end;

        public MyCapsuleBV() : base() { rad = 0.0; start = new fk_Vector(); end = new fk_Vector(); }
        public MyCapsuleBV(fk_Vector s, fk_Vector e, double r)
        {
            rad = r;
            start = new fk_Vector(s.x, s.y, s.z);
            end = new fk_Vector(e.x, e.y, e.z);
            position = start + (end - start) / 2;
        }

        override public fk_Shape GetShape() { return new fk_Capsule(8, (start - end).Dist(), rad); }

        override public void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);
            double len = (end - start).Dist() / 2;
            start = position + argModel.Vec * len;
            end = position - argModel.Vec * len;
        }

        public override bool PointInOutCheck(fk_Vector point)
        {
            double dist = DistanceCalculation.Point_Segment(point, start, end);
            if (dist > rad) return false;
            return true;
        }
    }

    public class MyAABB : MyBoundingVolume
    {
        public fk_Vector width;

        public MyAABB() : base() { width = new fk_Vector(); }
        public MyAABB(fk_Vector p, fk_Vector w) : base(p) { width = new fk_Vector(w.x, w.y, w.z); }

        override public fk_Shape GetShape() { return new fk_Block(width.x, width.x, width.z); }

        override public void SyncModel(fk_Model argModel) { base.SyncModel(argModel); }

        public override bool PointInOutCheck(fk_Vector point)
        {
            if (GetMaxVal(fk_Axis.X) < point.x || GetMinVal(fk_Axis.X) > point.x) return false;
            if (GetMaxVal(fk_Axis.Y) < point.y || GetMinVal(fk_Axis.Y) > point.y) return false;
            if (GetMaxVal(fk_Axis.Z) < point.z || GetMinVal(fk_Axis.Z) > point.z) return false;
            return true;
        }

        public double GetMaxVal(fk_Axis axis)
        {
            switch (axis)
            {
                case fk_Axis.X: return position.x + width.x;
                case fk_Axis.Y: return position.y + width.y;
                case fk_Axis.Z: return position.z + width.z;
                default: Console.WriteLine("Please select 1 axis"); return 0.0;
            }
        }

        public double GetMinVal(fk_Axis axis)
        {
            switch (axis)
            {
                case fk_Axis.X: return position.x - width.x;
                case fk_Axis.Y: return position.y - width.y;
                case fk_Axis.Z: return position.z - width.z;
                default: Console.WriteLine("Please select 1 axis"); return 0.0;
            }
        }
    }

    public class MyOBB : MyBoundingVolume
    {
        public fk_Vector width;
        protected fk_Vector[] localAxis;

        public MyOBB() : base()
        {
            width = new fk_Vector();
            localAxis = new fk_Vector[3];
            for (int i = 0; i < 3; i++) localAxis[i] = new fk_Vector();
        }
        //2次元用
        public MyOBB(fk_Vector p, fk_Vector axisX, fk_Vector axisY, fk_Vector w) : base(p)
        {
            width = new fk_Vector(w.x, w.y, w.z);
            SetAxis(axisX, axisY, new fk_Vector(0.0, 0.0, 1.0));
        }
        //3次元用
        public MyOBB(fk_Vector p, fk_Vector axisX, fk_Vector axisY, fk_Vector axisZ, fk_Vector w) : base(p)
        {
            width = new fk_Vector(w.x, w.y, w.z);
            SetAxis(axisX, axisY, axisZ);
        }

        override public fk_Shape GetShape() { return new fk_Block(width.x, width.y, width.z); }

        override public void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);
            SetAxis(argModel.Vec ^ argModel.Upvec, argModel.Upvec, argModel.Vec);
        }

        public override bool PointInOutCheck(fk_Vector point)
        {
            fk_Vector pVec = point - position;
            if (Math.Abs(GetAxis(fk_Axis.X) * pVec) > width.x / 2.0) return false;
            if (Math.Abs(GetAxis(fk_Axis.Y) * pVec) > width.y / 2.0) return false;
            if (Math.Abs(GetAxis(fk_Axis.Z) * pVec) > width.z / 2.0) return false;
            return true;
        }

        public fk_Vector GetAxis(fk_Axis axis)
        {
            switch (axis)
            {
                case fk_Axis.X: return localAxis[0];
                case fk_Axis.Y: return localAxis[1];
                case fk_Axis.Z: return localAxis[2];
                default: return null;
            }
        }

        public void SetAxis(fk_Vector lX, fk_Vector lY, fk_Vector lZ)
        {
            if (localAxis == null)
            {
                localAxis = new fk_Vector[3];
                for (int i = 0; i < 3; i++) localAxis[i] = new fk_Vector();
            }

            localAxis[0].Set(lX.x, lX.y, lX.z);
            localAxis[1].Set(lY.x, lY.y, lY.z);
            localAxis[2].Set(lZ.x, lZ.y, lZ.z);

            for (int i = 0; i < 3; i++)
                if (!localAxis[i].Normalize()) Console.WriteLine($"Zero Vector is set to OBB's Axis{i}");
        }
    }

    public class MyFanShapeBV : MyBoundingVolume
    {
        public fk_Vector origin; //同心円(球)の中心点
        public fk_Vector center; //扇形の中心軸
        public fk_Vector upVec; //3次元用　厚み方向の単位ベクトル
        public double cosVal;   //半角のコサイン値
        public double sRad, lRad, height;    //内円(球)半径、外円(球)半径、厚み

        public MyFanShapeBV() : base()
        {
            center = upVec = origin = new fk_Vector();
            cosVal = sRad = lRad = height = 0.0;
        }
        public MyFanShapeBV(fk_Vector O, fk_Vector A, fk_Vector U, double theta, double s, double l, double h)
        {
            origin = new fk_Vector(O.x, O.y, O.z);
            center = new fk_Vector(A.x, A.y, A.z);
            upVec = new fk_Vector(U.x, U.y, U.z);
            cosVal = Math.Cos(theta);
            sRad = s;
            lRad = l;
            height = h;
        }

        override public fk_Shape GetShape()
        {
            return null;
        }

        override public void SyncModel(fk_Model argModel)
        {
            var p = argModel.Position;
            origin.Set(p.x, p.y, p.z);
            var v = argModel.Vec;
            center.Set(v.x, v.y, v.z);
            var u = argModel.Upvec;
            upVec.Set(u.x, u.y, u.z);
            position = origin + center * (sRad + (lRad + sRad) / 2.0);
        }

        public override bool PointInOutCheck(fk_Vector point)
        {
            var p = new fk_Vector(point.x, point.y, point.z);
            fk_Vector pVec;

            //三次元の判定
            if(height != 0.0)
            {
                //平面までの距離を計算
                pVec = p - origin;
                double planeDist = upVec * pVec;
                if (Math.Abs(planeDist) > height) return false;
                //扇形内部にあるなら、点Pを平面に投影
                else p += upVec * planeDist;
            }

            //二次元判定
            //距離比較
            pVec = p - origin;
            double dist = pVec.Dist();
            if (dist < sRad || dist > lRad) return false;
            //方向比較
            pVec.Normalize();
            if(cosVal > pVec * center) return false;

            return true;
        }
    }
}