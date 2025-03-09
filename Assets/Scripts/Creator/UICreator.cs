using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using GameSystem;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;
using Vector3 = UnityEngine.Vector3;

namespace Creator
{
    public class UICreator<T, V> : Creator<UICreator<T, V>> where T : UI.Component where V : UI.Component.Data
    {
        private V _data = null;
        private bool _worldUI = false;
  
        public UICreator<T, V> SetData(V data = null) 
        {
            _data = data;
            
            return this;
        }
        
        public UICreator<T, V> SetWorldUI(bool worldUI)
        {
            _worldUI = worldUI;
            
            return this;
        }
        
        public T Create()
        {
            var component = UIManager.Instance?.Get<T>(worldUI: _worldUI) as UI.Component<V>;
            
            var rectTm = component?.GetComponent<RectTransform>();
            if (rectTm)
            {
                rectTm.anchoredPosition3D = Vector3.zero;
                rectTm.sizeDelta = Vector2.zero;
                rectTm.transform.localScale = Vector3.one;   
            }
            
            component?.Initialize(_data);
            // if (_component == null)
            //     return null;
            
            
            return component as T;
        }
    }
}