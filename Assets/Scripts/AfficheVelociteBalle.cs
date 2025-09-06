using UnityEngine;
using TMPro;
using System.Collections;

public class AfficheVelociteBalle : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textInfo;
    [SerializeField] Rigidbody rbBalle;
    void Start()
    {
        StartCoroutine(AfficheInfoBalle());
    }

    // Update is called once per frame
    IEnumerator AfficheInfoBalle()
    {
        while (true)
        {
            textInfo.text = rbBalle.linearVelocity.magnitude.ToString();
            yield return new WaitForSeconds(1f);
        }
        
    }
}
