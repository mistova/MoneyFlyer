﻿using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Security;
using UnityEditor;
using UnityEngine;

public class ParticleManager : MonoBehaviour
{
    public static ParticleManager instance;

    public ParticleSystem PS_Confetti_1;
    //public ParticleSystem PS_Confetti_2;


    int layerMask;
    private void Awake()
    {
        if(instance == null)
            instance = this;

        layerMask = LayerMask.GetMask("Cube");

    }

    //Zaten sahnede olanı oynatıyoruz
    public void Play(ParticleSystem particle, bool loop)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public void Play(ParticleSystem particle, bool loop,Vector3 pos)
    {
        if (particle != null)
        {
            particle.gameObject.SetActive(true);
            particle.gameObject.transform.position = pos;
            ParticleSystem.MainModule main = particle.main;

            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;
            particle.Play();
        }

    }


    public void Stop(ParticleSystem particle,bool destroy)
    {
        ParticleSystem.MainModule main = particle.main;

        if(destroy)
            main.stopAction = ParticleSystemStopAction.Destroy;
        else
            main.stopAction = ParticleSystemStopAction.None;

        particle.Stop();
    }

    //Yeni obje oluşturup pozisyon verip başlatıyoruz
    public GameObject CreateAndPlay(ParticleSystem particle,GameObject parent,Vector3 position,bool loop)
    {
        GameObject returnPart = null;
        //Eğer particle atanmışsa
        if(particle != null)
        {
            ParticleSystem newParticle;
            if(parent == null) newParticle = Instantiate(particle);
            else newParticle = Instantiate(particle, parent.transform);

            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            //main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
            returnPart = newParticle.gameObject;
        }
        return returnPart;

    }

    public GameObject CreateAndPlayRandom(List<ParticleSystem> particles, GameObject parent, Vector3 position, bool loop)
    {
        int rand = Random.Range(0, particles.Count);
        GameObject returnPart = null;
        //Eğer particle atanmışsa
        if (particles[rand] != null)
        {
            ParticleSystem newParticle;
            if (parent == null) newParticle = Instantiate(particles[rand]);
            else newParticle = Instantiate(particles[rand], parent.transform);

            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            //main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
            returnPart = newParticle.gameObject;
        }
        else
            Debug.LogWarning("Particle list element is null. Element: " + rand);
        
        return returnPart;

    }

    //Belirli bir süree sonra oynatıyoruz
    public IEnumerator CreateAndPlay(ParticleSystem particle, GameObject parent, Vector3 position, bool loop,float time)
    {
        yield return new WaitForSeconds(time);
        //Eğer particle atanmışsa
        if (particle != null)
        {
            ParticleSystem newParticle = Instantiate(particle, parent.transform);
            newParticle.gameObject.SetActive(true);

            ParticleSystem.MainModule main = newParticle.main;
            main.loop = loop;
            main.stopAction = ParticleSystemStopAction.Destroy;

            newParticle.gameObject.transform.position = position;

            newParticle.Play();
        }
    }


    //public IEnumerator ParticleFollowWithRaycast(ParticleSystem particle,GameObject followObject,Vector3 offsett)
    //{

    //    while (GameManager.instance.isGameRunning)
    //    {
    //        Vector3 start = particle.transform.position + new Vector3(0, 0.3f, 0);
    //        Vector3 dir = Vector3.down;
    //        RaycastHit hitInfo;
    //        particle.transform.position = followObject.transform.position + offsett;

    //        if (Physics.Raycast(start, dir, out hitInfo, 1f, layerMask))
    //        {
    //            //particle.transform.rotation = Quaternion.Euler(-90, 0, 0);
    //            particle.transform.rotation = Quaternion.FromToRotation(Vector3.up, hitInfo.normal);
    //        }

    //        yield return null;
    //    }
    //}


    //public IEnumerator ParticleFollow(ParticleSystem particle, GameObject followObject, Vector3 offsett)
    //{

    //    while (GameManager.instance.isGameRunning)
    //    {
    //        Vector3 start = particle.transform.position + new Vector3(0, 0.3f, 0);
    //        Vector3 dir = Vector3.down;
    //        RaycastHit hitInfo;
    //        particle.transform.position = followObject.transform.position + offsett;

    //        yield return null;
    //    }
    //}


    //public bool IsParticleThere(ParticleSystem particle,GameObject parent)
    //{
    //    bool ans = false;
    //    foreach (Transform item in parent.transform)
    //    {
    //        if (item.GetComponent<ParticleSystem>() != null && item.GetComponent<ParticleSystem>() == particle) ans = true;
    //    }

    //    return ans;
    //}




}



