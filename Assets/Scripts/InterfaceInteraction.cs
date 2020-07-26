using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InterfaceInteraction : MonoBehaviour
{
    [SerializeField]
    private GameObject _swarmAI;
    [SerializeField]
    private GameObject _helpText;

    public void SpawnCopters()
    {
        GameObject.Instantiate(_swarmAI);
    }

    public void SetActiveHelpText()
    {
        if (_helpText.active)
        {
            _helpText.SetActive(false);
        }
        else
        {
            _helpText.SetActive(true);
        }
    }
}
