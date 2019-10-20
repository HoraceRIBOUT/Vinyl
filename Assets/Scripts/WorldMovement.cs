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
    public static bool theEnd;
    public static bool gameover;

    [Header("Dot, relief")]
    public List<Transform> eachDotFromMusique;
    public GameObject dotGameObject;
    public Transform spawnPoint;


    //LD
    [System.Serializable]
    public class BeatData
    {
        public float yHeightOfGround;

        public List<GameObject> prefabToSpawnInHere = new List<GameObject>();

        [Header("Don't touch, only info")]
        public Transform dot;
        public float timeSum = 0;
        public float beatTiming = 0;
    }

    [Header("Level Design")]
    public List<BeatData> beatDatas = new List<BeatData>();
    public int currentSpawningIndex = 1;
    public float timeSum = 0;


    [Header("Ground drawing")]
    public GameObject groundObj;


    public void Start()
    {
        theEnd = false;
        gameover = false;
        GameManager.instance.player.inverseMovement = mainMovementSpeed * -1;
    }

    public void Update()
    {
        int index = 0;
        foreach (Strate strate in strateWhoMoves)
        {
            if (!theEnd)
            {
                Vector3 strateMovementSpeed = mainMovementSpeed * (Mathf.Pow(movementSlowPerStrate, index++));
                strate.main.Translate(strateMovementSpeed * Time.deltaTime);
            }
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


    public void Beat()
    {
        CreateNextDot();
    }
    


    void CreateNextDot()
    {
        Vector3 positionSpawn = spawnPoint.position;
        positionSpawn.y = beatDatas[currentSpawningIndex].yHeightOfGround;

        GameObject gO = Instantiate(dotGameObject, positionSpawn, spawnPoint.rotation, strateWhoMoves[0].main.transform);
        foreach(GameObject prefa in beatDatas[currentSpawningIndex].prefabToSpawnInHere)
            Instantiate(prefa, positionSpawn, spawnPoint.rotation, gO.transform);

        timeSum += GameManager.instance.beatTiming;
        beatDatas[currentSpawningIndex].beatTiming = GameManager.instance.beatTiming;
        beatDatas[currentSpawningIndex].timeSum = timeSum;


        beatDatas[currentSpawningIndex].dot = gO.transform;

        currentSpawningIndex++;
        if(beatDatas.Count == currentSpawningIndex)
        {

            theEnd = true;
            Debug.LogError("We are on the end my friend ");
        }

        //useless but still
        eachDotFromMusique.Add(gO.transform);


        //Create ground between two dot : 
        if (eachDotFromMusique.Count > 1)
            CreateGround(eachDotFromMusique[eachDotFromMusique.Count - 2].position, eachDotFromMusique[eachDotFromMusique.Count - 1].position);
    }

    void CreateGround(Vector3 dotA, Vector3 dotB)
    {
        Vector3 positionGr = (dotA + dotB) / 2f;
        float ratio = (dotB.y - dotA.y) / (dotB.x - dotA.x);
        float zValue = Mathf.Rad2Deg*Mathf.Atan(ratio);
        Quaternion rotationGr = Quaternion.Euler(0, 0, zValue);
        //Debug.Log("Ratio : " + ratio + " zVal = " + zValue + " quaternion = " + rotationGr);

        GameObject groundObject = Instantiate(groundObj, positionGr, rotationGr, strateWhoMoves[0].main.transform);
        float magnitudeOfDifference = (dotB - dotA).magnitude;
        
        groundObject.transform.localScale += Vector3.right * magnitudeOfDifference - Vector3.right;
    }



    ////////////////////
    //For testing : 
    [ContextMenu("Change y for random")]
    void ChangeHeightOnIt()
    {
        float souvenir = 0;
        foreach (BeatData bD in beatDatas)
        {
            if(souvenir == 0)
            {
                float rand = Random.Range(-3f, 3f);
                bD.yHeightOfGround = rand;
                if (Random.Range(-3f, 5f) < 0)
                    souvenir = rand;
            }
            else
            {
                bD.yHeightOfGround = souvenir;
                souvenir = 0;
            }
        }
    }

}
