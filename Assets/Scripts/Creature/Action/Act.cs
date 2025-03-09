using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

namespace Creature.Action
{
    public interface IAct
    {
        void Execute();
        void End();
        UniTaskVoid ChainUpdateAsync();
        void ChainLateUpdate();
        void ChainFixedUpdate();
    }
    
    public abstract class Act<T> : IAct where T : Act<T>.Data
    {
        public interface IListener
        {
            void End();
        }
        
        public class Data
        {
            public IListener IListener = null;
            
            public string AnimationKey { get; private set; } = string.Empty;
            
            public Data SetAnimationKey(string key)
            {
                AnimationKey = key;

                return this;
            }
        }
        
        protected T _data = null;
        protected IActor _iActor = null;
        protected float _duration = 0;

        private bool _update = false;
        private bool _end = false;
        private System.Action _endAction = null;
        
        public virtual void Initialize(IActor iActor)
        {
            _iActor = iActor;
        }
        
        public virtual void End()
        {
            if (_end)
                return;
            
            _endAction?.Invoke();
            _data?.IListener?.End();

            _update = false;
            _end = true;
        }

        public void SetData(T data)
        {
            _data = data;
        }

        public void SetEndActAction(System.Action endAction)
        {
            _endAction = endAction;
        }

        public async UniTaskVoid ChainUpdateAsync()
        {
            _update = true;
            
            await UniTask.Yield();
            await UniTask.WaitWhile(
                () =>
                {
                    ChainUpdate();

                    return _update && !_end;
                });
        }
        
        #region IAct

        public virtual void Execute()
        {
            _end = false;
        }
        
        protected virtual void ChainUpdate()
        {
            
        }

        public virtual void ChainLateUpdate()
        {
            
        }

        public virtual void ChainFixedUpdate()
        {
            
        }
        #endregion

        protected void SetAnimation(string animationName, bool loop)
        {
            if (_iActor == null)
                return;
            
            _iActor.Animator?.SetTrigger(animationName);
            // var animationState = _iActor.SkeletonAnimation?.AnimationState;
            // if (animationState == null)
            //     return;
            //
            // var animation = _iActor.SkeletonAnimation.skeletonDataAsset?.GetSkeletonData(true)?.Animations?
            //     .Find(animation => animation.Name.Contains(animationName));
            // if (animation == null)
            //     return;
            //
            // var trackEntry = animationState.SetAnimation(0, animationName, loop);
            // if (trackEntry == null)
            //     return;
            //
            // _duration = trackEntry.Animation.Duration;
        }
    }
}

