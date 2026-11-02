using System;
using UnityEngine.InputSystem;
using UnityEngine;

// PELAJARAN 2 — Event (si pemancar)
// Tempel ke Empty GameObject. Event = delegate yang lebih aman:
// kelas lain boleh langganan (+=), tapi HANYA pemancar yang boleh Invoke.

public class PengirimEvent : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
     public static event Action OnTekanSpasi;

    void Update()
    {
        if (Keyboard.current != null && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("Pemancar: spasi ditekan, kirim event.");
            OnTekanSpasi?.Invoke();
        }
    }

}
