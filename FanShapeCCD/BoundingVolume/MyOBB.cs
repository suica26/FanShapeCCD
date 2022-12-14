using System;
using FK_CLI;

namespace FanShapeCCD
{
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
}
