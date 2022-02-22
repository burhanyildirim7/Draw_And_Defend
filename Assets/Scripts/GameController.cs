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

    public GameObject castle,king,flag,inputListener,mainCam,enemyLookAtObject;

    public bool isDrawable = true;

    public Animator kingAnimator;
    Sequence sequence;
    private bool enemyToRight = true;
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


  

    public void StartingEventsAfterTapToStart()
    {

        isContinue = true;
        cizilenPixel = 0;
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
        ResetAllTrigger();
        king.GetComponentInChildren<Animator>().SetTrigger("idle");
        CameraContoller.instance.ActivateCinemachine();
        king.transform.GetChild(0).transform.DORotate(new Vector3(0, 180, 0), .5f);
    }


    // win sonu...
    public void FinishLevelEvents()
	{
        ScoreCarp(1);
        isContinue = false;
        ResetAllTrigger();
        king.GetComponentInChildren<Animator>().SetTrigger("victory");       
        UIController.instance.ActivateWinScreen();
        king.transform.GetChild(0).transform.DORotate(new Vector3(0,0,0),.5f);
        CameraContoller.instance.DeactivateCinemachine();
        mainCam.transform.DOMove(new Vector3(0,3.1f,28f),.3f);
        // sevineceklerrr... falan filann sonra win ekraný...
	}

    // fail game sonu
    void GameOverEvents()
	{
        isContinue = false;
        isDrawable = false;
        castle.transform.DOMoveY(-4f, .6f).OnComplete(() => EnemiesMoveToKing());    
    }

    private void EnemiesMoveToKing()
	{
        GameObject[] swarms = GameObject.FindGameObjectsWithTag("swarn");
        foreach(GameObject swarm in swarms)
		{
            Destroy(swarm);
		}
        //for (int i = 0; i < swarms.Length; i++)
        //{
        //    Destroy(swarms[0]);
        //}
        float radius = .8f;
        for (int i = 0; i < EnemiesOnCastle.Count; i++)
        {
            float angle = i * Mathf.PI * 2f / EnemiesOnCastle.Count;
            Vector3 newPos = new(king.transform.GetChild(0).transform.position.x + Mathf.Cos(angle) * radius, EnemiesOnCastle[i].transform.position.y, king.transform.GetChild(0).transform.position.z + Mathf.Sin(angle) * radius);
            EnemiesOnCastle[i].transform.DOMove(newPos, .6f).OnComplete(() => EnemiesLookAtKing());//

        }
        UIController.instance.ActivateLooseScreen();
    }

    private void EnemiesLookAtKing()
	{
        for (int i = 0; i < EnemiesOnCastle.Count; i++)
        {
            EnemiesOnCastle[i].transform.LookAt(enemyLookAtObject.transform, Vector3.up);
            EnemiesOnCastle[i].transform.Rotate(EnemiesOnCastle[i].transform.rotation.x, EnemiesOnCastle[i].transform.rotation.y + 179, EnemiesOnCastle[i].transform.rotation.z);
            ResetAllTrigger();
            king.GetComponentInChildren<Animator>().SetTrigger("die");
            StartCoroutine(StopCastleEnemies());
        }
    }

    private IEnumerator StopCastleEnemies()
    {
        yield return new WaitForSeconds(2.5f);
        for (int i = 0; i < EnemiesOnCastle.Count; i++)
        {
            EnemiesOnCastle[i].GetComponent<Animator>().SetTrigger("idle");
        }
    }


    private IEnumerator RunAllEnemies()
	{
        yield return new WaitForSeconds(.2f);
        GameObject[] enemies = GameObject.FindGameObjectsWithTag("enemy");
        for(int i=0; i < enemies.Length; i++)
		{
            enemies[i].GetComponent<Animator>().SetTrigger("run");
		}
	}

    private void ResetAllTrigger()
	{
        king.GetComponentInChildren<Animator>().ResetTrigger("die");
        king.GetComponentInChildren<Animator>().ResetTrigger("victory");
        king.GetComponentInChildren<Animator>().ResetTrigger("idle");
        king.GetComponentInChildren<Animator>().ResetTrigger("attack");
    }

    
}
