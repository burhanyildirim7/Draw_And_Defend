using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    public int cizimSiniri => (int)UIController.instance.meshSlider.maxValue;
    public int cizilenPixel = 0;

    [HideInInspector]public int score, elmas; // ayrintilar icin benioku 9. satirdan itibaren bak

    [HideInInspector] public bool isContinue;  // ayrintilar icin beni oku 19. satirdan itibaren bak

    //[HideInInspector] public List<GameObject> AllEnemies = new List<GameObject>();
    //[HideInInspector] public List<GameObject> AllSwarms = new List<GameObject>();

    [HideInInspector] public List<GameObject> EnemiesOnCastle = new List<GameObject>();

    //[HideInInspector] public List<Transform> SwarmsFistTransforms = new List<Transform>(); 

    [HideInInspector] public int castleHealth = 100;

    public GameObject meshCam,castle,king,flag;

    public bool isDrawable = true;

    Vector3 castleFirstPos, kingFirstPos;


	private void Awake()
	{
        if (instance == null) instance = this;
        //else Destroy(this);
	}

	void Start()
    {
        castleFirstPos = castle.transform.position;
        kingFirstPos = king.transform.position;
        isContinue = false;
        isDrawable = false;
        meshCam.SetActive(false);
    }


	/// <summary>
	/// Bu fonksiyon geçerli leveldeki scoreu belirtilen miktarda artirir veya azaltir. Artirma icin +5 gibi pozitif eksiltme
	/// icin -5 gibi negatif deger girin.
	/// </summary>
	/// <param name="eklenecekScore">Her collectible da ne kadar score eklenip cikarilacaksa parametre olarak o sayi verilmeli</param>
	public void SetScore(int eklenecekScore)
	{
        if(PlayerController.instance.collectibleVarMi) score += eklenecekScore;
        // Eðer oyunda collectible yok ise developer kendi score sistemini yazmalý...

    }


    /// <summary>
    /// Bu fonksiyon geçerli leveldeki elmasi belirtilen miktarda artirir veya azaltir. Artirma icin +5 gibi pozitif eksiltme
    /// icin -5 gibi negatif deger girin.
    /// </summary>
    /// <param name="eklenecekElmas">Her collectible da ne kadar elmas eklenip cikarilacaksa parametre olarak o sayi verilmeli</param>
    public void SetElmas(int eklenecekElmas)
    {
        elmas += eklenecekElmas;
        // buradaki elmas artýnca totalScore da otomatik olarak artacak.. bu sebeple asagidaki kodlar eklendi.
        PlayerPrefs.SetInt("totalElmas", PlayerPrefs.GetInt("totalElmas" + eklenecekElmas));
       // UIController.instance.SetTotalElmasText(); // totalElmaslarýn yazili oldugu texti
    }


    /// <summary>
    /// Oyun sonu x ler hesaplanip kac ile carpilacaksa parametre olacak o sayi gonderilmeli.
    /// </summary>
    /// <param name="katsayi"></param>
    public void ScoreCarp(int katsayi)
	{
        if (PlayerController.instance.xVarMi) score *= katsayi;
        else score = 1 * score;
        PlayerPrefs.SetInt("totalScore", PlayerPrefs.GetInt("totalScore") + score);
    }

    public void DecreaseCastleHealth()
	{
        
        castleHealth -= 10;
        if (castleHealth < 0) GameOverEvents();
        UIController.instance.SetCastleSlider(castleHealth);
	}

    public IEnumerator DelayAndActivateMeshCam()
	{
        yield return new WaitForSeconds(.8f);
        ActivateMeshCam();
	}
  
    public void ActivateMeshCam()
	{
        cizilenPixel = 0;
		isDrawable = true;
	}

    public void DeactivateMeshCam()
	{
        isDrawable = false;
        //StartCoroutine(DelayAndActivateMeshCam());
	}

    void GameOverEvents()
	{
        isContinue = false;
        isDrawable = false;
        meshCam.SetActive(false);
        castle.transform.DOMoveY(-12f,3);
        king.transform.DOMoveY(-4.2f,1).
            OnComplete(()=> {
                flag.SetActive(true);
                foreach(var enemy in EnemiesOnCastle)
				{
                    StartCoroutine(EnemiesAttackToKing(enemy));
                }        
                UIController.instance.ActivateLooseScreen();
            });
	}

    public void StartingEventsAfterTapToStart()
	{

        isDrawable = true;
        isContinue = true;
        cizilenPixel = 0;
        meshCam.SetActive(true);
    }

    public void StartingEventsAfterNextLevel()
    {
        StopAllCoroutines();
        foreach (var enemy in EnemiesOnCastle)
        {
            Destroy(enemy);
        }
        EnemiesOnCastle.Clear();
        king.transform.position = kingFirstPos;
        castle.transform.position = castleFirstPos;
        castleHealth = 100;
        UIController.instance.SetCastleSlider(castleHealth);
    }

    IEnumerator EnemiesAttackToKing(GameObject enemy)
    {
        int sayac = 0;
        float lerping = 0;
        int rnd = Random.Range(0,2);
        float xValue = Random.Range(-2f,2f);
        float zValue = Mathf.Sqrt((2 - xValue*xValue));
   
        Vector3 lastPos =new Vector3(king.transform.position.x + xValue, enemy.transform.position.y, king.transform.position.z -zValue);
        while (Vector3.Distance(enemy.transform.position,lastPos) > .1f)
        {
            enemy.transform.position = Vector3.Lerp(enemy.transform.position,lastPos,lerping);
            sayac++;
            lerping += .015f;
            yield return new WaitForSeconds(.05f);
        }
    }
}
