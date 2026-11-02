using System;
using UnityEngine;

// PELAJARAN 1 — Delegate
// Tempel script ini ke Empty GameObject, Play, buka Console.
// Delegate = "kotak yang bisa menyimpan method" (bukan angka, bukan teks).
public class BelajarDelegate : MonoBehaviour
{
    // Kita buat tipe baru: method yang tidak mengembalikan nilai, dan tidak butuh parameter.
    delegate void AksiSederhana();

    void Start()
    {
        ContohAction1();
        ContohAction2();
        ContohAction3();
    }

    void ContohAction1()
    {
        AksiSederhana aksi = TulisHalo;
        aksi();
    }

    void ContohAction2()
    {
        AksiSederhana aksi = TulisHalo;
        aksi += TulisDunia;
        aksi();
    }

    void ContohAction3()
    {
        // Action sudah disediakan C#. Sama seperti delegate void ...() di atas.
        Action aksi = TulisHalo;
        aksi += TulisDunia;
        aksi();
    }

    void TulisHalo()
    {
        //Debug.Log("Halo");
    }

    void TulisDunia()
    {
        //Debug.Log("Dunia");
    }
}
