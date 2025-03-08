using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Cysharp.Threading.Tasks;

using Creator;

namespace GameSystem
{
    public interface ICharacterManager : IManager
    { 
        UniTask<T> AddAsync<T>(int id, Transform rootTm) where T : Creature.Character;
    }
    
    public class CharacterManager : Manager, ICharacterManager
    {
        private List<Creature.Character> _characterList = null;

        public static Creature.Playable Playable { get; private set; } = null;
        
        public GameSystem.IGeneric Initialize()
        {
            CreatePlayableAsync().Forget();
            
            return this;
        }
        
        void GameSystem.IGeneric.ChainUpdate()
        {
            Playable?.ChainUpdate();
            
            for (int i = 0; i < _characterList?.Count; ++i)
            {
                var character = _characterList[i];
                if(character == null)
                    continue;
                
                if(!character.IsActivate)
                    continue;
                    
                character.ChainUpdate();
            }
        }

        void GameSystem.IGeneric.ChainLateUpdate()
        {
            for (int i = 0; i < _characterList?.Count; ++i)
            {
                var character = _characterList[i];
                if(character == null)
                    continue;
                
                if(!character.IsActivate)
                    continue;
                    
                character.ChainLateUpdate();
            }
        }

        private async UniTask CreatePlayableAsync()
        {
            Playable = await CharacterCreator<Creature.Playable>.Get
                .SetKey(nameof(Playable))
                .Create();
            Playable?.Initialize();
        }
        
        async UniTask<T> ICharacterManager.AddAsync<T>(int id, Transform rootTm)
        {
            if (_characterList == null)
            {
                _characterList = new();
                _characterList.Clear();
            }

            var character = _characterList?.Find(character => character.Id == id);
            if (character != null)
                return character as T;
            
            character = await CharacterCreator<T>.Get
                .SetKey(GetKey(id))
                .SetRoot(rootTm)
                .Create();
            
            if (character == null)
                return null;
            
            character.Initialize();
            
            _characterList?.Add(character);

            return character as T;
        }

        private string GetKey(int id)
        {
            switch (id)
            {
                case 1: return $"Animal/Barnie.prefab";
            }

            return string.Empty;
        }
    }
}