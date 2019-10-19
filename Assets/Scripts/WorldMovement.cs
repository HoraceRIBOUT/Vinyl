using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldMovement : MonoBehaviour
{
    [System.Serializable]
    public class Strate
    {
        public Transform main;
        //public List<Transform> listTrans;
    }
    [Header("Paralaxes")]
    public List<Strate> strateWhoMoves = new List<Strate>();
    public Vector3 mainMovementSpeed = new Vector3(-1,0,0);
    public float movementSlowPerStrate = 0.5f;

    [Header("Dot, relief")]
    public List<Transform> eachDotFromMusique;
    public GameObject dotGameObject;
    public Transform spawnPoint;

    public void Start()
    {
        GameManager.instance.player.inverseMovement = mainMovementSpeed * -1;

        StartCoroutine(createDotRegulary());
    }

    public void Update()
    {
        int index = 0;
        foreach (Strate strate in strateWhoMoves)
        {
            Vector3 strateMovementSpeed = mainMovementSpeed * (Mathf.Pow(movementSlowPerStrate, index++));

            strate.main.Translate(strateMovementSpeed * Time.deltaTime);
        }
        
    }

    public void OnGUI()
    {
        if (!GameManager.instance._DebugMode || eachDotFromMusique.Count < 1)
            return;
        
        for (int indexDot = 0; indexDot < eachDotFromMusique.Count-1; indexDot++)
        {
            Vector3 positionA = eachDotFromMusique[indexDot].transform.position;
            Vector3 positionB = eachDotFromMusique[indexDot + 1].transform.position;
            Debug.DrawLine(positionA, positionB, Color.blue, 1f/30f);

        }
    }


    IEnumerator createDotRegulary()
    {
        while (true)
        {
            CreateNextDot();
            yield return new WaitForSeconds(2f);
        }
    }


    void CreateNextDot()
    {
        GameObject gO = Instantiate(dotGameObject, spawnPoint.position, spawnPoint.rotation, strateWhoMoves[0].main.transform);

        gO.transform.position = new Vector3(gO.transform.position.x, Random.Range(-3f, 3f), gO.transform.position.z);

        eachDotFromMusique.Add(gO.transform);
    }

}
