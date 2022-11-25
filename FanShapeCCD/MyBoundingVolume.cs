using FK_CLI;
using System;
using System.Runtime.CompilerServices;
using System.Security.Cryptography.X509Certificates;

namespace FanShapeCCD
{
    public abstract class MyBoundingVolume
    {
        public fk_Vector position;

        public MyBoundingVolume() { position = new fk_Vector(); }
        public MyBoundingVolume(fk_Vector p) { position = new fk_Vector(p.x, p.y, p.z); }

        abstract public fk_Shape GetShape();
        virtual public void SyncModel(fk_Model argModel) { }
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

        public override fk_Shape GetShape() { return new fk_Sphere(8, rad); }
        public override void SyncModel(fk_Model argModel) { base.SyncModel(argModel); }
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

        public override fk_Shape GetShape() { return new fk_Capsule(8, (start - end).Dist(), rad); }

        public override void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);
            double len = (end - start).Dist() / 2;
            start = position + argModel.Vec * len;
            end = position - argModel.Vec * len;
        }
    }

    public class MyAABB : MyBoundingVolume
    {
        public fk_Vector width;

        public MyAABB() : base() { width = new fk_Vector(); }
        public MyAABB(fk_Vector p, fk_Vector w) : base(p) { width = new fk_Vector(w.x, w.y, w.z); }

        public override fk_Shape GetShape() { return new fk_Block(width.x, width.x, width.z); }

        public override void SyncModel(fk_Model argModel) { base.SyncModel(argModel); }

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

        public override fk_Shape GetShape() { return new fk_Block(width.x, width.y, width.z); }

        public override void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);
            SetAxis(argModel.Vec ^ argModel.Upvec, argModel.Upvec, argModel.Vec);
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
        public fk_Vector centerAxis; //扇形の中心軸
        public fk_Vector upVec; //3次元用　厚み方向の単位ベクトル
        public double cosVal;   //半角のコサイン値
        public double sRad, lRad, height;    //内円(球)半径、外円(球)半径、厚み

        public MyFanShapeBV() : base()
        {
            centerAxis = upVec = origin = new fk_Vector();
            cosVal = sRad = lRad = height = 0.0;
        }
        public MyFanShapeBV(fk_Vector O, fk_Vector A, fk_Vector U, double theta, double s, double l, double h)
        {
            origin = new fk_Vector(O.x, O.y, O.z);
            centerAxis = new fk_Vector(A.x, A.y, A.z);
            upVec = new fk_Vector(U.x, U.y, U.z);
            cosVal = Math.Cos(theta);
            sRad = s;
            lRad = l;
            height = h;
        }

        public override fk_Shape GetShape()
        {
            return null;
        }

        public override void SyncModel(fk_Model argModel)
        {
            base.SyncModel(argModel);
        }
    }
}