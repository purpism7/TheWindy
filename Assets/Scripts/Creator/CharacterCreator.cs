using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using GameSystem;


namespace Creator
{
    public class CharacterCreator<T> : Creator<CharacterCreator<T>> where T : Creature.Character
    {
        private string _key = string.Empty;
        private Transform _rootTm = null;

        // public CharacterCreator<T> SetId(int id)
        // {
        //     _id = id;
        //     return this;
        // }

        public CharacterCreator<T> SetKey(string key)
        {
            _key = key;
            return this;
        }
        
        public CharacterCreator<T> SetRoot(Transform rootTm)
        {
            _rootTm = rootTm;
            return this;
        }
        
        public async UniTask<T> Create()
        {
            GameObject loadGameObj = await AddressableManager.Instance.LoadAssetByNameAsync<GameObject>(_key);
            var gameObj = GameObject.Instantiate(loadGameObj, _rootTm);
            if (!gameObj)
                return null;
            
            var t = gameObj.GetComponent<T>();
            Debug.Log(t);
                
                
            return t;
        }
    }
}