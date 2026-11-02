using UnityEngine;
using System;

// INHERITANCE + ABSTRACTION
// Enemy menjadi class induk untuk semua jenis zombie.
// Enemy juga mengimplementasikan IDamageable agar Enemy bisa menerima damage.
public class Enemy : MonoBehaviour, IDamageable
{
    [Header("Enemy Stats")]
    [SerializeField] private int hp = 100;
    [SerializeField] private int damage = 20;

    public float ms = 2f;

    protected Transform player;

    [Header("State Machine")]
    [SerializeField] private float JarakDeteksi = 6f;
    [SerializeField] private float JarakSerang = 1.5f;
    [SerializeField] private float JedaSerang = 1f;

    // State sekarang
    private StateZombie state = StateZombie.IDLE;

    // Waktu terakhir enemy menyerang
    private float waktuSerangTerakhir;



    // DITAMBAHKAN
    // Mencegah Mati() dijalankan berkali-kali.
    private bool sudahMati = false;


    // Delegate + Event
    // Event ini akan dipancarkan ketika zombie mati.
    public static event Action<Enemy> OnZombieMati;


    protected virtual void Start()
    {
        GameObject playerObj =
            GameObject.FindGameObjectWithTag("Player");

        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }



    void Update()
    {
        // DITAMBAHKAN
        // Kalau zombie sudah mati, jangan jalankan state lagi.
        if (sudahMati)
            return;

        PeriksaTransisi();

        switch (state)
        {
            case StateZombie.IDLE:
                PerilakuIdle();
                break;

            case StateZombie.PATROL:
                PerilakuPatrol();
                break;

            case StateZombie.CHASE:
                PerilakuChase();
                break;

            case StateZombie.ATTACK:
                PerilakuAttack();
                break;
        }
    }



    public float JarakKePlayer()
    {
        if (player == null)
            return Mathf.Infinity;

        return Vector2.Distance(
            transform.position,
            player.position
        );
    }


    void PeriksaTransisi()
    {
        float jarak = JarakKePlayer();

        if (jarak <= JarakSerang)
        {
            state = StateZombie.ATTACK;
        }
        else if (jarak <= JarakDeteksi)
        {
            state = StateZombie.CHASE;
        }
        else
        {
            state = StateZombie.PATROL;
        }
    }




    public void Kejar()
    {
        if (player == null)
            return;

        transform.position = Vector2.MoveTowards(
            transform.position,
            player.position,
            ms * Time.deltaTime
        );
    }


    // virtual memungkinkan class turunan melakukan override.
    public virtual void Serang()
    {
        //Debug.Log("Enemy menyerang Player");

        if (player == null)
            return;

        // ABSTRACTION
        // Enemy tidak perlu mengetahui detail cara Player menerima damage.
        // Enemy cukup menggunakan interface IDamageable.
        IDamageable target =
            player.GetComponent<IDamageable>();

        if (target != null)
        {
            target.KenaDamage(damage);
        }
    }




    void PerilakuIdle()
    {
        //Debug.Log("Enemy sedang IDLE");
    }



    void PerilakuPatrol()
    {
        //Debug.Log("Enemy sedang PATROL");
    }



    void PerilakuChase()
    {
        Kejar();

        //Debug.Log("Enemy sedang CHASE");
    }


    void PerilakuAttack()
    {
        if (player == null)
            return;

        PlayerMovement playerMovement =
            player.GetComponent<PlayerMovement>();

        if (playerMovement == null || playerMovement.SudahMati)
            return;

        if (Time.time >= waktuSerangTerakhir + JedaSerang)
        {
            Serang();
            Mati();
        }
    }



    public void KenaDamage(int jumlah)
    {
        // DITAMBAHKAN
        // Jangan menerima damage lagi kalau sudah mati.
        if (sudahMati)
            return;

        hp -= jumlah;

        Debug.Log(
            name +
            " menerima " +
            jumlah +
            " damage. HP sekarang: " +
            hp
        );

        if (hp <= 0)
        {
            Mati();
        }
    }



    protected virtual void Mati()
    {
        // DITAMBAHKAN
        // Pengaman utama agar Mati() hanya berjalan satu kali.
        if (sudahMati)
            return;

        // DITAMBAHKAN
        // Tandai zombie sudah mati SEBELUM event dijalankan.
        sudahMati = true;

        Debug.Log("Enemy mati!");



        // Memberitahu GameManager atau object lain
        // bahwa zombie ini sudah mati.
        OnZombieMati?.Invoke(this);


        // DIUBAH
        // Destroy hanya boleh dijalankan saat Play Mode.
        if (Application.isPlaying)
        {
            Destroy(gameObject);
        }
    }



    [ContextMenu("Tes Event: Zombie Mati")]
    void TesEventMati()
    {

        if (!Application.isPlaying)
        {
            Debug.LogWarning(
                "Tes Event: Zombie Mati hanya bisa dijalankan saat Play Mode."
            );

            return;
        }


        Mati();
    }
}