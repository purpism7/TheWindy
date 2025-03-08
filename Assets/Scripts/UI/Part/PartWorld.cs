using System.Collections;
using System.Collections.Generic;
using GameSystem;
using UnityEngine;

namespace UI.Part
{
    public class PartWorld<T> : Component<T> where T : PartWorld<T>.Data
    {
        public class Data : Component.Data
        {
            public Transform TargetTm = null;
            public Vector2 Offset = Vector2.zero;
        }
        
        // protected T _data = null;
        private RectTransform _rectTm = null;
        
        public override void Initialize(T data)
        {
            base.Initialize(data);
 
            _data = data;

            _rectTm = GetComponent<RectTransform>();
        }

        public override void ChainLateUpdate()
        {
            base.ChainLateUpdate();
            
            if (!_rectTm)
                return;

            if (!_data?.TargetTm)
                return;

            var pos = GetScreenPos(_data.TargetTm.position);
            if(pos != null)
                _rectTm.anchoredPosition = pos.Value;
        }
        
        protected Vector3? GetScreenPos(Vector3 targetPos)
        {
            var camera = Manager.Get<ICameraManager>()?.MainCamera;
            if (camera == null)
                return null;
            
            var worldUIRootRectTm = UIManager.Instance?.WorldUIRootRectTm;
            if (!worldUIRootRectTm)
                return null;
            
            var uiCamera = UIManager.Instance?.UICamera;
            if (uiCamera == null)
                return null;
            
            var screenPos = camera.WorldToScreenPoint(targetPos);

            Vector2 localPos = Vector2.zero;
            RectTransformUtility.ScreenPointToLocalPointInRectangle(worldUIRootRectTm, screenPos, uiCamera, out localPos);
            localPos.x += _data.Offset.x;
            localPos.y += _data.Offset.y;
            
            return localPos;
        } 
    }
}

