using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.AI;
using Cysharp.Threading.Tasks;

using Creature.Action;
using GameSystem;


namespace Creature
{
    public class Animal : Character, Act<Move.Data>.IListener, Act<Interaction.Data>.IListener
    {
        [SerializeField] private bool autoMove = false;
        
        public override void Initialize()
        {
            base.Initialize();
        }

        public override void Activate()
        {
            base.Activate();

            if (autoMove)
                RandomMoveAsync().Forget();
        }

        public override void ChainUpdate()
        {
            base.ChainUpdate();

            UpdateInteraction();
        }
        
        public override string AnimationKey<T>(Act<T> act)
        {
            switch (act)
            {
                case Move:
                    return nameof(Move);
                case Interaction:
                    return nameof(Idle);
            }
            
            return act?.GetType().Name;
        }

        private async UniTask RandomMoveAsync()
        {
            var delaySec = UnityEngine.Random.Range(5f, 10f);
            await UniTask.Delay(TimeSpan.FromSeconds(delaySec));
            
            var currIAct = IActCtr?.CurrIAct;
            if (currIAct is Interaction)
                return;
            
            var range = 5f;
            float randomX = UnityEngine.Random.Range(-range, range);
            float randomY = UnityEngine.Random.Range(-range, range);
            var targetPos = new Vector3(randomX, randomY, 0);
            
            NavMeshHit hit;
            if (!NavMesh.SamplePosition(targetPos, out hit, 1f, NavMesh.AllAreas))
            {
                RandomMoveAsync().Forget();
                return;
            }
            
            targetPos = hit.position;
            targetPos.z = 0;
            
            IActCtr?.MoveToTarget(targetPos)?.Execute();
        }

        private void UpdateInteraction()
        {
            if (IActCtr?.CurrIAct is Interaction)
                return;
            
            var playable = CharacterManager.Playable;
            if (playable == null)
                return;
            
            var distance = Vector3.Distance(playable.Transform.position, Transform.position);
            if(distance <= 2f)
            {
                // Debug.Log("Interaction");
                IActCtr?.AddActAsync<Interaction, Interaction.Data>(
                    new Interaction.Data
                    {
                        TargetIActor = playable,
                    });
                IActCtr?.Execute();
            }
        }

        void Act<Move.Data>.IListener.End()
        {
            Debug.Log("Act<Move.Data>.IListener.End()");
            
            RandomMoveAsync().Forget();
        }
        
        void Act<Interaction.Data>.IListener.End()
        {
            Debug.Log("Act<Interaction.Data>.IListener.End()");
            
            RandomMoveAsync().Forget();
        }
    }
}

