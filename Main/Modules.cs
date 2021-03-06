﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Linq;
using System.Text;
using ET.Interface;

namespace ET.Main
{

    //模块载入器类
    public class Modules : IDictionary<String, ICommModule>
    {

        [ImportMany("ETModule", AllowRecomposition = true)]
        private IEnumerable<Lazy<ICommModule, Dictionary<string, object>>> _modules;
        private Dictionary<String, ICommModule> _ms= new Dictionary<String, ICommModule>();


        public void InitialModules()
        {
            var catalog = new DirectoryCatalog("Modules");
            var container = new CompositionContainer(catalog);
            var objectToSatisfy = this;
            container.ComposeParts(this);

            var _msatt = new List<ModuleHeaderAttribute>();
            foreach (var m in _modules)
            {
                var mt = new ModuleHeaderAttribute { ModuleKey = m.Metadata["ModuleKey"].ToString(), ModuleShowName = m.Metadata["ModuleShowName"].ToString(), ILevel = (Int32)m.Metadata["ILevel"], IsOnlyOneFile = (bool)m.Metadata["IsOnlyOneFile"] };
                mt.ModuleIcon = m.Value.ModuleIcon;
                mt.FileIcon = m.Value.FileIcon;
                _msatt.Add(mt);

                _ms.Add(m.Metadata["ModuleKey"].ToString(), m.Value);
            }
            ModulesHeaders = _msatt;
        }

         public List<ModuleHeaderAttribute> ModulesHeaders { get; private set; }


        #region --IDictionary--

        public ICommModule this[string key] { get => _ms[key]; }
        ICommModule IDictionary<string, ICommModule>.this[string key] { get => _ms[key]; set => throw new NotImplementedException(); }

        public ICollection<string> Keys => _ms.Keys;

        public ICollection<ICommModule> Values => _ms.Values;

        public int Count => _ms.Count;

        public bool IsReadOnly => true;


        public void Add(string key, ICommModule value)
        {
            throw new NotImplementedException();
        }

        public void Add(KeyValuePair<string, ICommModule> item)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }

        public bool Contains(KeyValuePair<string, ICommModule> item)
        {
            throw new NotImplementedException();
        }

        public bool ContainsKey(string key)
        {
            throw new NotImplementedException();
        }

        public void CopyTo(KeyValuePair<string, ICommModule>[] array, int arrayIndex)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<KeyValuePair<string, ICommModule>> GetEnumerator()
        {
            return _ms.GetEnumerator();
        }


        public bool Remove(string key)
        {
            throw new NotImplementedException();
        }

        public bool Remove(KeyValuePair<string, ICommModule> item)
        {
            throw new NotImplementedException();
        }

        public bool TryGetValue(string key, out ICommModule value)
        {
            return _ms.TryGetValue(key,out value);
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _ms.GetEnumerator();
        }
        #endregion
    }
}
