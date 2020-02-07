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

    private string DicaAgente = "n/s";
    private string CódigoDica = "n/s";
    private string EmoçãoDica = "n/s";
    private string EscolhaJogador = "n/s";
    private bool EraAdoAgente = false;
    private string EmoçãoClasse = "n/s";
    private string Codigo = "n/s";
    private string Frase = "n/s";
    private string cartasCombinaram = "n/s";

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
                    cabecalho = "Round;CondicaoDica;CondicaoAnim;condicaoAgente;Interacao;DicaAgente;CodigoDica;EmoçaoDica;EscolhaJogador;EraAdoAgente;Metch;EmocaoClasse;Codigo;Frase";
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

    public void saveEscolha(int escolha1, int escolha2, bool doAgente){
        this.EscolhaJogador = escolha1.ToString()+"|"+escolha2.ToString();
        this.EraAdoAgente = doAgente;
    }

    public void saveReacao(string msg, string cod, string emocao){
        this.EmoçãoClasse = emocao;
        this.Codigo = cod;
        this.Frase = msg;
    }

    public void saveMetch(bool mecth){
        this.cartasCombinaram = mecth.ToString();
    }

    public void registra(){
        /*
            Função responsavel por receber os dados e gravalos no arquivo de saida
        */
        using(var file = new StreamWriter(filePath + "/Log/individuo" + id + ".txt", true)){
                file.WriteLine(StaticValor.round+";"+StaticValor.condicao.ToString()+";"+StaticValor.condicaoAnim.ToString()+";"+
                                StaticValor.condicaoAgente.ToString()+";"+this.interacao+";"+
                                this.DicaAgente+";"+this.CódigoDica+";"+this.EmoçãoDica+";"+
                                this.EscolhaJogador+";"+this.EraAdoAgente.ToString()+";"+this.cartasCombinaram+";"+
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
                f.codigo = frase.codigo;
                f.emocao = frase.emocao;
                f.msg = frase.msg;
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
        int escolha = (int)Random.Range(0, 6);
        Frase f = new Frase();

        foreach (Frase item in frases){
            if(item.emocao.Equals(sentimento)){
                Debug.Log("Frase CTR: " + item.msg);
                if(escolha <= 0){
                    f.codigo = item.codigo;
                    f.emocao = item.emocao;
                    f.msg = item.msg;
                    if(sentimento.Equals("Dica")){
                        f.emocao = "Talk";
                    }
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
