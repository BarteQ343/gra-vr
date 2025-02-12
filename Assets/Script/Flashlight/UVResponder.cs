using System;
using UnityEngine;

namespace Script.Flashlight
{
    [RequireComponent(typeof(MeshRenderer))]
    public class UVResponder : MonoBehaviour, ISphereCastTarget
    {
        private MeshRenderer _meshRenderer;

        private bool _isHit;
        private void Awake()
        {
            _meshRenderer = GetComponent<MeshRenderer>();
        }

        public void OnSphereCastHit()
        {
            if (!_isHit)
            {
                _meshRenderer.enabled = true;
                _isHit = true;
            }
        }

        public void OnSphereCastExit()
        {
            if (_isHit)
            {
                _meshRenderer.enabled = false;
                _isHit = false;
            }
        }
    }
}