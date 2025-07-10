using System.Collections.Generic;
using UnityEngine;

namespace ApeCharacter
{
    public class CharacterKernel : MonoBehaviour
    {
        private readonly List<IInitializable> _initializables = new();
        private readonly List<IUpdatable> _updatables = new();
        private readonly List<IFixedUpdatable> _fixedUpdatables = new();
        private readonly List<ILateUpdatable> _lateUpdatables = new();

        private readonly List<IUpdatable> _updatablesBuffer = new();
        private readonly List<IFixedUpdatable> _fixedUpdatablesBuffer = new();
        private readonly List<ILateUpdatable> _lateUpdatablesBuffer = new();

        public void AddInitializable(IInitializable obj)
        {
            if (InitializeEnded)
                obj.Initialize();

            _initializables.Add(obj);
        }

        public void AddUpdatable(IUpdatable obj) => _updatablesBuffer.Add(obj);
        public void AddFixedUpdatable(IFixedUpdatable obj) => _fixedUpdatablesBuffer.Add(obj);
        public void AddLateUpdatable(ILateUpdatable obj) => _lateUpdatablesBuffer.Add(obj);

        private bool InitializeEnded { get; set; }

        private void FlushBuffers()
        {
            if (_updatablesBuffer.Count > 0)
            {
                _updatables.AddRange(_updatablesBuffer);
                _updatablesBuffer.Clear();
            }

            if (_fixedUpdatablesBuffer.Count > 0)
            {
                _fixedUpdatables.AddRange(_fixedUpdatablesBuffer);
                _fixedUpdatablesBuffer.Clear();
            }

            if (_lateUpdatablesBuffer.Count > 0)
            {
                _lateUpdatables.AddRange(_lateUpdatablesBuffer);
                _lateUpdatablesBuffer.Clear();
            }
        }

        private void Start()
        {
            foreach (var obj in _initializables)
                obj.Initialize();

            InitializeEnded = true;
        }

        private void Update()
        {
            FlushBuffers();
            float dt = Time.deltaTime;
            foreach (var obj in _updatables)
                obj.OnUpdate(dt);
        }

        private void FixedUpdate()
        {
            FlushBuffers();
            float dt = Time.fixedDeltaTime;
            foreach (var obj in _fixedUpdatables)
                obj.OnFixedUpdate(dt);
        }

        private void LateUpdate()
        {
            FlushBuffers();
            float dt = Time.deltaTime;
            foreach (var obj in _lateUpdatables)
                obj.OnLateUpdate(dt);
        }
    }
}