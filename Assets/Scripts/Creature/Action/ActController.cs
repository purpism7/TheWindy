using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;
using UnityEngine.AI;

using Cysharp.Threading.Tasks;
using UnityEditor.Rendering;
using UnityEngine.UI;


namespace Creature.Action
{
    public interface IActController : IController<IActController, IActor>
    {
        // void Idle();
        IActController MoveToTarget(Vector3? pos = null);
        void Execute(); 
        UniTask AddActAsync<T, V>(V data = null) where T : Act<V>, new() where V : Act<V>.Data, new();

        IAct CurrIAct { get; }
        bool InAction { get; }
    }
    
    public class ActController : Controller, IActController
    {
        private IActor _iActor = null;
        private Dictionary<System.Type, IAct> _iActDic = null;
       
        private Queue<IAct> _iActQueue = null;
        private Vector3 _originPos = Vector3.zero;

        public IAct CurrIAct { get; private set; } = null;
        public bool InAction { get; private set; } = false;

        IActController IController<IActController, IActor>.Initialize(IActor iActor)
        {
            _iActor = iActor;

            _iActDic = new();
            _iActDic.Clear();
            
            return this;
        }

        #region IController

        void IController<IActController, IActor>.ChainUpdate()
        {
            if (!IsActivate)
                return;
        }

        void IController<IActController, IActor>.ChainLateUpdate()
        {
            if (!IsActivate)
                return;
            
            CurrIAct?.ChainLateUpdate();
        }
        
        void IController<IActController, IActor>.ChainFixedUpdate()
        {
            if (!IsActivate)
                return;
            
            CurrIAct?.ChainFixedUpdate();
        }

        public override void Activate()
        {
            base.Activate();
            
            if (transform.parent)
            {
                _originPos = transform.parent.position;
                _originPos.z = 0;
            }
        }

        public override void Deactivate()
        {
            base.Deactivate();
            
            _iActQueue?.Clear();
            Idle();
        }
        #endregion
            
        #region IActController
        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="pos"></param>
        /// <param name="finishAction"></param>
        /// <param name="reverse">Target Pos 에 도착 후, 반대 방향으로 Flip 할지.</param>
        /// <returns></returns>
        IActController IActController.MoveToTarget(Vector3? pos)
        {
            if (!IsActivate)
                return null;

            var targetPos = _originPos;
            if (pos != null)
                targetPos = pos.Value;
            
            var data = new Move.Data
            {
                TargetPos = targetPos,
            };

            AddActAsync<Move, Move.Data>(data).Forget();

            return this;
        }

        // void IActController.Interaction()
        // {
        //     Idle();
        //     
        //     AddActAsync<Interaction, Interaction.Data>().Forget();
        // }

        private async UniTask ExecuteAsync()
        {
            if (InAction)
                await UniTask.Yield(PlayerLoopTiming.LastPostLateUpdate);
            
            if (_iActQueue?.Count > 0)
            {
                if (_iActQueue.TryDequeue(out IAct iAct))
                {
                    CurrIAct?.End();
                    // await UniTask.Yield();
                        
                    InAction = true;
                    
                    iAct?.Execute();
                    SetCurrIAct(iAct);
                    
                    // if(iAct is Move)
                    iAct?.ChainUpdateAsync().Forget();
                    
                    return;
                }
            }

            Idle();
        }

        public void Execute()
        {
            ExecuteAsync().Forget();
        }

        private void Idle()
        {
            Execute<Idle, Idle.Data>();
            // SetCurrIAct(null);
            
            InAction = false;
        }

        public async UniTask AddActAsync<T, V>(V data = null) where T : Act<V>, new() where V : Act<V>.Data, new()
        {
            var act = GetAct<T, V>();
            if (act == null)
                return;
            
            data = GetData<V>(act, data);
            
            if (_iActQueue == null)
            {
                _iActQueue = new();
                _iActQueue.Clear();
            }
            
            _iActQueue?.Enqueue(act);
        }

        private Act<V> GetAct<T, V>() where T : Act<V>, new() where V : Act<V>.Data, new()
        {
            if (_iActDic == null)
            {
                _iActDic = new();
                _iActDic.Clear();
            }
            
            System.Type type = typeof(T);
            Act<V> act = null;
            
            if (_iActDic.TryGetValue(type, out IAct iAct))
            {
                act = iAct as Act<V>;
            }
            else
            {
                act = new T();
                act.Initialize(_iActor);
                act.SetEndActAction(EndAct);
                
                _iActDic?.TryAdd(type, act);
            }
            
            return act;
        }
        #endregion

        private V GetData<V>(Act<V> act, V data) where V : Act<V>.Data, new()
        {
            if (data == null)
                data = new V();
            
            act.SetData(data);
            data.IListener = _iActor as Act<V>.IListener;
            
            var animationKey = _iActor?.AnimationKey(act);
            Debug.Log(animationKey);
            data.SetAnimationKey(animationKey);

            return data;
        }

        private void Execute<T, V>(V data = null) where T : Act<V>, new() where V : Act<V>.Data, new()
        {
            var act = GetAct<T, V>();
            if (act == null)
                return;

            data = GetData<V>(act, data);
            act.Execute();
            SetCurrIAct(act);
            
            act.ChainUpdateAsync().Forget();
        }
        
        private void EndAct()
        {
            Execute();
        }

        private void SetCurrIAct(IAct iAct)
        {
            CurrIAct = iAct;
            
            // Debug.Log(name + " = " + iAct?.GetType());
        }
    }
}

