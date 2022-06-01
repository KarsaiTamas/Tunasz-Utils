using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
namespace TunaszUtils
{
    public struct MathExtra
{
    public static bool IsInAngleRange(float rotation,float targetRotation,float angleRange)
    {
        return (Mathf.Abs(Mathf.DeltaAngle(rotation, targetRotation)) > angleRange);
    }
    /// <summary>
    /// Calculate the closest point where the agent should go
    /// </summary>
    /// <param name="a">Target</param>
    /// <param name="b">Mob</param>
    /// <returns></returns>
    public static Vector2 CalculateClosestRadiusPoint(Vector2 a, Vector2 b, float distance)
    {
        float x, y;
        x = a.x + distance * ((b.x - a.x) /
            (Mathf.Sqrt(Mathf.Pow((b.x - a.x), 2) + Mathf.Pow((b.y - a.y), 2))));
        y = a.y + distance * ((b.y - a.y) /
            (Mathf.Sqrt(Mathf.Pow((b.x - a.x), 2) + Mathf.Pow((b.y - a.y), 2))));
        Vector2 returnValue = new Vector2(x, y);
        //keepDistance
        return returnValue;
    }
    public static Vector3 RandomDestination(Transform origin, float distance, LayerMask layerMask)
    {

        Vector3 vector;
        //var grid = ground.layoutGrid;
        vector = Random.insideUnitCircle * distance;

        //var cellToGo = grid.WorldToCell(vector);
        while (!CanMoveToPos(origin, vector, distance, layerMask))
        {
            distance = 5 + Random.Range(1, 5);

            vector = Random.insideUnitCircle * distance;

        }
        vector += origin.position;
        return vector;


    }

    public static float GetLerpT(float a, float b, float lerpValue)
    {

        float returnValue;
        returnValue = lerpValue - a;
        float t = -a + b;
        returnValue /= t;
        return Mathf.Abs(returnValue);
    }
    /// <summary>
    /// Gets the angle between A and B
    /// </summary>
    /// <param name="a">Angle is based around this object</param>
    /// <param name="b">Compare object A with this object</param>
    /// <returns></returns>
    public static float GetAngle(Vector3 a, Vector3 b)
    {
        Vector3 dir = a - b;
        return Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg + 90f;
    }
    /// <summary>
    /// Get's the coordinates around an object from angle and distance
    /// </summary>
    /// <param name="circleMiddlePos">The circle's middle position</param>
    /// <param name="angle">The angle where you want to get a position</param>
    /// <param name="radius">Distance between the middle point and the edge</param>
    /// <returns></returns>
    public static Vector3 GetCoordinatesFromAngleGDistance(Vector3 circleMiddlePos,float angle, float radius)
    {
        var radians = angle * Mathf.Deg2Rad;
        var mP = circleMiddlePos;
        var x = Mathf.Cos(radians) * radius + mP.x;
        var y = Mathf.Sin(radians) * radius+ mP.y;
        
        return new Vector3(x, y, 0); 
    }
        public static Vector3 GetCoordinatesFromAngleGDistance(Vector3 pos, Vector3 dir, float distance)
        { 
            Ray r = new Ray(pos, dir); 
            return r.GetPoint(distance);
        }
        /// <summary>
        /// Gets a position using an object and it's direction.
        /// </summary>
        /// <param name="obj1"></param>
        /// <param name="dir1"></param>
        /// <param name="distance"></param>
        /// <param name="lerpT">This should be between 0-1</param>
        /// <returns></returns>
        public static Vector3 GetPosition(
        Vector3 obj,
        Vector3 dir,
        float distance,
        float lerpT
        )
        { 
            return GetCoordinatesFromAngleGDistance(obj, dir, Mathf.Lerp (0,distance,lerpT));

        }
        /// <summary>
        /// Get a position next to object A. Based on the angle between object A and B,
        /// and the position.
        /// </summary>
        /// <param name="pos">Exact position between the min and max, should be between 0-1</param>
        /// <param name="objectTobeAround">Coordinate going to be around this object</param>
        /// <param name="objectToCalcTheAngle">Comparing object which we get the angle from</param>
        /// <param name="randomAngle">Offset to the angle</param>
        /// <param name="randomCoord">Lerping coordinate between min and max distance</param>
        /// <param name="minRadius">Minimum distance between the 2 objects</param>
        /// <param name="maxRadius">Maximum distance between the 2 objects</param>
        /// <param name="offSet">
        /// <br>90 Half circle in front of the object</br>
        /// <br>270 Half circle behind of the object</br>
        /// </param>
        /// <returns></returns>
        public static Vector3 GetCoordinatesBetween2Objects( 
        Vector3 objectTobeAround,
        Vector3 objectToCalcTheAngle,
        float pos,
        float randomAngle=0,
        float randomCoord=0.5f,
        float minRadius=0f,
        float maxRadius= 1f,
        float offSet=0
        )
    { 

        //get the angle between objectTobeAround and objectToCalcTheAngle
        float angleBetween = GetAngle(objectTobeAround, objectToCalcTheAngle);
        angleBetween += offSet;
        //Get the angle based on guardPos within 180 radius
        float angle = Mathf.LerpAngle(angleBetween - 90, angleBetween + 90,
            pos);
        //turn the angle into a coordinate
        //give it a small random angle range
        //give it the ability to walk between the minimum and maximum leash range
        return GetCoordinatesFromAngleGDistance(objectTobeAround, randomAngle + angle,
            Mathf.Lerp(minRadius, maxRadius, randomCoord));

    }
    /// <summary>
    /// Get a coordinate so objectTobeBehind will be sandwiched between this coordinate and objectToCalcTheAngle
    /// </summary>
    /// <param name="objectTobeBehind"></param>
    /// <param name="objectToCalcTheAngle"></param>
    /// <param name="radius"></param>
    /// <returns></returns>
    public static Vector3 GetCoordinatesBetween2ObjectsBehind1OfThem(
        Vector3 objectTobeBehind,
        Vector3 objectToCalcTheAngle,
        float radius
        )
    {
        //get the angle 
        float angleBetween = GetAngle(objectTobeBehind, objectToCalcTheAngle);
        angleBetween += 180;
        //turn the angle into a coordinate
        return GetCoordinatesFromAngleGDistance(objectTobeBehind, angleBetween, radius);

    }

    public static bool CanMoveToPos(Transform transform,Vector3 dir, float distance,LayerMask layerMask)
    {
        var ray = Physics2D.Raycast(transform.position, dir, distance, layerMask);
        return ray.collider==null;
    }

    public static bool CanMoveToPos(Transform transform, Vector3 dir, float distance, LayerMask layerMask, out RaycastHit2D raycastHit2D)
    {
        var ray = Physics2D.Raycast(transform.position, dir, distance, layerMask);
        raycastHit2D = ray;
        return ray.collider == null;
    }


    public static bool TryMove(Transform transform, Vector3 dir, float distance, LayerMask layerMask)
    {
        Vector3 moveDir = dir;

        bool canMove = CanMoveToPos(transform, moveDir, distance, layerMask);

        
        if (!canMove)
        {
            moveDir = new Vector3(dir.x, 0f).normalized;
            canMove = moveDir.x != 0f && CanMoveToPos(transform, moveDir, distance, layerMask);
            if (!canMove)
            {
                moveDir = new Vector3(0f, dir.y).normalized;
                canMove = moveDir.y != 0f && CanMoveToPos(transform, moveDir, distance, layerMask);
                
            }
        }
        Debug.Log(canMove);
        //if (canMove) transform.position += moveDir * distance;
        return canMove;
    }
    /// <summary>
    /// Gives the fraction of a percentage
    /// </summary>
    /// <param name="percentageNumber"></param>
    /// <returns></returns>
    public static float PercentageFraction(float percentageNumber)
    {
        return percentageNumber/100;
    }
}
}
