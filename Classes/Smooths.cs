﻿using UnityEngine;

namespace _UTIL_
{
    public static class Util_smooths
    {
        public static bool NO_SMOOTH;
    }

    public abstract class Smooth<T> : OnValue<T> where T : struct
    {
        public T velocity;
        public T target, delta;
        public abstract bool isUp { get; }

        //----------------------------------------------------------------------------------------------------------

        public Smooth(in T init = default) : base(init) { target = init; }

        //----------------------------------------------------------------------------------------------------------

        public override void ForceUpdate(T value)
        {
            base.ForceUpdate(value);
            target = value;
        }

        public virtual void Reset(T value)
        {
            ForceUpdate(value);
            velocity = delta = default;
        }
    }

    [System.Serializable]
    public class SmoothFloat : Smooth<float>
    {
        public override bool isUp => target > _value;

        private bool IsTargetHigher => Mathf.Abs(target) > Mathf.Abs(_value);

        //----------------------------------------------------------------------------------------------------------

        public SmoothFloat(in float init = default) : base(init) { }

        //----------------------------------------------------------------------------------------------------------

        public override bool Update(float value)
        {
            delta = value - _value;
            return base.Update(value);
        }

        public override void ForceUpdate(float value)
        {
            delta = default;
            base.ForceUpdate(value);
        }

        public bool SmoothDamp(in float damp, in float deltaTime) => Update(Util_smooths.NO_SMOOTH ? target : Mathf.SmoothDamp(_value, target, ref velocity, damp, Mathf.Infinity, deltaTime));

        public bool SmoothDamp(in float up, in float down, in float deltaTime) => Update(Util_smooths.NO_SMOOTH ? target : Mathf.SmoothDamp(_value, target, ref velocity, IsTargetHigher ? up : down, Mathf.Infinity, deltaTime));

        public bool SmoothDamp(in float up, in float down, in float limit, in float deltaTime) => Update(Util_smooths.NO_SMOOTH ? target : Mathf.SmoothDamp(_value, target, ref velocity, IsTargetHigher ? up : down, limit, deltaTime));

        public bool SmoothDampAngle(in float damp, in float deltaTime, in float maxSpeed = Mathf.Infinity) => Update(Util_smooths.NO_SMOOTH ? target : Mathf.SmoothDampAngle(_value, target, ref velocity, damp, maxSpeed, deltaTime));
    }

    public abstract class SmoothVector<T> : Smooth<T> where T : struct
    {
        [Min(0)] public float sqr;

        //----------------------------------------------------------------------------------------------------------

        public SmoothVector(in T init = default) : base(init) { }
    }

    [System.Serializable]
    public class SmoothVector2 : SmoothVector<Vector2>
    {
        public override bool isUp => target.sqrMagnitude > sqr;

        //----------------------------------------------------------------------------------------------------------

        public SmoothVector2(in Vector2 init = default) : base(init) { sqr = init.sqrMagnitude; }

        //----------------------------------------------------------------------------------------------------------

        public override bool Update(Vector2 value)
        {
            delta = value - _value;
            return base.Update(value);
        }

        public override void ForceUpdate(Vector2 value)
        {
            delta = value - _value;
            base.ForceUpdate(value);
        }

        public bool SmoothDamp(in float damp, in float deltaTime, in float maxSpeed = Mathf.Infinity)
        {
            Vector2 val = Util_smooths.NO_SMOOTH ? target : Vector2.SmoothDamp(_value, target, ref velocity, damp, maxSpeed, deltaTime);
            sqr = val.sqrMagnitude;
            return Update(val);
        }

        public bool SmoothDamp(in float spring, in float damp, in float deltaTime, in float maxSpeed = Mathf.Infinity)
        {
            Vector2 val = Util_smooths.NO_SMOOTH ? target : Vector2.SmoothDamp(_value, target * spring - _value * (spring - 1), ref velocity, damp, maxSpeed, deltaTime);
            sqr = val.sqrMagnitude;
            return Update(val);
        }
    }

    [System.Serializable]
    public class SmoothVector3 : SmoothVector<Vector3>
    {
        public override bool isUp => target.sqrMagnitude > sqr;

        //----------------------------------------------------------------------------------------------------------

        public SmoothVector3(in Vector3 init = default) : base(init) { sqr = init.sqrMagnitude; }

        //----------------------------------------------------------------------------------------------------------

        public override bool Update(Vector3 value)
        {
            delta = value - this._value;
            return base.Update(value);
        }

        public override void ForceUpdate(Vector3 value)
        {
            delta = value - _value;
            base.ForceUpdate(value);
        }

        public bool SmoothDamp(in float damp, in float deltaTime, in float maxSpeed = Mathf.Infinity)
        {
            Vector3 val;
            if (damp < .01f)
            {
                val = target;
                velocity = Vector3.zero;
            }
            else
                val = Util_smooths.NO_SMOOTH ? target : Vector3.SmoothDamp(_value, target, ref velocity, damp, maxSpeed, deltaTime);
            sqr = val.sqrMagnitude;
            return Update(val);
        }

        public bool SmoothDamp(in float spring, in float damp, in float deltaTime, in float maxSpeed = Mathf.Infinity)
        {
            Vector3 val = Util_smooths.NO_SMOOTH ? target : Vector3.SmoothDamp(_value, target * spring - _value * (spring - 1), ref velocity, damp, maxSpeed, deltaTime);
            sqr = val.sqrMagnitude;
            return Update(val);
        }
    }
}