using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameSystem
{
    public interface IRegionManager : IManager
    {
        
    }
    
    public class RegionManager : Manager, IRegionManager
    {
        [SerializeField] private Region currRegion = null;

        private List<Region> _regionList = null;
        
        public GameSystem.IGeneric Initialize()
        {
            currRegion?.Initialize();
            
            return this;
        }
        
        void GameSystem.IGeneric.ChainUpdate()
        {
            currRegion?.ChainUpdate();
        }

        void GameSystem.IGeneric.ChainLateUpdate()
        {
            
        }
    }
}


