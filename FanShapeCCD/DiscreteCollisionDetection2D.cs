using FK_CLI;
using System;

namespace FanShapeCCD
{
    static public class DCD2D
    {
        static public class SameBV
        {
            static public bool SphereSphere(MySphereBV sphere1, MySphereBV sphere2)
            {
                double dist = (sphere1.position - sphere2.position).Dist();
                double sumRad = sphere1.rad + sphere2.rad;

                if (dist > sumRad) return false;
                return true;
            }

            static public bool CapsuleCapsule(MyCapsuleBV capsule1, MyCapsuleBV capsule2)
            {
                double dist = DistCalc.SegmentSegment(capsule1.start, capsule1.end, capsule2.start, capsule2.end);
                double sumRad = capsule1.rad + capsule2.rad;

                if (dist > sumRad) return false;

                return true;
            }

            static public bool AABBAABB(MyAABB aabb1, MyAABB aabb2)
            {
                if (aabb1.GetMinVal(fk_Axis.X) > aabb2.GetMaxVal(fk_Axis.X)) return false;
                if (aabb1.GetMaxVal(fk_Axis.X) < aabb2.GetMinVal(fk_Axis.X)) return false;
                if (aabb1.GetMinVal(fk_Axis.Y) > aabb2.GetMaxVal(fk_Axis.Y)) return false;
                if (aabb1.GetMaxVal(fk_Axis.Y) < aabb2.GetMinVal(fk_Axis.Y)) return false;

                return true;
            }

            static public bool OBBOBB(MyOBB obb1, MyOBB obb2)
            {
                return true;
            }

            static public bool FanShapeFanShape(MyFanShapeBV fanshape1, MyFanShapeBV fanshpae2)
            {
                return true;
            }
        }

        static public class OtherBV
        {
            static public bool SphereCapsule(MySphereBV sphere, MyCapsuleBV capsule)
            {
                return true;
            }

            static public bool SphereAABB(MySphereBV sphere, MyAABB aabb)
            {
                return true;
            }

            static public bool SphereOBB(MySphereBV sphere, MyOBB obb)
            {
                return true;
            }

            static public bool SphereFanShape(MySphereBV sphere, MyFanShapeBV fanshape)
            {
                return true;
            }

            static public bool CapsuleAABB(MyCapsuleBV capsule, MyAABB aabb)
            {
                return true;
            }

            static public bool CapsuleOBB(MyCapsuleBV capsule, MyOBB obb)
            {
                return true;
            }

            static public bool CapsuleFanShape(MyCapsuleBV capsule, MyFanShapeBV fanshape)
            {
                return true;
            }

            static public bool AABBOBB(MyAABB aabb, MyOBB obb)
            {
                return true;
            }

            static public bool AABBFanShape(MyAABB aabb, MyFanShapeBV fanshape)
            {
                return true;
            }

            static public bool OBBFanShape(MyOBB obb, MyFanShapeBV fanshape)
            {
                return true;
            }
        }
    }
}
