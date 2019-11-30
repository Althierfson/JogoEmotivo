using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuInicial : MonoBehaviour
{
    [SerializeField]
    InputField id = null;

    void Start()
    {

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
            Debug.Log("Setado para false:" + StaticValor.condicao.ToString());
        }else{
            StaticValor.condicao = !valor;
            Debug.Log("Setado para true:" + StaticValor.condicao.ToString());
        }
    }
}
