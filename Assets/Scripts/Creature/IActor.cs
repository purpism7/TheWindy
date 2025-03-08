using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Creature.Action;

namespace Creature
{
    public interface IActor : ISubject
    {
        Animator Animator { get; }
        
        IStat IStat { get; }
        IActController IActCtr { get; }

        string AnimationKey<T>(Act<T> act) where T : Act<T>.Data;
    }
}
