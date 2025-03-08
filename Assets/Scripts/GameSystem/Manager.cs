using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public interface IGeneric
    {
        IGeneric Initialize();
        void ChainUpdate();
        void ChainLateUpdate();
    }
    
    public interface IManager : IGeneric
    {
        
    }
    
    public abstract class Manager : MonoBehaviour
    {
        public bool IsActivate { get; private set; } = false;
    
        public virtual void Activate()
        {
            IsActivate = true;
        }
        
        public virtual void Deactivate()
        {
            IsActivate = false;
        }
        
        public static T Get<T>() where T : IManager
        {
            var iMgrGenericList = MainManager.Instance?.IMgrGenericList;
            if (iMgrGenericList == null)
                return default;

            // var findIMgrGeneric = iMgrGenericList.Find(iMgrGeneric => iMgrGeneric is T);
            // if (findIMgrGeneric == null)
            // {
            //     // findIMgrGeneric = 
            // }
        
            foreach (var iMgrGeneric in iMgrGenericList)
            {
                if (iMgrGeneric is T)
                    return (T)iMgrGeneric;
            }
        
            return default;
        }
    }
}
