using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

using GameSystem;

public class MainManager : Singleton<MainManager>
{
    private List<IGeneric> _iMgrGenericList = null;

    public IEnumerable<IGeneric> IMgrGenericList => _iMgrGenericList;
    
    protected override void Initialize()
    {
        if (_iMgrGenericList == null)
        {
            _iMgrGenericList = new();
            _iMgrGenericList.Clear();
        }

        _iMgrGenericList?.Add(GetComponent<CameraManager>()?.Initialize());
       
        _iMgrGenericList?.Add(transform.AddOrGetComponent<CharacterManager>()?.Initialize());
        _iMgrGenericList?.Add(transform.AddOrGetComponent<RegionManager>()?.Initialize());
    }

    private void Update()
    {
        for (int i = 0; i < _iMgrGenericList?.Count; ++i)
            _iMgrGenericList[i]?.ChainUpdate();
    }

    private void LateUpdate()
    {
        for (int i = 0; i < _iMgrGenericList?.Count; ++i)
            _iMgrGenericList[i]?.ChainLateUpdate();
    }
}
