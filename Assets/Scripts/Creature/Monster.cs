using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Creature.Action;

namespace Creature
{
    public class Monster : Character
    {
        
        public override string AnimationKey<T>(Act<T> act)
        {
            switch (act)
            {
                case Idle: return "00_Idle";
                case Move: return "02_Run";
            }

            return string.Empty;
        }
        
        public override void Initialize()
        {
            base.Initialize();

            IActCtr = transform.AddOrGetComponent<ActController>();
            IActCtr?.Initialize(this);
        }
    }
}
