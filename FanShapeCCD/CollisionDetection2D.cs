using FK_CLI;
using System;

namespace FanShapeCCD
{
    static public class DCD2D
    {
        internal class SameBV
        {
            static public bool SphereSphere(MySphereBV sphere1, MySphereBV sphere2)
            {
                return true;
            }

            static public bool CapsuleCapsule(MyCapsuleBV capsule1, MyCapsuleBV capsule2)
            {
                return true;
            }

            static public bool AABBAABB(MyAABB aabb1, MyAABB aabb2)
            {
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

        internal class OtherBV 
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
