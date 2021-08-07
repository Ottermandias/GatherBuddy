using System;
using System.Collections;
using System.Collections.Generic;

namespace GatherBuddy.Utility
{
    public class FusedList<T> : IList<T>
    {
        private readonly IList<T> _list1;
        private readonly IList<T> _list2;

        public FusedList(IList<T> list1, IList<T> list2)
        {
            _list1 = list1;
            _list2 = list2;
        }

        public IEnumerator<T> GetEnumerator()
            => throw new NotImplementedException();

        IEnumerator IEnumerable.GetEnumerator()
            => GetEnumerator();

        public void Add(T item)
            => _list2.Add(item);

        public void Clear()
        {
            _list1.Clear();
            _list2.Clear();
        }

        public bool Contains(T item)
            => _list1.Contains(item) || _list2.Contains(item);

        public void CopyTo(T[] array, int arrayIndex)
        {
            _list1.CopyTo(array, arrayIndex);
            _list2.CopyTo(array, arrayIndex + _list1.Count);
        }

        public bool Remove(T item)
            => _list1.Remove(item) || _list2.Remove(item);

        public int Count
            => _list1.Count + _list2.Count;

        public bool IsReadOnly
            => false;

        public int IndexOf(T item)
        {
            var idx = _list1.IndexOf(item);
            if (idx >= 0)
                return idx;

            idx = _list2.IndexOf(item);
            return idx < 0 ? -1 : idx + _list1.Count;
        }

        public void Insert(int index, T item)
        {
            if (index < _list1.Count)
                _list1.Insert(index, item);
            else
                _list2.Insert(index - _list1.Count, item);
        }

        public void RemoveAt(int index)
        {
            if (index < _list1.Count)
                _list1.RemoveAt(index);
            else
                _list2.RemoveAt(index - _list1.Count);
        }

        public T this[int index]
        {
            get => index < _list1.Count ? _list1[index] : _list2[index - _list1.Count];
            set
            {
                if (index < _list1.Count)
                    _list1[index] = value;
                else
                    _list2[index - _list1.Count] = value;
            }
        }
    }
}
