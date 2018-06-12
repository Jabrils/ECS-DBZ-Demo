using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering.PostProcessing;
using UnityEngine.Playables;
using UnityEngine.UI;
using Unity.Collections;
using Unity.Entities;
using Unity.Transforms;
using Unity.Mathematics;

public class ctrl : MonoBehaviour {

    public int amount;
    public float sbRadius;
    public GameObject spiritBombLoc;
    public GameObject spiritBomb, buu, begin, fin;
    public Text fps;
    public float energySpd = 1;
    public static bool fullyCharged;
    public static float eSpd;
    public static float3 bPos;
    Vector3 sbLoc { get { return spiritBombLoc.transform.position; } }
    public static bool done;
    public Animator anim;
    public PostProcessVolume pV;
    public InputField[] inps;

    NativeArray<Entity> ents;
    EntityManager eM;

    PlayableDirector pD;

    void Start()
    {
        fin.SetActive(false);
    }

    void StartDBZDemo()
    {
        amount = int.Parse(inps[0].text);
        sbRadius = float.Parse(inps[1].text);
        energySpd = float.Parse(inps[2].text);


        pD = GetComponent<PlayableDirector>();
        pD.Play();
    }

    // Update is called once per frame
    void Update () {
        eSpd = energySpd;
        bPos = sbLoc;
        fps.text = $"Entities: {amount}\n"+Mathf.Round(1 / Time.deltaTime);
        begin.SetActive(pD == null);

        //
        if (Input.GetKeyDown(KeyCode.Return))
            {
            if (pD == null)
            {
                StartDBZDemo();
            }
            else
            {
                //SceneManager.LoadScene("preload");
            }
        }

        if (pD != null)
        {
            // 
            if (!done && pD.time > 12)
            {
                StartSpiritBomb();
            }

            // 
            if (pD.time > 35.5f)
            {
                energySpd += .05f;
            }

            // 
            if (pD.time > 55f)
            {
                fullyCharged = true;
            }

            // 
            if (pD.time > 61.3f)
            {
                spiritBombLoc.transform.LookAt(buu.transform);
                spiritBombLoc.transform.Translate(Vector3.forward * .5f);
            }

            // 
            if (pD.time > 62.5f)
            {
                Bloom b = null;
                pV.profile.TryGetSettings(out b);

                b.intensity.value += .15f;
            }

            // 
            if (pD.time > 69)
            {
                fin.SetActive(true);
            }
        }

            //
            if(Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }

    // 
    public void StartSpiritBomb()
    {
        if (!done)
        {
            eM = World.Active.GetOrCreateManager<EntityManager>();
            ents = new NativeArray<Entity>(amount, Allocator.Temp);
            eM.Instantiate(spiritBomb, ents);

            spiritBombLoc.transform.localPosition += Vector3.up * (sbRadius * (1.1f));

            // 
            for (int i = 0; i < amount; i++)
            {
                eM.SetComponentData(ents[i], new Position { Value = new float3(sbLoc.x + Random.Range(-1800f, 1800f), 0, sbLoc.z + Random.Range(-1800f, 1800f)) });

                // Randomize a location around the spirit bomb location
                Vector3 newLoc = new Vector3(sbLoc.x + Random.Range(-100f, 100f), sbLoc.y + Random.Range(-100f, 100f), sbLoc.z + Random.Range(-100f, 100f));

                // calculate the distance from the spirit bomb location
                float distance = Vector3.Distance(newLoc, sbLoc);

                // get the vector from center of spirit bomb location
                Vector3 fromCenter = newLoc - sbLoc;

                // if distance is greater than defined radius
                if (distance > sbRadius)
                {
                    // scale the vector to sit on the edge / clamp the domain of the vector
                    fromCenter *= sbRadius / distance;
                }

                eM.AddComponentData(ents[i], new BombEnergy { destOffset = fromCenter });
            }

            ents.Dispose();
        }
        done = true;
    }

}
