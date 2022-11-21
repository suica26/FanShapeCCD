using FK_CLI;
using System;

namespace FanShapeCCD
{
    public abstract class MyBoundingVolume
    {
        public fk_Vector position;

        public MyBoundingVolume() { position = new fk_Vector(); }
        public MyBoundingVolume(fk_Vector p) { position = new fk_Vector(p.x, p.y, p.z); }
    }

    public class MySphereBV : MyBoundingVolume
    {
        public double rad
        {
            get { return rad; }
            set { if(value < 0) rad = 0; }
        }

        public MySphereBV() :base() { rad = 0.0; }
        public MySphereBV(fk_Vector p, double r) : base(p) { rad = r; }
    }

    public class MyCapsuleBV : MyBoundingVolume
    {
        public double rad;
        public fk_Vector start, end;
        public MyCapsuleBV() : base() { start = new fk_Vector(); end = new fk_Vector(); }
        public MyCapsuleBV(fk_Vector p, fk_Vector s, fk_Vector e, double r) : base(p)
        {
            rad = r;
            start = new fk_Vector(s.x, s.y, s.z);
            end = new fk_Vector(e.x, e.y, e.z);
        }
    }

    public class MyAABB : MyBoundingVolume
    {
        public fk_Vector width;

        public MyAABB() :base() { width = new fk_Vector(); }
        public MyAABB(fk_Vector p, fk_Vector w) : base(p) { width = new fk_Vector(w.x, w.y, w.z); }
    }

    public class MyOBB : MyBoundingVolume
    {
        public fk_Vector width;
        protected fk_Vector[] localAxis;

        public MyOBB() : base() { SetAxis(new fk_Vector(), new fk_Vector(), new fk_Vector()); }
        public MyOBB(fk_Vector p, fk_Vector axisX, fk_Vector axisY, fk_Vector axisZ, fk_Vector w) : base(p) 
        {
            width = new fk_Vector(w.x, w.y, w.z);
            SetAxis(axisX, axisY, axisZ); 
        }

        public fk_Vector GetAxis(fk_Axis axis)
        {
            switch (axis)
            {
                case fk_Axis.X: return localAxis[0];
                case fk_Axis.Y: return localAxis[1];
                case fk_Axis.Z: return localAxis[2];
                    default:
                    Console.WriteLine("Please select 1 axis");
                        return null;
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
                if (!localAxis[i].Normalize()) Console.WriteLine($"Zero Vector is seted to OBB's Axis{i}");
        }
    }

    public class MyFanShapeBV : MyBoundingVolume
    {
        public fk_Vector cVec;
        public fk_Vector hVec;
        public fk_Vector rotOrigin;
        public double cosVal;
        public double sRad, lRad, width;
    }
}
