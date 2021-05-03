using System;
using System.Collections;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace Hsr.CloudSolutions.SmartKitchen.UI.Templates
{
    public abstract class DataTemplateSelectorCollection 
        : DataTemplateSelector
        , IList<DataTemplate>
        , IList
    {
        private readonly List<DataTemplate> _dataTemplates = new List<DataTemplate>();

        public IEnumerable<DataTemplate> DataTemplates => _dataTemplates;


        public IEnumerator<DataTemplate> GetEnumerator()
        {
            return _dataTemplates.GetEnumerator();
        }

        public void Add(DataTemplate item)
        {
            InternalAdd(item);
        }

        private int InternalAdd(DataTemplate item)
        {
            if (Contains(item))
            {
                return -1;
            }
            _dataTemplates.Add(item);
            return _dataTemplates.Count - 1;
        }
        
        public void Clear()
        {
            _dataTemplates.Clear();
        }

        public bool Contains(DataTemplate item)
        {
            return _dataTemplates.Contains(item);
        }

        public void CopyTo(DataTemplate[] array, int arrayIndex)
        {
            _dataTemplates.CopyTo(array, arrayIndex);
        }

        public bool Remove(DataTemplate item)
        {
            return _dataTemplates.Remove(item);
        }

        public int Count => _dataTemplates.Count;

        public bool IsReadOnly => ((IList)_dataTemplates).IsReadOnly;

        public object SyncRoot => ((ICollection)_dataTemplates).SyncRoot;
        public bool IsSynchronized => ((ICollection) _dataTemplates).IsSynchronized;
        
        public int IndexOf(DataTemplate item)
        {
            return _dataTemplates.IndexOf(item);
        }

        public void Insert(int index, DataTemplate item)
        {
            _dataTemplates.Insert(index, item);
        }

        public void RemoveAt(int index)
        {
            _dataTemplates.RemoveAt(index);
        }

        public DataTemplate this[int index]
        {
            get => _dataTemplates[index];
            set => _dataTemplates[index] = value;
        }

        private DataTemplate Cast(object value)
        {
            var template = value as DataTemplate;
            if (template == null)
            {
                throw new ArgumentException($"DataTemplateSelectorCollection can only work with DataTemplates but it was tried to add a '{value?.GetType().FullName}' to the collection.");
            }
            return template;
        }

        #region IList

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public void CopyTo(Array array, int index)
        {
            Array.Copy(_dataTemplates.ToArray(), 0, array, index, _dataTemplates.Count);
        }

        int IList.Add(object value)
        {
            return InternalAdd(Cast(value));
        }

        bool IList.Contains(object value)
        {
            return Contains(Cast(value));
        }

        int IList.IndexOf(object value)
        {
            return IndexOf(Cast(value));
        }

        void IList.Insert(int index, object value)
        {
            Insert(index, Cast(value));
        }

        void IList.Remove(object value)
        {
            Remove(Cast(value));
        }

        bool IList.IsFixedSize => false;

        object IList.this[int index]
        {
            get => this[index];
            set => this[index] = Cast(value);
        }

        #endregion
    }
}
