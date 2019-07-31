using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelperFunctions
{ 
    /// <summary>
    /// Determines of an object is within the line of sight of another object
    /// </summary>
    /// <param name="objectForward">The forward direction vector of the viewing object</param>
    /// <param name="fromToVector">The direction vector to the object being viewed<param>
    /// <param name="angle">The desired line of sight angle</param>
    /// <returns>Returns true if the object is within the line of sight, otherwise returns false</returns>
    public static bool IsObjectFacingObject(Vector3 objectForward, Vector3 fromToVector, float angle)
    {
        float losAngle = Vector3.SignedAngle(objectForward, fromToVector, Vector3.up);
        //Debug.Log(Mathf.Abs(losAngle) <= angle);
        return Mathf.Abs(losAngle) <= angle;

    }

    /// <summary>
    /// Given a, b, and c this function calculates the zeros of a quadradric function using the quadratic formula
    /// </summary>
    /// <param name="a"></param>
    /// <param name="b"></param>
    /// <param name="c"></param>
    /// <returns>Reutns a Vector2 either containing the two zeros or Mathf.negativeInfinity if the radicand was less than zero</returns>
    public static Vector2 QuadraticEquationSolver(float a, float b, float c)
    {
        float x1;
        float x2;
        float radicand = Mathf.Pow(b, 2) - (4 * a * c);
        if (radicand < 0)
            return Vector2.negativeInfinity;
        x1 = ((-b) + Mathf.Sqrt(radicand)) / (2 * a);
        x2 = ((-b) - Mathf.Sqrt(radicand)) / (2 * a);
        return new Vector2(x1, x2);
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <returns></returns>
    public static Vector3 CalculateLinearBezierPoint(float t, Vector3 p0, Vector3 p1)
    {
        float u = 1 - t;
        Vector3 p = u * p0 + t * p1;
        return p;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <returns></returns>
    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2)
    {
        float u = 1 - t;
        float tt = t * t;
        float uu = u * u;
        Vector3 p = uu * p0;
        p += 2 * u * t * p1;
        p += tt * p2;
        return p;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="t"></param>
    /// <param name="p0"></param>
    /// <param name="p1"></param>
    /// <param name="p2"></param>
    /// <param name="p3"></param>
    /// <returns></returns>
    public static Vector3 CalculateQuadraticBezierPoint(float t, Vector3 p0, Vector3 p1, Vector3 p2, Vector3 p3)
    {
        float u = 1 - t;
        float u2 = u * u;
        float u3 = u * u * u;
        float t2 = t * t;
        float t3 = t * t * t;
        Vector3 p = u3 * p0;
        p += 3 * u2 * t * p1;
        p += 3 * u * t2 * p2;
        p += t3 * p3;
        return p;
    }
}
