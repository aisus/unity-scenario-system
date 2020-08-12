using System;
using UnityEngine;

namespace TrainingSystem.Scripts.Configuration
{
    /// <summary>
    /// Mapping of object string keys to displayed names
    /// </summary>
    [CreateAssetMenu(fileName = "New DisplayedObjectNames", menuName = "Displayed Object Names", order = 1)]
    public class DisplayedObjectNames : ScriptableObject
    {
        [Serializable]
        public class Entry
        {
            /// <summary>
            /// Object key for program logic
            /// </summary>
            public string ObjectKey => _objectKey;

            /// <summary>
            /// Name to display
            /// </summary>
            public string DisplayedName => _displayedName;

            [SerializeField] private string _objectKey;
            [SerializeField] private string _displayedName;
        }

        public Entry[] Data => _data;

        [SerializeField] private Entry[] _data;
    }
}