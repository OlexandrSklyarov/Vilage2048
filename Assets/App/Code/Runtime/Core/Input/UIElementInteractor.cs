using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Assets.App.Code.Runtime.Core.Input
{
    public class UIElementInteractor : MonoBehaviour
    {
        public int InstanceID => gameObject.GetInstanceID();
        public IEnumerable<RaycastResult> ClickResults => _clickResults;

        [SerializeField] private GraphicRaycaster[] _raycasters;
        [SerializeField] private bool _isShowDebug;

        private PointerEventData _clickData;
        private List<RaycastResult> _clickResults = new();

        private void Awake()
        {
            _clickData = new PointerEventData(EventSystem.current);  
            _raycasters = GetComponentsInChildren<GraphicRaycaster>(true);      
        }        

        public bool IsSelectUIElement(Vector2 screenPosition)
        {
            _clickData.position = screenPosition;            
            _clickResults.Clear();

            foreach (var raycaster in _raycasters)
            {
                raycaster.Raycast(_clickData, _clickResults); 
            }

            if (_isShowDebug)            
                foreach (var item in _clickResults)
                    Util.DebugLog.Print($"Select ui element {item.gameObject.name}");            

            return _clickResults.Count > 0;
        }
    }
}