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
    public abstract class Character : MonoBehaviour, IActor
    {
        #region Inspector

        [SerializeField] private int id = 0;
        [SerializeField] private Transform rootTm = null;

        #endregion

        private IStatGeneric _iStatGeneric = null;
        

        public int Id
        {
            get { return id; }
        }

        public Animator Animator { get; private set; } = null;
        public SpriteRenderer SpriteRenderer { get; private set; } = null;

        public Transform Transform
        {
            get { return NavMeshAgent.transform; }
        }
        
        public NavMeshAgent NavMeshAgent { get; private set; } = null;

        public IStat IStat
        {
            get { return _iStatGeneric?.Stat; }
        }

        public Action.IActController IActCtr { get; protected set; } = null;
        
        public System.Action<IActor> EventHandler { get; private set; } = null;

        #region Temp Stat
        [SerializeField] [Range(1f, 100f)] [Tooltip("이동 속도.")]
        private float moveSpeed = 1f;

        [SerializeField] 
        [Range(0f, 100f)]
        private float maxHp = 1f;
        #endregion

        public bool IsActivate
        {
            get
            {
                if (!rootTm)
                    return false;

                return rootTm.gameObject.activeSelf;
            }
        }

        public abstract string AnimationKey<T>(Act<T> act) where T : Act<T>.Data;

        #region ICharacterGeneric

        public virtual void Initialize()
        {
            // EventHandler = null;

            NavMeshAgent = gameObject.GetComponentInChildren<NavMeshAgent>();
            if (NavMeshAgent != null)
            {
                NavMeshAgent.updateRotation = false;
                NavMeshAgent.updateUpAxis = false;
            }
            
            Animator = GetComponentInChildren<Animator>();
            SpriteRenderer = gameObject.GetComponentInChildren<SpriteRenderer>();

            _iStatGeneric = new Stat();
            _iStatGeneric?.Initialize(this);

            SetOriginStat();

            IActCtr = transform.AddOrGetComponent<ActController>();
            IActCtr?.Initialize(this);
        }

        public virtual void ChainUpdate()
        {
            if (!IsActivate)
                return;

            IActCtr?.ChainUpdate();
        }

        public virtual void ChainLateUpdate()
        {
            if (!IsActivate)
                return;

            IActCtr?.ChainLateUpdate();
        }
        
        public virtual void ChainFixedUpdate()
        {
            if (!IsActivate)
                return;

            IActCtr?.ChainFixedUpdate();
        }

        public virtual void Activate()
        {
            _iStatGeneric?.Activate();
            IActCtr?.Activate();

            Extensions.SetActive(rootTm, true);
        }

        public virtual void Deactivate()
        {
            _iStatGeneric?.Deactivate();
            IActCtr?.Deactivate();

            EventHandler = null;

            Extensions.SetActive(rootTm, false);
        }

        #endregion

        // public void EnableNavmeshAgent()
        // {
        //     NavMeshAgent = SkeletonAnimation?.AddOrGetComponent<NavMeshAgent>();
        //     if (NavMeshAgent != null)
        //     {
        //         NavMeshAgent.enabled = true;
        //
        //         NavMeshAgent.baseOffset = 0.5f;
        //         NavMeshAgent.speed = 3.5f;
        //         NavMeshAgent.angularSpeed = 200f;
        //         NavMeshAgent.acceleration = 100f;
        //         NavMeshAgent.radius = 0.5f;
        //         NavMeshAgent.height = 2f;
        //
        //         NavMeshAgent.updateRotation = false;
        //         NavMeshAgent.updateUpAxis = false;
        //
        //         NavMeshAgent.isStopped = false;
        //         NavMeshAgent.ResetPath();
        //     }
        // }

        public void DisableNavmeshAgent()
        {
            if (NavMeshAgent == null)
                return;

            NavMeshAgent.isStopped = true;
            NavMeshAgent.enabled = false;
        }
        
        #region ISubject

        public void SortingOrder(float order)
        {
            if (SpriteRenderer == null)
                return;
            
            // Debug.Log(Mathf.CeilToInt(-order * 100f));
            SpriteRenderer.sortingOrder = Mathf.CeilToInt(-order * 100f);
        }
        
        public void Flip(float x)
        {
            if (SpriteRenderer == null)
                return;

            SpriteRenderer.flipX = x > 0;
        }
        
        void ISubject.SetEventHandler(System.Action<IActor> eventHandler)
        {
            EventHandler += eventHandler;
        }

        #endregion

        #region Temp Stat

        private void SetOriginStat()
        {
            IStat?.SetOrigin(Stat.EType.MoveSpeed, moveSpeed);
            IStat?.SetOrigin(Stat.EType.Hp, maxHp);
            IStat?.SetOrigin(Stat.EType.MaxHp, maxHp);
        }

        #endregion
    }
}
