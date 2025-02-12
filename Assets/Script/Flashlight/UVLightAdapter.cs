using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Script.Flashlight
{
    [RequireComponent(typeof(Light))]
    public class UVLightAdapter : MonoBehaviour
    {
        private Light _light;
        private LayerMask mask;
        
        private float _innerRadius;
        private float _outerRadius;
        private float _radius;
        private float _range;
        
        private HashSet<ISphereCastTarget> _currentHits = new();
        private HashSet<ISphereCastTarget> _previousHits = new();
        
        private void Awake()
        {
            _light = GetComponent<Light>();
            _outerRadius = Mathf.Tan(_light.spotAngle * 0.5f * Mathf.Deg2Rad) * _light.range;
            _innerRadius = Mathf.Tan(_light.innerSpotAngle * 0.5f * Mathf.Deg2Rad) * _light.range;
            
            Debug.Log("Outer radius: " + _outerRadius + " Inner radius: " + _innerRadius);
            
            _range = _light.range;
            mask = LayerMask.GetMask("UV"); 
        }

        private void OnDrawGizmos()
        {
            if (_light == null) return;

            Gizmos.color = Color.yellow;
            Gizmos.DrawWireSphere(_light.transform.position, _radius);
            
            Vector3 endPoint = _light.transform.position + _light.transform.forward * _range;
            Gizmos.DrawWireSphere(endPoint, _radius);

            
            Gizmos.color = Color.cyan;
            Gizmos.DrawLine(_light.transform.position, endPoint);
        }

        private void OnDisable()
        {
            foreach (var target in _previousHits)
            {
                target.OnSphereCastExit();
            }
        }

        private void FixedUpdate()
        {
            if (_radius >= _outerRadius)
            {
                _radius = _innerRadius;
            }
            
            _radius = Mathf.Lerp(_innerRadius, _outerRadius, Mathf.PingPong(Time.time, 1f));
            
            Vector3 origin = _light.transform.position + _light.transform.forward * _radius;
            
            _currentHits.Clear();
            
            if (Physics.SphereCast(origin, _radius, _light.transform.forward, out var hit, _range, mask))
            {
                ISphereCastTarget target = hit.collider.GetComponent<ISphereCastTarget>();
                if (target != null)
                {
                    _currentHits.Add(target);
                    target.OnSphereCastHit();
                }
            }
            
            foreach (var target in _previousHits)
            {
                if (!_currentHits.Contains(target))
                {
                    target.OnSphereCastExit();
                }
            }
            
            (_previousHits, _currentHits) = (_currentHits, _previousHits);
        }

    }
}