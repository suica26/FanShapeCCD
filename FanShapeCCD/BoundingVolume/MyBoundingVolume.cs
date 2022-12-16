using FK_CLI;
using System;

namespace FanShapeCCD
{
    public abstract class MyBoundingVolume
    {
        public fk_Vector position;

        public MyBoundingVolume() { position = new fk_Vector(); }
        public MyBoundingVolume(fk_Vector p) { position = new fk_Vector(p.x, p.y, p.z); }

        public abstract fk_Shape GetShape();
        public virtual void SyncModel(fk_Model argModel)
        {
            var p = argModel.Position;
            position.Set(p.x, p.y, p.z);
        }

        public abstract bool PointInOutCheck(fk_Vector P);

        public abstract fk_Vector Support(fk_Vector D);
    }
}