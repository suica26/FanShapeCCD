using FK_CLI;
using System;

namespace FanShapeCCD
{
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

        public override bool PointInOutCheck(fk_Vector point)
        {
            double dist = (position - point).Dist();
            if (dist > rad) return false;
            return true;
        }
    }
}
