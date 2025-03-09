using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace UI
{
    public class Component : MonoBehaviour
    {
        public class Data
        {
            
        }
        
        [SerializeField]
        private Transform rootTm = null;
        
        public virtual void Initialize()
        {
            
        }
        
        public bool IsActivate 
        {
            get
            {
                if (!rootTm)
                    return false;
                
                return rootTm.gameObject.activeSelf;
            }
        }
        
        public virtual void Activate()
        {
            Extensions.SetActive(rootTm, true);
        }
        
        public virtual void Deactivate()
        {
            Extensions.SetActive(rootTm, false);
        }

        public virtual void ChainUpdate()
        {
            
        }
        
        public virtual void ChainLateUpdate()
        {
            
        }
    }
    
    public abstract class Component<T> : Component where T : Component.Data
    {
        protected T _data = null;

        public virtual void Initialize(T data)
        {
            base.Initialize();
            
            _data = data;
        }
        
        public virtual void Activate(T data)
        {
            base.Activate();
            
            _data = data;
        }
    }
}