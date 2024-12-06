﻿using UnityEngine;

namespace _UTIL_
{
    internal abstract class ParticleModule : MonoBehaviour
    {
        protected ParticleCollisionHandler handler;
        protected new ParticleSystem particleSystem;

        //--------------------------------------------------------------------------------------------------------------

        private void Awake()
        {
            particleSystem = GetComponent<ParticleSystem>();
        }

        //--------------------------------------------------------------------------------------------------------------

        void Start()
        {
            handler = GetComponentInParent<ParticleCollisionHandler>();
            handler.onParticleCollision += _OnParticleCollision;
            Debug.Log($"{this}({transform.GetPath(true)}).{nameof(handler)}({handler.GetType().FullName}) = {handler}", this);
        }

        //--------------------------------------------------------------------------------------------------------------

        protected abstract void _OnParticleCollision(GameObject other, ParticleCollisionEvent collision);
    }
}