using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightPatch : MonoBehaviour
{
    [SerializeField] GameObject defaultPatch;
    [SerializeField] GameObject highlightedPatch;
    // Start is called before the first frame update
    
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void highlgihtPatch()
    {
        defaultPatch.SetActive(false);
        highlightedPatch.SetActive(true);
    }
}
