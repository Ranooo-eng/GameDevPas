using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour, IDamageable
{
    [Header("Movement")]
    public float kecepatan = 5f;

    private Vector2 arahGerak;
    private Rigidbody2D rb;

    public Vector2 ArahGerak => arahGerak; // DITAMBAHKAN: Memberikan akses arah gerakan ke script animasi
    public Vector2 LastDirection { get; private set; } = Vector2.down; // DITAMBAHKAN: Menyimpan arah terakhir Player

    [SerializeField] private InputActionReference moveAction;

    [Header("Player Stats")]
    [SerializeField] private int hp = 100;
    [SerializeField] private HealthBarUI healthBarUI; // DITAMBAHKAN: Referensi ke UI Health Bar
    public bool SudahMati { get; private set; } = false;
    public float RemainingHealthPercentage => Mathf.Clamp01((float)hp / 100f); // DITAMBAHKAN

    [Header("Coin")]
    public int skor = 0;

    [Header("Health Bar Test")]
    [SerializeField] private bool aktifkanTesDamage = true; // DITAMBAHKAN: Mengaktifkan tes damage dengan tombol H

    void Awake()
    {
        rb = GetComponent<Rigidbody2D>();

        if (rb == null)
        {
            Debug.LogError("Rigidbody2D tidak ditemukan pada Player!", this);
        }
    }

    void Start()
    {
        UpdateHealthBar(); // DITAMBAHKAN: Menampilkan HP awal saat game dimulai
    }

    void OnEnable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Enable();
        }
    }

    void OnDisable()
    {
        if (moveAction != null && moveAction.action != null)
        {
            moveAction.action.Disable();
        }

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }
    }

    void Update()
    {
        if (moveAction != null && moveAction.action != null)
        {
            arahGerak = moveAction.action.ReadValue<Vector2>().normalized;

            if (arahGerak != Vector2.zero) // DITAMBAHKAN: Periksa apakah Player sedang bergerak
            {
                LastDirection = arahGerak; // DITAMBAHKAN: Simpan arah terakhir untuk animasi
            }
        }

        // DITAMBAHKAN: Tes Health Bar menggunakan tombol H
        if (aktifkanTesDamage &&
            Keyboard.current != null &&
            Keyboard.current.hKey.wasPressedThisFrame)
        {
            KenaDamage(20);
        }
    }

    void FixedUpdate()
    {
        if (rb != null)
        {
            rb.linearVelocity = arahGerak * kecepatan;
        }
    }

    public void KenaDamage(int jumlah)
    {
        if (hp <= 0)
        {
            return;
        }

        hp -= jumlah;

        hp = Mathf.Max(0, hp); // DITAMBAHKAN: Mencegah HP menjadi negatif

        UpdateHealthBar(); // DITAMBAHKAN: Memperbarui Health Bar setelah terkena damage

        Debug.Log(
            name + " kena " +
            jumlah +
            " damage. HP sekarang: " +
            hp
        );

        if (hp <= 0)
        {
            Mati();
        }
    }

    void Mati()
    {
        Debug.Log("PLAYER MATI!");
        
        SudahMati = true;

        arahGerak = Vector2.zero;

        if (rb != null)
        {
            rb.linearVelocity = Vector2.zero;
        }

        enabled = false;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Coin"))
        {
            Destroy(other.gameObject);

            skor++;

            Debug.Log("Skor Coin Player: " + skor);

            GameManager gameManager =
                FindFirstObjectByType<GameManager>();

            if (gameManager != null)
            {
                gameManager.AmbilKoin();
            }
        }
    }

    void UpdateHealthBar() // DITAMBAHKAN: Memperbarui tampilan Health Bar
    {
        if (healthBarUI != null)
        {
            healthBarUI.UpdateHealthBar(RemainingHealthPercentage);
        }
        else
        {
            Debug.LogWarning("HealthBarUI belum dihubungkan pada Player!", this);
        }
    }
}

