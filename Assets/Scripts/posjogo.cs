using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class posjogo : MonoBehaviour {
    
    [SerializeField]
    private InputField senha;

    void Start(){}

    void Update(){}

    public void entrar(){
        
        if(senha.text == "jogo15"){
            UnityEngine.SceneManagement.SceneManager.LoadScene("inicial");
        }
    }
}
