using System.Collections;
using System.Collections.Generic;
using Creator;
using UnityEngine;

using Cysharp.Threading.Tasks;

using GameSystem;
using Creature;

public class Game : MonoBehaviour
{
    private async UniTask Awake()
    {
        // MainManager.Instance
        Debug.Log("Game Awake");
        // await ResourceManager.Instance.InitializeAsync();

        // UIManager.Create();
    }
}