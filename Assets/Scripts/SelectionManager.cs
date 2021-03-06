using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionManager : MonoBehaviour
{
    [SerializeField] private string selectableTag = "Selectable";
    [SerializeField] private Material highlightMaterial;
    [SerializeField] private Material defaultMaterial;

    [SerializeField] AudioClip _TriggerSound = null;

    


    private Transform _selection;
    private void Update()
    {
        if (_selection != null)
        {
            var selectionRenderer = _selection.GetComponent<Renderer>();
            selectionRenderer.material = defaultMaterial;
            //triggerGraphic1.SetActive(true);
            _selection = null;
        }

        var ray = Camera.main.ScreenPointToRay (new Vector3(Screen.width / 2f, Screen.height / 2f, 0f));
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit))
        {
            var selection = hit.transform;
            if (selection.CompareTag(selectableTag))
            {
                var selectionRenderer = selection.GetComponent<Renderer>();
                if (selectionRenderer != null)
                {
                    AudioHelper.PlayClip2D(_TriggerSound, 1);
                    selectionRenderer.material = highlightMaterial;

                    
                    //triggerGraphic1.SetActive(true);
                }
                _selection = selection;
            }

        }
    }
}
