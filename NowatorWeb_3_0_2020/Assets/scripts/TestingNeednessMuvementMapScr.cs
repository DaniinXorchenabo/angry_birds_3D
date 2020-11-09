using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Data;
using System;
using static System.IO.File;
using System.Text;

public class TestingNeednessMuvementMapScr : MonoBehaviour
{
    public Transform player;
    public GameObject Cam;
    private FlyingScript flyScr;
    private Rigidbody _rb;
    private MuvementMapLib mapsLib;
    private float mass;
    private float stiffness_coefficient;
    


    void Awake(){
        flyScr = player.gameObject.GetComponent<FlyingScript>();
        _rb = player.gameObject.GetComponent<Rigidbody>();
        mapsLib = gameObject.GetComponent<MuvementMapLib>();
        try
        {
            mass = _rb.mass;
            stiffness_coefficient = flyScr.stiffness_coefficient;
            var some = mapsLib.muveMap[_rb.mass][flyScr.stiffness_coefficient];
            gameObject.GetComponent<DictTimeDistanceCreater>().enabled = false;
            this.enabled = false;
        }
        catch (SystemException  e)
        {
            Debug.Log("Error!  " + e);
            Debug.Log("Подождите... Идёт попытка исправить...");
            gameObject.GetComponent<DictTimeDistanceCreater>().enabled = true;
            player.gameObject.GetComponent<DistanceToTimeTranslaterScript>().enabled = false;
            Cam.SetActive(false);
        
        }
    }

    float WhatIShouldDo(float flagOrMass){
        try
        {
            var some = mapsLib.muveMap[_rb.mass];
            mass = -mass;
            flagOrMass = -flagOrMass;
            //значит такая масса есть, но нет такого коэфициента
        }
        catch (SystemException  e)
        {

            //значит нет такой массы, сапускаем процесс сначала
        }
        return flagOrMass;
    }
    //str.StartsWith("/*clone*/") 
    //string[] words = text.Split(new char[] { ' ' });
    //string[] words = text.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
    public void WorkWithFile(float flagOrMass, float k, string stringMap){
        flagOrMass = WhatIShouldDo(flagOrMass);
        var fileName = "MuvementMapLib"; //"muvement";
        var path = "Assets/scripts/maps/" + fileName + ".cs";
        if (!System.IO.File.Exists(path)){
            System.IO.FileStream fsCreate = new System.IO.FileStream(path, System.IO.FileMode.Create);  
            byte[] bdata = Encoding.Default.GetBytes(CreateEmputyFile(fileName));
            fsCreate.Write(bdata, 0, bdata.Length);              
            fsCreate.Close();
        }
        System.IO.FileStream fs_read = new System.IO.FileStream(path, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Read);   
        using (System.IO.StreamReader sr = new System.IO.StreamReader(fs_read))
        {
            var mainData = sr.ReadToEnd().Split(new char[] { '\n' }, StringSplitOptions.RemoveEmptyEntries);
            fs_read.Close();
            var newMainData = "";
            var passFlag = false;
            if (flagOrMass >= 0f){
                foreach (string str in mainData){
                    if (str.StartsWith("/*new_mass*/") && !passFlag){
                        passFlag = true;
                        newMainData += str +"\n";
                        newMainData += DecorateToMass(flagOrMass, k, stringMap + "\n");
                        // + моя дата
                        
                    } else {
                        newMainData += str +"\n";
                    }
                }
            } else {
                flagOrMass *= -1;
                var findingStr = "/*mass " + flagOrMass.ToString().Replace(",", ".") + "f" + "*/";
                bool findNewKFlag = false;
                foreach (string str in mainData){
                    if (str.StartsWith(findingStr) && !passFlag){
                        findNewKFlag = true;
                        newMainData += str +"\n";
                    } else if (findNewKFlag && str.StartsWith("/*new_k*/")) {
                        findNewKFlag = false;
                        passFlag = true;
                        newMainData += str +"\n";
                        newMainData += DecorteToK(k, stringMap + "\n");
                        // + моя дата

                    } else {
                        newMainData += str +"\n";
                    }
                }
            }
            System.IO.FileStream fsWrite = new System.IO.FileStream(path, System.IO.FileMode.Create);  
            byte[] bMainData = Encoding.Default.GetBytes(newMainData);
            fsWrite.Write(bMainData, 0, bMainData.Length);  
            fsWrite.Close();

        }
        print("ended write");
        print("-------------------------------------");
        print("Пожалуйста!!! Откройте MuvementMapLib.cs скрипт!!!!");
        print("-------------------------------------");
        
    }

    string MapToString(Dictionary<float, float> data){
        string str = "/*data*/                    ";
        foreach (KeyValuePair<float, float> kvp in data) {
            str += KeyValFloatTranslateToString(kvp.Key, kvp.Value);
        }
        return str + "\n";
    }
    string KeyValFloatTranslateToStringWithRound(float key, float val){
        return "{" + (Mathf.Round(key*100)/100).ToString().Replace(",", ".") + "f, " + (val).ToString().Replace(",", ".") + "f}, ";
    }
    string KeyValFloatTranslateToString(float key, float val){
        return "{" + key.ToString().Replace(",", ".") + "f, " + val.ToString().Replace(",", ".") + "f}, ";
    }
    string DecorteToK(float k, Dictionary<float, float> dict){

        var number = k.ToString().Replace(",", ".") + "f";
        var data = "/*k " + number + "*/                {" + number;
        data += ", new  Dictionary<float, float>{\n";
        data += MapToString(dict);
        data += "/*-k " + number + "*/                }},\n";
        data += "/*blank*/  \n";
        return data;
    }
    string DecorteToK(float k, string dict){
        /*dict должен содержать в конце \n*/
        var number = k.ToString().Replace(",", ".") + "f";
        var data = "/*k " + number + "*/                {" + number;
        data += ", new  Dictionary<float, float>{\n";
        data += "/*data*/                    " + dict;
        data += "/*-k " + number + "*/                }},\n";
        data += "/*blank*/  \n";
        return data;
    }
    string DecorateToMass(float mass, float k, Dictionary<float, float> dict){
        var number = mass.ToString().Replace(",", ".") + "f";
        var data = "/*mass " + number + "*/        {" + number;
        data += ", new Dictionary<float, Dictionary<float, float>>{\n";
        data += "/*new_k*/\n";
        data += DecorteToK(k, dict);
        data += "/*-mass " + number + "*/        }},\n";
        data += "/*blank*/\n";
        return data;
    }
    string DecorateToMass(float mass, float k, string dict){
        /*dict должен содержать в конце \n*/
        var number = mass.ToString().Replace(",", ".") + "f";
        var data = "/*mass " + number + "*/        {" + number;
        data += ", new Dictionary<float, Dictionary<float, float>>{\n";
        data += "/*new_k*/\n";
        data += DecorteToK(k, dict);
        data += "/*-mass " + number + "*/        }},\n";
        data += "/*blank*/\n";
        return data;
    }
    string CreateBeginingFile(string fileName){
        var data = "/*clone*/using System.Collections;\n";
        data += "/*clone*/using System.Collections.Generic;\n";
        data += "/*clone*/using UnityEngine;\n/*clone*/\n";
        data += "/*clone*/public class " + fileName + " : MonoBehaviour\n/*clone*/{\n";
        data += "/*clone*/    public Dictionary<float, Dictionary<float, Dictionary<float, float>>> muveMap =  new Dictionary<float, Dictionary<float, Dictionary<float, float>>>{\n";
        data += "/*new_mass*/\n";
        return data;
    }
    string CreateEndingFile(){
        var data = "/*blank*/ \n/*clone*/    };\n/*clone*/}";
        return data;
    }
    string CreateEmputyFile(string fileName){
        var data = CreateBeginingFile(fileName);
        data += CreateEndingFile();
        return data;
    }

}
