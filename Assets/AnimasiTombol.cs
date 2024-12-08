using UnityEngine;

public class AnimasiTombol : MonoBehaviour
{
    public void _Animasi()
    {
        GetComponent<Animation>().Play("button");
        Audio.instance.PanggilSuara(0);
    }

    public void Menu()
    {
        GetComponent<Animation>().Play("menu");
        Audio.instance.PanggilSuara(0);
    }
}
