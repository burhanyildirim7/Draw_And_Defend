using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class GameController : MonoBehaviour
{
    public static GameController instance; // singleton yapisi icin gerekli ornek ayrintilar icin BeniOku 22. satirdan itibaren bak.
    public int cizimSiniri => (int)UIController.instance.meshSlider.maxValue;
    public int cizilenPixel = 0;

    public float swarmSpeed = .02f;

    [HideInInspector]public int score, elmas; // ayrintilar icin benioku 9. satirdan itibaren bak

    [HideInInspector] public bool isContinue,isLastSwarm;  // ayrintilar icin beni oku 19. satirdan itibaren bak

    [HideInInspector] public List<GameObject> EnemiesOnCastle = new List<GameObject>();

    [HideInInspector] public int castleHealth = 100;

    public GameObject meshCam,castle,king,flag,inputListener;

    public bool isDrawable = true;

    public Animator kingAnimator;

    Vector3 castleFirstPos, kingFirstPos;


	private void Awake()
	{
        if (instance == null) instance = this;
        else Destroy(this);
	}

	void Start()
    {

        castleFirstPos = castle.transform.position;
        kingFirstPos = king.transform.position;
        StartingEventsAfterNextLevel();
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
        score += eklenecekScore;
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
        //if (PlayerController.instance.xVarMi) score *= katsayi;
        //else score = 1 * score;
        score *= katsayi;
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
        inputListener.SetActive(true);
	}

    public void DeactivateMeshCam()
	{
        inputListener.SetActive(false);
	}

    public void StartingEventsAfterTapToStart()
    {

        isDrawable = true;
        isContinue = true;
        cizilenPixel = 0;
        meshCam.SetActive(true);
        inputListener.SetActive(true);
        isLastSwarm = false;
        StartCoroutine(RunAllEnemies());
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
        inputListener.SetActive(false);
    }


    // win sonu...
    public void FinishLevelEvents()
	{
        meshCam.SetActive(false);
        DeactivateMeshCam();
        ScoreCarp(1);
        isContinue = false;
        UIController.instance.ActivateWinScreen();

        // sevineceklerrr... falan filann sonra win ekraný...
	}

    // fail game sonu
    void GameOverEvents()
	{
        isContinue = false;
        isDrawable = false;
        meshCam.SetActive(false);
        castle.transform.DOMoveY(-4f,.6f).OnComplete(() => {
            flag.SetActive(true);
            int enemyCount = EnemiesOnCastle.Count;
            float sayac = 0;
            foreach (var enemy in EnemiesOnCastle)
            {
                float xValue = -.7f + ((1.4f * sayac) / enemyCount);
                StartCoroutine(EnemiesAttackToKing(enemy, xValue));
                sayac += 1f;
            }
            UIController.instance.ActivateLooseScreen();
        });         
	}


    IEnumerator EnemiesAttackToKing(GameObject enemy , float xValue)
    {
        float lerping = 0;
        float zValue = Mathf.Sqrt((2f - (xValue*xValue)));
   
        Vector3 lastPos =new Vector3(king.transform.position.x + xValue, enemy.transform.position.y, king.transform.position.z -zValue);
        while (Vector3.Distance(enemy.transform.position,lastPos) > .1f)
        {
            enemy.transform.position = Vector3.Lerp(enemy.transform.position,lastPos,lerping);
            lerping += .015f;
            yield return new WaitForSeconds(.05f);
        }
    }

    public IEnumerator RunAllEnemies()
	{
        yield return new WaitForSeconds(.2f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        for(int i=0; i < enemies.Length; i++)
		{
            enemies[i].GetComponent<Animator>().SetTrigger("run");
		}
	}

    
}
