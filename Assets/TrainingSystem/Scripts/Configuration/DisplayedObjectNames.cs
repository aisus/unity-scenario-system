using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    /// <summary>
    /// Mapping of object string keys to displayed names
    /// </summary>
    [CreateAssetMenu(fileName = "New DisplayedObjectNames", menuName = "Displayed Object Names", order = 2)]
    public class DisplayedObjectNames : ScriptableObject
    {
        [Serializable]
        public class KeyNamePair
        {
            /// <summary>
            /// Object key for program logic
            /// </summary>
            public string Key => _key;

            /// <summary>
            /// Name to display
            /// </summary>
            public string DisplayedName => _displayedName;

            [SerializeField] private string _key;
            [SerializeField] private string _displayedName;
        }

        public KeyNamePair[] Data => _data;

        [SerializeField] private KeyNamePair[] _data;

        private Dictionary<string, string> _keyToName;
        private Dictionary<string, string> _nameToKey;

        public string GetNameByKey(string key) => (_keyToName ?? InitKeyToName())[key];

        public string GetKeyByName(string name) => (_nameToKey ?? InitNameToKey())[name];

        private Dictionary<string, string> InitKeyToName() =>
            _keyToName = _data.ToDictionary(x => x.Key, x => x.DisplayedName);

        private Dictionary<string, string> InitNameToKey() =>
            _nameToKey = _data.ToDictionary(x => x.DisplayedName, x => x.Key);
    }
}