using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Creature.Action
{
    public class Idle : Act<Idle.Data>
    {
        public class Data : Act<Idle.Data>.Data
        {
            // public float DelaySec = 0;
        }
        
        public override void Execute()
        {
            base.Execute();
            
            if (_data == null)
                return;
            
            SetAnimation(_data.AnimationKey, true);

            // if (_data != null)
            //     _delaySec = _data.DelaySec;
            // // else
            // //     _delaySec = UnityEngine.Random.Range(5f, 10f);
            //
            // _currSec = 0;
        }
    }
}

