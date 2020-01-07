using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;

public class MenuInicial : MonoBehaviour
{
    [SerializeField]
    InputField id = null;

    [SerializeField]
    Text vozes;

    void Start()
    {
        Speaker.Speak("ola mundo", null, Speaker.VoiceForName("Microsoft Daniel"));

        vozes.text = Application.persistentDataPath;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChamarMenu()
    {
        StaticValor.id = id.text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("menu");
    } 

    public void onChangeCondicao(bool valor){
        if(StaticValor.condicao == true){
            StaticValor.condicao = valor;
        }else{
            StaticValor.condicao = !valor;
        }
    }

    public void onChangeCondicaoAnim(bool valor){
        if(StaticValor.condicaoAnim == true){
            StaticValor.condicaoAnim = false;
        }else{
            StaticValor.condicaoAnim = true;
        }
    }

    public void onChangeCondicaoAgente(bool valor){
        if(StaticValor.condicaoAnim == true){
            StaticValor.condicaoAgente = false;
        }else{
            StaticValor.condicaoAgente = true;
        }
    }
}
