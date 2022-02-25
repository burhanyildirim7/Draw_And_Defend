using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    public GameObject splashPrefab;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("mesh"))
        {
            MoreMountains.NiceVibrations.MMVibrationManager.Haptic(MoreMountains.NiceVibrations.HapticTypes.MediumImpact);
            GetComponent<Collider>().enabled = false;
            UIController.instance.EnemyScoreFunc(transform.position, transform.parent.gameObject);
            GameObject tempSplash = Instantiate(splashPrefab, transform.position, Quaternion.identity);
            Destroy(tempSplash, 2f);
            Destroy(gameObject);
        }
        else if (other.CompareTag("kale"))
        {
            // restart game i?in tutulaml?..
            transform.parent = null;
            StartCoroutine(RandomPosition());
            GetComponent<Collider>().enabled = false;
            GameController.instance.EnemiesOnCastle.Add(gameObject);
            GetComponent<Animator>().SetTrigger("attack");
            // sald?r? yap?yor...
            GameController.instance.DecreaseCastleHealth();

        }
    }


    IEnumerator RandomPosition()
    {
        int sayac = 0;
        float xValue = Random.Range(-.02f, .02f);
        float zValue = Random.Range(-.01f, .01f);
        while (sayac < 60)
        {
            transform.position += new Vector3(xValue, 0, zValue);
            sayac++;
            yield return new WaitForSeconds(.02f);
        }
    }



}
