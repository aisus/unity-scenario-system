using System.Linq;
using TrainingSystem.Scripts.Configuration;
using UnityEngine;

namespace TrainingSystem.Scripts.Infrastructure.Services.Utility
{
    /// <inheritdoc cref="IObjectNamesService" />
    public class ObjectNamesService : MonoBehaviour, IObjectNamesService
    {
        [SerializeField] private DisplayedObjectNames _displayedObjectNames;

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
    }
}