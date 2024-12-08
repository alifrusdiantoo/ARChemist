using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Sistem : MonoBehaviour
{
    public static Sistem instance; // Singleton instance

    [System.Serializable]
    public class GolonganData
    {
        public string golongan; // Nama golongan
        public Material golonganMaterial; // Material khusus untuk golongan
        public Material[] unsurKimia; // Material unsur-unsur di golongan ini
    }

    public GolonganData[] golonganList; // Daftar semua golongan dan unsurnya
    private Dictionary<string, GolonganData> golonganDictionary; // Untuk akses cepat golongan

    public GameObject spawnParent; // Parent untuk objek yang di-spawn
    public GameObject templateObject; // Template prefab objek
    public GameObject controlButtons;

    public Button nextButton; // Tombol Next
    public Button previousButton; // Tombol Previous
    public Button backButton; // Tombol Kembali
    public Button homeButton; // Tombol Kembali

    private List<GameObject> spawnedObjects = new List<GameObject>(); // Untuk melacak objek yang di-spawn
    private int currentIndex = 0; // Indeks objek golongan yang sedang ditampilkan
    private int currentUnsurIndex = 0; // Indeks unsur yang sedang ditampilkan
    private bool showingGolongan = true; // Apakah sedang menampilkan golongan
    private string currentGolonganName; // Nama golongan saat mode unsur

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        // Konversi data array ke dictionary
        golonganDictionary = new Dictionary<string, GolonganData>();
        foreach (var golongan in golonganList)
        {
            golonganDictionary.Add(golongan.golongan, golongan);
        }

        // Tampilkan golongan pertama
        //ShowGolongan();
        Audio.instance.PutarBGM();
        UpdateNavigationButtons();
    }

    // Fungsi untuk menampilkan daftar golongan
    public void ShowGolongan()
    {
        ClearSpawnedObjects(); // Bersihkan objek sebelumnya
        showingGolongan = true;
        currentIndex = Mathf.Clamp(currentIndex, 0, golonganList.Length - 1); // Pastikan indeks valid

        var golongan = golonganList[currentIndex];
        GameObject golonganObject = Instantiate(templateObject, spawnParent.transform);
        golonganObject.name = golongan.golongan;

        Renderer renderer = golonganObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = golongan.golonganMaterial; // Gunakan material khusus golongan
        }

        // Tambahkan event klik
        golonganObject.AddComponent<GolonganClickHandler>().Initialize(golongan.golongan);
        spawnedObjects.Add(golonganObject);
        spawnParent.GetComponent<Animation>().Play("popup");
        Audio.instance.PanggilSuara(1);

        UpdateNavigationButtons();
    }

    // Fungsi untuk menampilkan daftar unsur dari golongan tertentu
    public void ShowUnsurByGolongan(string golongan)
    {
        if (golonganDictionary.TryGetValue(golongan, out GolonganData golonganData))
        {
            ClearSpawnedObjects(); // Bersihkan objek sebelumnya
            showingGolongan = false;
            currentGolonganName = golongan;
            currentUnsurIndex = 0; // Mulai dari unsur pertama

            // Tampilkan unsur pertama
            ShowUnsur(golonganData.unsurKimia[currentUnsurIndex]);
        }
        else
        {
            Debug.LogError($"Golongan {golongan} tidak ditemukan!");
        }
    }

    // Fungsi untuk menampilkan satu unsur berdasarkan material
    private void ShowUnsur(Material unsurMaterial)
    {
        ClearSpawnedObjects(); // Hapus objek sebelumnya

        GameObject unsurObject = Instantiate(templateObject, spawnParent.transform);
        unsurObject.name = unsurMaterial.name;

        Renderer renderer = unsurObject.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = unsurMaterial;
        }

        spawnedObjects.Add(unsurObject);
        spawnParent.GetComponent<Animation>().Play("popup");
        Audio.instance.PanggilSuara(1);

        UpdateNavigationButtons();
    }

    // Fungsi untuk navigasi ke objek berikutnya
    public void Next()
    {
        if (showingGolongan)
        {
            currentIndex = (currentIndex + 1) % golonganList.Length;
            ShowGolongan();
        }
        else
        {
            GolonganData golonganData = golonganDictionary[currentGolonganName];
            currentUnsurIndex = (currentUnsurIndex + 1) % golonganData.unsurKimia.Length;
            ShowUnsur(golonganData.unsurKimia[currentUnsurIndex]);
        }
    }

    // Fungsi untuk navigasi ke objek sebelumnya
    public void Previous()
    {
        if (showingGolongan)
        {
            currentIndex = (currentIndex - 1 + golonganList.Length) % golonganList.Length;
            ShowGolongan();
        }
        else
        {
            GolonganData golonganData = golonganDictionary[currentGolonganName];
            currentUnsurIndex = (currentUnsurIndex - 1 + golonganData.unsurKimia.Length) % golonganData.unsurKimia.Length;
            ShowUnsur(golonganData.unsurKimia[currentUnsurIndex]);
        }
    }

    // Fungsi untuk kembali ke daftar golongan
    public void BackToGolongan()
    {
        ShowGolongan();
    }

    // Bersihkan semua objek yang di-spawn
    private void ClearSpawnedObjects()
    {
        foreach (var obj in spawnedObjects)
        {
            Destroy(obj);
        }
        spawnedObjects.Clear();
    }

    // Perbarui status tombol navigasi
    private void UpdateNavigationButtons()
    {
        if (showingGolongan)
        {
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(false);
            homeButton.gameObject.SetActive(true);
        }
        else
        {
            nextButton.gameObject.SetActive(true);
            previousButton.gameObject.SetActive(true);
            backButton.gameObject.SetActive(true);
            homeButton.gameObject.SetActive(false);
        }
    }

    // Fungsi untuk event OnTargetFound
    public void OnTargetFound()
    {
        if (controlButtons != null)
        {
            controlButtons.SetActive(true); // Tampilkan tombol jika target ditemukan
            Debug.Log("Target ditemukan. Menampilkan tombol kontrol.");
        }
    }

    // Fungsi untuk event OnTargetLost
    public void OnTargetLost()
    {
        if (controlButtons != null)
        {
            controlButtons.SetActive(false); // Sembunyikan tombol jika target hilang
            Debug.Log("Target hilang. Menyembunyikan tombol kontrol.");
        }
    }
}

// Script tambahan untuk menangani klik pada objek golongan
public class GolonganClickHandler : MonoBehaviour
{
    private string golonganName;

    // Inisialisasi dengan nama golongan
    public void Initialize(string golongan)
    {
        golonganName = golongan;
    }

    private void OnMouseDown()
    {
        // Panggil fungsi untuk menampilkan daftar unsur
        Sistem.instance.ShowUnsurByGolongan(golonganName);
    }
}
