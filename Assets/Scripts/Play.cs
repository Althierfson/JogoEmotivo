using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Play : MonoBehaviour
{
    
    [SerializeField]
    InputField nome;

    void Start()
    {
        StaticValor.arquivos = new Arquivos(StaticValor.id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void ChamarJogoHum(){
        StaticValor.nomeJogador = nome.text;
        UnityEngine.SceneManagement.SceneManager.LoadScene("jogo1");
    } 

}
