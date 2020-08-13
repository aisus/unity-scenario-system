using UnityEngine;

namespace TrainingSystem.Scripts.SceneInteraction
{
    /// <summary>
    /// Selector component to select interactive objects
    /// and perform actions
    /// </summary>
    public class InteractiveObjectsSelector : MonoBehaviour
    {
        /// <summary>
        /// Interactive object the user is currently looking at
        /// </summary>
        public InteractiveBehaviour CurrentSelectedObject { get; private set; }

        [SerializeField] [Range(0.1f, 10f)] private float     _maxSelectionDistance;
        [SerializeField]                    private LayerMask _interactionLayer;
        [SerializeField]                    private Material  _outlineMaterial;
        
        private void Update()
        {
            CheckSelection();
            CheckActionOnSelectedEntity();
        }
        
        private void CheckSelection()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, _maxSelectionDistance, _interactionLayer))
            {
                DeselectCurrentObject();
                return;
            }

            if (CurrentSelectedObject && hit.transform == CurrentSelectedObject.transform) return;

            var foundObject = hit.transform.GetComponent<InteractiveBehaviour>();
            if (foundObject)
            {
                SelectNewObject(foundObject);
            }
            else
            {
                DeselectCurrentObject();
            }
        }

        private void CheckActionOnSelectedEntity()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CurrentSelectedObject) CurrentSelectedObject.PerformAction();
            }
        }

        private void DeselectCurrentObject()
        {
            if (!CurrentSelectedObject) return;
            CurrentSelectedObject.Select(false);
            CurrentSelectedObject = null;
        }

        private void SelectNewObject(InteractiveBehaviour foundObject)
        {
            DeselectCurrentObject();
            CurrentSelectedObject = foundObject;
            CurrentSelectedObject.Select(true, _outlineMaterial);
        }
    }
}