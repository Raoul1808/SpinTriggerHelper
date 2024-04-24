using System.Collections.Generic;

namespace SpinTriggerHelper
{
    internal class ModTriggerStore
    {
        private readonly List<ITrigger> _triggers = new List<ITrigger>();
        private int _currentOrNextIndex = 0;
        private ITrigger _previousTrigger;
        private ITrigger _currentTrigger;
        private ITrigger _nextTrigger;

        public event TriggerManager.TriggerUpdate OnTriggerUpdate;

        public void AddTriggers(IEnumerable<ITrigger> trigger)
        {
            _triggers.AddRange(trigger);
            _triggers.Sort((t1, t2) => t1.Time.CompareTo(t2.Time));
            if (_currentOrNextIndex == 0 && _currentTrigger == null && _triggers.Count > 0)
            {
                _currentTrigger = _triggers[0];
                if (_triggers.Count > 1)
                {
                    _nextTrigger = _triggers[1];
                }
            }
        }

        public void Clear()
        {
            _triggers.Clear();
            Reset();
        }

        public void Reset()
        {
            _currentOrNextIndex = 0;
            _previousTrigger = null;
            _currentTrigger = null;
            _nextTrigger = null;
            if (_triggers.Count > 0)
            {
                _currentTrigger = _triggers[0];
                if (_triggers.Count > 1)
                {
                    _nextTrigger = _triggers[1];
                }
            }
        }
        
        public void Update(float trackTime)
        {
            if (trackTime >= _nextTrigger?.Time)
            {
                OnTriggerUpdate?.Invoke(_currentTrigger, _nextTrigger.Time);
                _currentTrigger = _nextTrigger;
                _currentOrNextIndex++;
                if (_triggers.Count > _currentOrNextIndex)
                {
                    _nextTrigger = _triggers[_currentOrNextIndex];
                }
            }
            if (trackTime >= _currentTrigger?.Time)
            {
                OnTriggerUpdate?.Invoke(_currentTrigger, trackTime);
            }
        }
    }
}
