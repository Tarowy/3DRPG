using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class EntendsionMethod //扩展方法
{
    private const float DotThreshold = 0.5f;
    public static bool IsFacingTarget(this Transform transform, Transform target)
    {
        var vectorDirection = target.position-transform.position; //函数调用者和target的方向
        vectorDirection.Normalize();

        var dot = Vector3.Dot(transform.forward, vectorDirection);
        return dot >= DotThreshold;
    } 
}
