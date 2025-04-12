﻿using System;
using UnityEngine;

namespace _UTIL_
{
    public abstract class Interpolator<T> where T : struct
    {
        public T a, b;
        public float ta, tb;

        public T last;
        public float tlast;

        //----------------------------------------------------------------------------------------------------------

        public void OnValue(in T value, in float time)
        {
            a = last;
            ta = tlast;
            last = b = value;
            tlast = tb = time;
        }

        public T Interp(in float time)
        {
            tlast = time;

            float lerp = ta >= tb ? 1 : Mathf.InverseLerp(ta, tb, time);
            if (lerp >= 1)
                return last = b;

            return last = Lerp(lerp);
        }

        protected abstract T Lerp(in float lerp);

        public T InterpAround(in float time, in Vector3 pivot)
        {
            tlast = time;

            float lerp = Mathf.InverseLerp(ta, tb, time);
            if (lerp >= 1)
                return last = b;

            return last = SlerpAround(lerp, pivot);
        }

        protected abstract T SlerpAround(in float lerp, in Vector3 pivot);
    }

    [Serializable]
    public class PosDirInterpolator3D : Interpolator<PosDir3D>
    {
        protected override PosDir3D Lerp(in float lerp) => PosDir3D.Slerp(a, b, lerp);
        protected override PosDir3D SlerpAround(in float lerp, in Vector3 pivot)
        {
            Vector3 position = pivot + Vector3.Slerp(a.position - pivot, b.position - pivot, lerp);
            Vector3 direction = Vector3.Slerp(a.direction, b.direction, lerp);
            return (position, direction);
        }
    }

    [Serializable]
    public class InterpolatorV3 : Interpolator<Vector3>
    {
        protected override Vector3 Lerp(in float lerp) => Vector3.Lerp(a, b, lerp);
        protected override Vector3 SlerpAround(in float lerp, in Vector3 pivot) => pivot + Vector3.Slerp(a - pivot, b - pivot, lerp);
    }

    [Serializable]
    public class InterpolatorQuaternion : Interpolator<Quaternion>
    {
        protected override Quaternion Lerp(in float lerp) => Quaternion.Slerp(a, b, lerp);
        protected override Quaternion SlerpAround(in float lerp, in Vector3 pivot) => throw new NotImplementedException();
    }

    [Serializable]
    public class InterpolatorPosRot : Interpolator<PosRot>
    {
        protected override PosRot Lerp(in float lerp) => PosRot.Slerp(a, b, lerp);
        protected override PosRot SlerpAround(in float lerp, in Vector3 pivot) => throw new NotImplementedException();
    }

    [Serializable]
    public readonly struct RgbInfos1
    {
        public readonly Vector3 position;
        public readonly Vector3 velocity;

        //----------------------------------------------------------------------------------------------------------

        public RgbInfos1(in Vector3 position, in Vector3 velocity)
        {
            this.position = position;
            this.velocity = velocity;
        }

        //----------------------------------------------------------------------------------------------------------

        public static implicit operator (Vector3 position, Vector3 velocity)(in RgbInfos1 value) => (value.position, value.velocity);

        public static implicit operator RgbInfos1(in (Vector3 position, Vector3 velocity) value) => new(value.position, value.velocity);
    }
}