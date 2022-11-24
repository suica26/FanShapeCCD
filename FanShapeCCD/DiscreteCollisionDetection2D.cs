using FK_CLI;
using System;

namespace FanShapeCCD
{
    static public class DCD2D
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

                return true;
            }

            static public bool OBB_OBB(MyOBB obb1, MyOBB obb2)
            {
                //obbの各軸を取得
                fk_Vector nAX = obb1.GetAxis(fk_Axis.X);
                fk_Vector nAY = obb1.GetAxis(fk_Axis.Y);
                fk_Vector nBX = obb2.GetAxis(fk_Axis.X);
                fk_Vector nBY = obb2.GetAxis(fk_Axis.Y);

                //幅の大きさに対応した軸を計算
                fk_Vector AX = nAX * obb1.width.x;
                fk_Vector AY = nAY * obb1.width.y;
                fk_Vector BX = nBX * obb2.width.x;
                fk_Vector BY = nBY * obb2.width.y;

                double hlA, hlB, l;
                fk_Vector interval = obb1.position - obb2.position;

                //分離軸はnAX
                hlA = obb1.width.x;
                hlB = Math.Abs(nAX * BX) + Math.Abs(nAX * BY);
                l = interval * nAX;
                if (l >= hlA + hlB) return false;

                //分離軸はnAY
                hlA = obb1.width.y;
                hlB = Math.Abs(nAY * BX) + Math.Abs(nAY * BY);
                l = interval * nAY;
                if (l >= hlA + hlB) return false;

                //分離軸はnBX
                hlA = Math.Abs(nBX * AX) + Math.Abs(nBX * AY);
                hlB = obb2.width.x;
                l = interval * nBX;
                if (l >= hlA + hlB) return false;

                //分離軸はnBY
                hlA = Math.Abs(nBY * AX) + Math.Abs(nBY * AY);
                hlB = obb2.width.y;
                l = interval * nBY;
                if (l >= hlA + hlB) return false;

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
