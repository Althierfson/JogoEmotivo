using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class Arquivos{

    public List<Frase> frases;
    string id;
    public string filePath = Application.persistentDataPath;
    private int interacao;

    // Informações de gravação

    private string DicaAgente;
    private string CódigoDica;
    private string EmoçãoDica;
    private string EscolhaJogador;
    private bool EraAdoAgente;
    private string EmoçãoClasse;
    private string Codigo;
    private string Frase;

    public Arquivos(string id){
        frases = new List<Frase>();
        this.id = id;
        criarArquivo();
        lerFrases();
    }

    public void criarArquivo(){ 
        /*
            Criar o arquivo de saida para o individo
        */
        string cabecalho;
        this.interacao = 1;
        Debug.Log(filePath);
        // C:/Users/007br/Documents/TCC/UnityEmotionalAgent/EmotionalAgent/Assets/Scripts
        if(!System.IO.File.Exists(filePath + "/Log/individuo" + id + "" + ".txt")){
            using(StreamWriter file = File.CreateText(filePath + "/Log/individuo" + id + ".txt")){
                    cabecalho = "Round;Condição;Interacao;DicaAgente;CodigoDica;EmoçaoDica;EscolhaJogador;EraAdoAgente;EmocaoClasse;Codigo;Frase";
                    file.WriteLine(cabecalho);
                }
        }
        return;
    }

    public void lerFrases(){
        /*
            Função responsavel por ler o arquivo de frases e adicionar na lista frases
        */
        string line;
        string[] vetorline;

         
        System.IO.StreamReader file = 
            new System.IO.StreamReader(filePath + "/falas.txt");
        

        while( (line = file.ReadLine()) != null){ // Ler linha por linha do arquivo de entrada falas.txt
            Frase novo = new Frase();

            vetorline = line.Split(';');
            novo.codigo = vetorline[0];
            novo.emocao = vetorline[1];
            novo.msg = vetorline[2];
            frases.Add(novo);
        }

        file.Close();
    }

    public void saveDica(string msg, string cod, string emocao){
        this.DicaAgente = msg;
        this.CódigoDica = cod;
        this.EmoçãoDica = emocao;
    }

    public void saveEscolha(string escolha, bool doAgente){
        this.EscolhaJogador = escolha;
        this.EraAdoAgente = doAgente;
    }

    public void saveReacao(string msg, string cod, string emocao){
        this.EmoçãoClasse = emocao;
        this.Codigo = cod;
        this.Frase = msg;
    }

    public void registra(){
        /*
            Função responsavel por receber os dados e gravalos no arquivo de saida
        */
        using(var file = new StreamWriter(filePath + "/Log/individuo" + id + ".txt", true)){
                file.WriteLine(StaticValor.round+";"+StaticValor.condicao.ToString()+";"+this.interacao+";"+
                                this.DicaAgente+";"+this.CódigoDica+";"+this.EmoçãoDica+";"+
                                this.EscolhaJogador+";"+this.EraAdoAgente.ToString()+";"+
                                this.EmoçãoClasse+";"+this.Codigo+";"+this.Frase+";");
            }
        interacao++;
    }

    public Frase getFrases(string cod){
        /*
            é informado o codigo, e a função retorna o objeto, caso não ache retorna um valor nulo
        */
        Frase f = new Frase();

        foreach(Frase frase in frases){
            if(frase.codigo.Equals(cod)){
                f = frase;
                Debug.Log("arquivo CTR: "+f.msg);
                return f;
            }
        }

        return null;
    }

    public Frase pickUpEmocao(string sentimento){
        /*
            Função responsevel por retorna uma frase aleatoria com o sentimento recebido
            ToDo: reoganizar a forma de buscar frases aleatorias
        */

        //Random numAleatorio = new Random();
        int escolha = (int)Random.Range(0, 2);
        Frase f = new Frase();

        foreach (Frase item in frases){
            if(item.emocao.Equals(sentimento)){
                if(escolha <= 0){
                    f = item;
                    return f;
                }else{
                    escolha--;
                }
            }
        }
        
        return null;
    }
}

public class Frase{
    public string codigo;    // Guarda o identificador da frase
    public string emocao;    // Guarda o amoção que será exibida
    public string msg;       // Garda a menssagem ou frase
}
