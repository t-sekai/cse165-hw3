using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using TMPro;
using Unity.XR.CoreUtils;

public class Wound : MonoBehaviour
{
    [SerializeField]
    private LayerMask selectableLayers;
    [SerializeField]

    HashSet<GameObject> patches = new HashSet<GameObject>();

    private void Update()
    {
        foreach (var patch in patches)
        {
            if (selectableLayers == (selectableLayers | (1 << patch.layer)))
            {
                if (Mathf.Abs(Vector3.Dot(patch.transform.up, transform.up)) > 0.8f)
                {
                    patch.tag = "Untagged";
                    patch.layer = LayerMask.NameToLayer("Default");
                    patch.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeAll;
                    patches.Remove(patch);

                    patch.transform.parent = transform;
                    // patch.GetComponent<MeshRenderer>().material.EnableKeyword("_EMISSION");
                    // patch.GetComponent<MeshRenderer>().material.SetColor("_EmissionColor", Color.green);

                    //patches[patch].transform.parent = transform;
                    //patches[patch].SetActive(true);
                    patch.GetComponent<HighlightPatch>().highlgihtPatch();

                }
            }
        }
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.tag == "Patch")
        {
            //GameObject successText = Instantiate(obj);
            //successText.SetActive(false);
            patches.Add(collider.gameObject);
            // Destroy(successText, 5);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (patches.Contains(other.gameObject))
            patches.Remove(other.gameObject);
    }
}
