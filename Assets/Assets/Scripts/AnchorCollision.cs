using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class AnchorCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Anchor")
        {
            if (this.GetComponent<ARAnchor>() == null)
            {
                this.AddComponent<ARAnchor>();
            }
            var anchor = this.GetComponent<ARAnchor>();

        }
    }
}
