using FK_CLI;
using System;

namespace FanShapeCCD
{
    static public class DCD3D
    {
        static public class SameBV
        {
            static public bool Sphere_Sphere(MySphereBV sphere1, MySphereBV sphere2)
            {
                double dist = (sphere1.position - sphere2.position).Dist();
                double sumRad = sphere1.rad + sphere2.rad;

                if (dist > sumRad) return false;
                return true;
            }

            static public bool Capsule_Capsule(MyCapsuleBV capsule1, MyCapsuleBV capsule2)
            {
                double dist = DistanceCalculation.Segment_Segment(capsule1.start, capsule1.end, capsule2.start, capsule2.end);
                double sumRad = capsule1.rad + capsule2.rad;

                if (dist > sumRad) return false;

                return true;
            }

            static public bool AABB_AABB(MyAABB aabb1, MyAABB aabb2)
            {
                if (aabb1.GetMinVal(fk_Axis.X) > aabb2.GetMaxVal(fk_Axis.X)) return false;
                if (aabb1.GetMaxVal(fk_Axis.X) < aabb2.GetMinVal(fk_Axis.X)) return false;
                if (aabb1.GetMinVal(fk_Axis.Y) > aabb2.GetMaxVal(fk_Axis.Y)) return false;
                if (aabb1.GetMaxVal(fk_Axis.Y) < aabb2.GetMinVal(fk_Axis.Y)) return false;
                if (aabb1.GetMinVal(fk_Axis.Z) > aabb2.GetMaxVal(fk_Axis.Z)) return false;
                if (aabb1.GetMaxVal(fk_Axis.Z) < aabb2.GetMinVal(fk_Axis.Z)) return false;

                return true;
            }

            static public bool OBB_OBB(MyOBB obb1, MyOBB obb2)
            {
                return true;
            }

            static public bool FanShape_FanShape(MyFanShapeBV fanShape1, MyFanShapeBV fanShpae2)
            {
                return true;
            }
        }

        static public class OtherBV
        {
            static public bool Sphere_Capsule(MySphereBV sphere, MyCapsuleBV capsule)
            {
                return true;
            }

            static public bool Sphere_AABB(MySphereBV sphere, MyAABB aabb)
            {
                return true;
            }

            static public bool Sphere_OBB(MySphereBV sphere, MyOBB obb)
            {
                return true;
            }

            static public bool Sphere_FanShape(MySphereBV sphere, MyFanShapeBV fanShape)
            {
                return true;
            }

            static public bool Capsule_AABB(MyCapsuleBV capsule, MyAABB aabb)
            {
                return true;
            }

            static public bool Capsule_OBB(MyCapsuleBV capsule, MyOBB obb)
            {
                return true;
            }

            static public bool Capsule_FanShape(MyCapsuleBV capsule, MyFanShapeBV fanShape)
            {
                return true;
            }

            static public bool AABB_OBB(MyAABB aabb, MyOBB obb)
            {
                return true;
            }

            static public bool AABB_FanShape(MyAABB aabb, MyFanShapeBV fanShape)
            {
                return true;
            }

            static public bool OBB_FanShape(MyOBB obb, MyFanShapeBV fanShape)
            {
                return true;
            }
        }
    }
}
