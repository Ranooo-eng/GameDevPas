using UnityEngine;
// PELAJARAN 2 — Event (si penerima)
// Tempel ke GameObject LAIN (boleh Empty). Penerima tidak perlu kenal detail pemancar.

public class PenerimaEvent : MonoBehaviour
{

    void OnEnable()
    {
        PengirimEvent.OnTekanSpasi += Reaksi;
    }

    void OnDisable()
    {
        PengirimEvent.OnTekanSpasi -= Reaksi;
    }

    void Reaksi()
    {
        Debug.Log("Penerima: aku dengar event spasi!");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
