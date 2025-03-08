using System.Collections;
using System.Collections.Generic;
using Creator;
using UI.Part;
using UnityEngine;

namespace Creature.Action
{
    public class Interaction : Act<Interaction.Data>
    {
        public class Data : Act<Interaction.Data>.Data
        {
            public IActor TargetIActor = null;
        }

        private PartSpeechBubble _partSpeechBubble = null;
        
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
                navMeshAgent.SetDestination(_iActor.Transform.position);
            }

            if (_partSpeechBubble == null)
            {
                _partSpeechBubble = UICreator<PartSpeechBubble, PartSpeechBubble.Data>.Get?
                    .SetData(new PartSpeechBubble.Data
                    {
                        TargetTm = _iActor?.Transform,
                        Offset = new Vector2(0, 220f),
                    })
                    .SetWorldUI(true)
                    .Create();
            }
            
            _partSpeechBubble?.Activate();
        }
        
        protected override void ChainUpdate()
        {
            base.ChainUpdate();

            var targetIActor = _data?.TargetIActor;
            if (targetIActor == null)
            {
                End();
                return;
            }
            
            var distance = Vector3.Distance(targetIActor.Transform.position, _iActor.Transform.position);
            if (distance > 2f)
            {
                End();
                return;
            }
        }
        
        public override void ChainLateUpdate()
        {
            base.ChainLateUpdate();
            
            _partSpeechBubble?.ChainLateUpdate();
        }

        public override void End()
        {
            base.End();
            
            Debug.Log("End");
            _partSpeechBubble?.Deactivate();
        }
    }
}
