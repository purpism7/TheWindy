using System;
using System.Collections;
using System.Collections.Generic;
using Creator;
using UnityEngine;
using UnityEngine.AI;

using Creature.Action;
using UI.Part;

namespace Creature
{
    public class Playable : Character
    {
        [SerializeField] private VariableJoystick variableJoystick = null;
        
        // public NavMeshAgent NavMeshAgent { get; private set; } = null;

        private Vector3 _prevPos = Vector3.zero;
        private string _animationKey = string.Empty;

        // temp
        private PartSpeechBubble _partSpeechBubble = null;

        public override void Initialize()
        {
            base.Initialize();

            // _partSpeechBubble = new UICreator<PartSpeechBubble, PartSpeechBubble.Data>()
            //     .SetData(new PartSpeechBubble.Data
            //     {
            //         TargetTm = NavMeshAgent?.transform,
            //     })
            //     .SetWorldUI(true)
            //     .Create();
            
            GameSystem.Manager.Get<GameSystem.ICameraManager>()?.SetPlayable(this);
        }

        public override string AnimationKey<T>(Act<T> act)
        {
            // throw new System.NotImplementedException();
            return string.Empty;
        }

        public override void ChainUpdate()
        {
            base.ChainUpdate();
            
            if (!Transform)
                return;
            
            var horizontal = Input.GetAxis("Horizontal");
            var vertical = Input.GetAxis("Vertical");
#if UNITY_ANDROID || UNITY_IOS
            horizontal = variableJoystick.Horizontal;
            vertical = variableJoystick.Vertical;
#endif
            
            string animationKey = string.Empty;
            if (horizontal == 0 &&
                vertical == 0)
                animationKey = "Idle";
            else
                animationKey = "Walk";

            if (_animationKey != animationKey)
            {
                Animator?.SetTrigger(animationKey);
                _animationKey = animationKey;
            }
            
            Vector2 direction = _prevPos - Transform.position;
            // if (direction.x < 0)
            //     Transform.localScale = new Vector3(-1, 1, 1);
            // else if (direction.x > 0)
            //     Transform.localScale = Vector3.one;
            //
            Flip(-direction.x);
            _prevPos = Transform.position;
            
            var targetPos = Transform.position + new Vector3(horizontal, vertical, 0);
            targetPos.z = targetPos.y;
            Transform.position = Vector3.Lerp(Transform.position, targetPos, Time.deltaTime * NavMeshAgent.speed);
            
            SortingOrder(Transform.position.y);
        }

        private void LateUpdate()
        {
            _partSpeechBubble?.ChainLateUpdate();
        }
    }
}
    
