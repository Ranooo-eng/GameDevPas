using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public int totalKoin;
    private int koinTerkumpul = 0;

    [SerializeField] private int skor = 0;


    private int jumlahZombieMati = 0;

    void Start()
    {
        // TODO: hitung jumlah koin di scene saat mulai
        totalKoin = GameObject.FindGameObjectsWithTag("Coin").Length;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void AmbilKoin()
    {
        koinTerkumpul++;

        // TODO: jika koinTerkumpul == totalKoin, panggil Menang()
        if (koinTerkumpul == totalKoin)
        {
            Menang();
        }
    }

    void Menang()
    {
        Debug.Log("KAMU MENANG!");
    }




    void OnEnable()
    {
        Enemy.OnZombieMati += TambahSkorSaatZombieMati;

        Enemy.OnZombieMati += SaatZombieMati;
    }

    void OnDisable()
    {
        Enemy.OnZombieMati -= TambahSkorSaatZombieMati;

        Enemy.OnZombieMati -= SaatZombieMati;
    }


    void TambahSkorSaatZombieMati(Enemy zombieYangMati)
    {
        skor += 10;
        Debug.Log("Skor: " + skor);
    }


    void SaatZombieMati(Enemy zombie)
    {
        jumlahZombieMati++;

        Debug.Log(
            "GameManager dengar event. Zombie mati: "
            + jumlahZombieMati
            + " ("
            + zombie.name
            + ")"
        );
    }



    void OnGUI()
    {
        GUI.skin.label.fontSize = 22;

        GUI.Label(
            new Rect(16, 16, 480, 36),
            "Koin: " + koinTerkumpul + " / " + totalKoin
        );

        GUI.Label(
            new Rect(16, 52, 480, 36),
            "Skor: " + skor
        );

        GUI.Label(
            new Rect(16, 88, 480, 36),
            "Zombie mati: " + jumlahZombieMati
        );
    }
}