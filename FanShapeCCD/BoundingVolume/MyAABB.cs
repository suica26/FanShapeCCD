using FK_CLI;
using System;

namespace FanShapeCCD
{
    public class MyAABB : MyBoundingVolume
    {
        public fk_Vector width;

        public MyAABB() : base() { width = new fk_Vector(); }
        public MyAABB(fk_Vector p, fk_Vector w) : base(p) { width = new fk_Vector(w.x, w.y, w.z); }

        public override fk_Shape GetShape() { return new fk_Block(width.x, width.x, width.z); }

        public override void SyncModel(fk_Model argModel) { base.SyncModel(argModel); }

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
}
