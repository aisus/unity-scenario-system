using System.Linq;
using TrainingSystem.Scripts.Configuration;
using TrainingSystem.Scripts.Infrastructure.Services.Preferences;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility.ObjectNames
{
    /// <inheritdoc cref="IObjectNamesService" />
    [DisallowMultipleComponent]
    public class ObjectNamesService : MonoBehaviour, IObjectNamesService
    {

        private DisplayedObjectNames _displayedObjectNames;
        
        /// <inheritdoc />
        public string GetNameByKey(string key) =>
            _displayedObjectNames.Data.FirstOrDefault(x => x.ObjectKey == key)?.DisplayedName;

        /// <inheritdoc />
        public string GetKeyByName(string name) =>
            _displayedObjectNames.Data.FirstOrDefault(x => x.DisplayedName == name)?.ObjectKey;

        /// <inheritdoc />
        public void OnSceneExit()
        {
        }
        
        private void Start()
        {
            _displayedObjectNames = GlobalPreferences.DisplayedObjectNames;
        }
    }
}