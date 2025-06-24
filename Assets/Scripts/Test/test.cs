using TMPro;
using UnityEngine;
using Image = UnityEngine.UI.Image;



public class test : MonoBehaviour  // Klasse erbt von MonoBehaviour, damit sie Unity-Komponenten sind

{
    public  int Würfel2 = 0;
    public int Alo1 = 0;
    public TMP_Text Chatt;
    public Image Würfel_img;
    public Sprite Würfel_1;
    public Sprite Würfel_2;
    public Sprite Würfel_3;
    public Sprite Würfel_4;
    public Sprite Würfel_5;
    public Sprite Würfel_6;

    private void Start()  // Wird einmal ausgeführt, wenn das Script gestartet wird
    {
        Würfel_img = GetComponent<Image>();
    }
    private void Update()

    {


        if (Input.GetKeyDown(KeyCode.H))
        {
            int Würfel = Random.Range(1, 6);
            if (Würfel == 1)
                Würfel_img.sprite = Würfel_1;
            if (Würfel == 2)
                Würfel_img.sprite = Würfel_2;
            if (Würfel == 3)
                Würfel_img.sprite = Würfel_3;
            if (Würfel == 4)
                Würfel_img.sprite = Würfel_4;
            if (Würfel == 5)
                Würfel_img.sprite = Würfel_5;
            if (Würfel == 6)
                Würfel_img.sprite = Würfel_6;


            int Alo = Random.Range(1, 6);
            if (Alo > Würfel)
                Chatt.text = "Alo War besser als du mit: " + Alo;
            Alo1++;
            if (Alo < Würfel)
                Chatt.text = "Alo hatt ales gegeben aber am ende war es nicht genug. " + Würfel + " Das ist gut gewürfelt";
            Würfel2++;
        }
        if (Input.GetKeyDown(KeyCode.J))
            Chatt.text = "Punkte stand: | Alo: " + Alo1 + "   |    User: " + Würfel2;
    }
}
