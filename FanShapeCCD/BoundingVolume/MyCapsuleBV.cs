using FK_CLI;

namespace FanShapeCCD
{
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

        public override bool PointInOutCheck(fk_Vector point)
        {
            double dist = DistanceCalculation.Point_Segment(point, start, end);
            if (dist > rad) return false;
            return true;
        }
    }
}
