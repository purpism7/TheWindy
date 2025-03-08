using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using GameSystem;
using Creature;
using Cysharp.Threading.Tasks;

public class Region : MonoBehaviour
{
    [SerializeField] private int id = 0;
    [SerializeField] private Transform rootTm = null;

    public void Initialize()
    {
        CreateAnimalAsync().Forget();
    }

    public void Activate()
    {
        
    }

    public void ChainUpdate()
    {
        
    }

    private async UniTask CreateAnimalAsync()
    {
        var iCharacterMgr = Manager.Get<ICharacterManager>();
        if (iCharacterMgr == null)
            return;
        
        var animal = await iCharacterMgr.AddAsync<Animal>(1, rootTm);
        animal?.Activate();
    }
}
