using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Creature.Action;
using UnityEngine.AI;

namespace Creature
{
    public interface ISubject
    {
        int Id { get; }
        Transform Transform { get; }
        NavMeshAgent NavMeshAgent { get; }

        bool IsActivate { get; }

        void SortingOrder(float order);
        void Flip(float x);

        void SetEventHandler(System.Action<IActor> eventHandler);
        System.Action<IActor> EventHandler { get; }
    }
}

