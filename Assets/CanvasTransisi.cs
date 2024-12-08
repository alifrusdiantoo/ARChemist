using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasTransisi : MonoBehaviour
{
    public static string NamaScene;

    public void BtnPindah(string nama)
    {
        this.gameObject.SetActive(true);
        NamaScene = nama;
        GetComponent<Animator>().Play("end");
    }

    public void ObjectInActive()
    {
        this.gameObject.SetActive(false);
    }

    public void PindahScene()
    {
        SceneManager.LoadScene(NamaScene);
    }

    public void BtnKeluar()
    {
        Application.Quit();
    }
}
