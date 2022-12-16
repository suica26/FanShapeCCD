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

        public override bool PointInOutCheck(fk_Vector P)
        {
            if (GetMaxVal(fk_Axis.X) < P.x || GetMinVal(fk_Axis.X) > P.x) return false;
            if (GetMaxVal(fk_Axis.Y) < P.y || GetMinVal(fk_Axis.Y) > P.y) return false;
            if (GetMaxVal(fk_Axis.Z) < P.z || GetMinVal(fk_Axis.Z) > P.z) return false;
            return true;
        }

        public override fk_Vector Support(fk_Vector D)
        {
            var d = new fk_Vector(D.x, D.y, D.z);
            d.Normalize();

            var verticies = GetVertex();
            double max = d * verticies[0];
            var point = verticies[0];

            for (int i = 1; i < verticies.Length; i++)
            {
                if (d * verticies[i] > max)
                {
                    max = d * verticies[i];
                    point = verticies[i];
                }
            }

            return point;
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

        public fk_Vector[] GetVertex()
        {
            var vertices = new fk_Vector[8];
            var xVec = new fk_Vector(1.0, 0.0, 0.0) * width.x;
            var yVec = new fk_Vector(0.0, 1.0, 0.0) * width.y;
            var zVec = new fk_Vector(0.0, 0.0, 1.0) * width.z;

            vertices[0] = position + xVec + yVec + zVec;
            vertices[1] = position - xVec + yVec + zVec;
            vertices[2] = position - xVec + yVec - zVec;
            vertices[3] = position + xVec + yVec - zVec;
            vertices[4] = position + xVec - yVec + zVec;
            vertices[5] = position - xVec - yVec + zVec;
            vertices[6] = position - xVec - yVec - zVec;
            vertices[7] = position + xVec - yVec - zVec;

            return vertices;
        }
    }
}
