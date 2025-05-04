using UnityEngine;

namespace _Project.Scripts.Main.Utilities.Scripts
{
    public static class MathUtil
    {
        /// <summary>
        /// Returns angle in radians
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public static float DirToAngle(float x, float y)
        {
            return Mathf.Atan2(y, x);
        }

        /// <summary>
        /// Returns vector from an angle
        /// </summary>
        /// <param name="rad"></param>
        /// <returns></returns>
        public static Vector2 AngleToDir(float rad)
        {
            return new Vector2(Mathf.Cos(rad), Mathf.Sin(rad));
        }
    
        /// <summary>
        /// returns the squared distance between two points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float SquaredDistance(Vector3 a,Vector3 b)
        {
            Vector2 dist = a - b;
            return dist.sqrMagnitude;
        }

        public static Vector2 Reflect(Vector2 dir, Vector2 normal)
        {
            return dir - 2 * Vector2.Dot(dir, normal) * normal;
        }
        
        /// <summary>
        /// returns an angle towards "to" direction in degrees
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static float LookAt2D(Vector3 from,Vector3 to)
        {
            Vector3 dir = to - from; 
            float angle = DirToAngle(dir.x, dir.y) * Mathf.Rad2Deg;
            return angle;
        }
        
        /// <summary>
        /// returns the point on a quadratic Bézier curve defined by three points
        /// see: https://devforum.roblox.com/t/can-anyone-explain-bezier-curves-in-the-most-simple-way-possible-please/693507/2
        /// </summary>
        /// <param name="t">A parameter that varies from 0 to 1, determining the position of a point on the curve</param>
        /// <param name="p0">The starting point of the curve</param> 
        /// <param name="p1">The control point that influences the curve's shape</param>
        /// <param name="p2">The ending point of the curve</param>
        /// <returns></returns>
        public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
        {
            float u = 1 - t;
            return u * u * p0 + 2 * u * t * p1 + t * t * p2;
        }

    }
}