using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

namespace Creature.Action
{
    public class Move : Act<Move.Data>
    {
        public class Data : Act<Move.Data>.Data
        {
            public Vector3 TargetPos = Vector3.zero;
        }

        public override void Initialize(IActor iActor)
        {
            base.Initialize(iActor);
        }

        public override void Execute()
        {
            base.Execute();
            
            if (_data == null)
                return;

            SetAnimation(_data.AnimationKey, true);

            var navMeshAgent = _iActor?.NavMeshAgent;
            if (navMeshAgent != null)
            {
                navMeshAgent.speed = _iActor.IStat.Get(Stat.EType.MoveSpeed);
                navMeshAgent.SetDestination(_data.TargetPos);
            }
        }

        protected override void ChainUpdate()
        {
            base.ChainUpdate();

            var navMeshAgent = _iActor?.NavMeshAgent;
            if (navMeshAgent == null)
                return;
            
            var iActorTm = _iActor.Transform;
            if (!iActorTm)
                return;
            
            Vector2 direction = _data.TargetPos - iActorTm.position;
            _iActor?.Flip(direction.x);
            _iActor?.SortingOrder(iActorTm.position.y);
            
            var remainDistance = navMeshAgent.remainingDistance;
            if (remainDistance <= navMeshAgent.stoppingDistance)
                End();
        }
    }
}
