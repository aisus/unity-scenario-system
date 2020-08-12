using UnityEngine;

namespace TrainingSystem.Scripts.SceneInteraction
{
    public class Selector : MonoBehaviour
    {
        public InteractiveBehaviour CurrentSelectedObject { get; private set; }

        [Header("Selector settings")] [SerializeField] [Range(0.1f, 10f)]
        private float _maxSelectionDistance;

        [SerializeField] private LayerMask _interactionLayer;
        
        [Header("Materials")] [SerializeField] private Material _outlineMaterial;

        #region EVENT_FUNCTIONS
        
        private void Update()
        {
            CheckSelection();
            CheckActionOnSelectedEntity();
        }

        #endregion

        private void CheckSelection()
        {
            var ray = new Ray(transform.position, transform.forward);
            if (!Physics.Raycast(ray, out var hit, _maxSelectionDistance, _interactionLayer))
            {
                if (!CurrentSelectedObject) return;
                CurrentSelectedObject.Select(false);
                CurrentSelectedObject = null;
                return;
            }

            var foundObject = hit.transform.GetComponent<InteractiveBehaviour>();
            if (foundObject)
            {
                if (foundObject == CurrentSelectedObject) return;
                if (CurrentSelectedObject) CurrentSelectedObject.Select(false);
                CurrentSelectedObject = foundObject;
                CurrentSelectedObject.Select(true, _outlineMaterial);
            }
            else
            {
                if (!CurrentSelectedObject) return;
                CurrentSelectedObject.Select(false, _outlineMaterial);
                CurrentSelectedObject = null;
            }
        }

        private void CheckActionOnSelectedEntity()
        {
            if (Input.GetKeyDown(KeyCode.Mouse0))
            {
                if (CurrentSelectedObject) CurrentSelectedObject.PerformAction();
            }
        }
    }
}