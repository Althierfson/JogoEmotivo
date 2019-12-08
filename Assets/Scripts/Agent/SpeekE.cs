using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.UI;
using Crosstales.RTVoice;
using Crosstales.RTVoice.Model;


public class SpeekE : MonoBehaviour{
    
    public AudioSource som;            // Audio da Voz
    private Animator anim;             // Controle da animação
    public GameObject ballonImage;     // Imagem do balão
    public Text textBallon;            // Texto no balão
    private string animState;          // Guarda a animação atual
    Frase estadoAtual = new Frase();   // Objeto estado

    public int[] tabuleiro;            // Tabuleiro atual em jogo
    public int dica1, dica2;           // As dicas do agente
    public bool agentInWork = false;   // Se o agente esta falando
    private bool emReacao;
    private bool fimloop;
    private bool endGame;
    private bool podeFinalizar;
    private int escohasCorretas;

    [SerializeField]
    private bool talkControle = false; // Controle das partes intrudução, rounds, e finalização
    [SerializeField]
    private Text AgenteFrase;          // frase a serdita
    [SerializeField]
    private GameObject panelConverca;  // Panel que sobre poem o jogo
    [SerializeField]
    private Button startGame;          // Botao para avança para o jogo

    public void Start(){
        anim = GetComponent<Animator>();   // Pegando a Animator para controle das animações
        ballonImage.SetActive(false);      // esconde a imagem do balão
        som = GetComponent<AudioSource>();

        if(!StaticValor.condicaoAnim){
            anim.enabled = false;
        }
    }

    public void configuraçaoDeRounds(){
        if(StaticValor.round == 1){
            // primeira partida
            intruducao();
        }else{
            panelConverca.SetActive(false);
            this.gerarDica();
        }
    }

    public void Speek(){
        // Quando essa função é chamada o TextToSpeech falará a frase contidade em estadoAtual.msg

        if(StaticValor.condicaoAgente){
            //Speaker.Speak(text.text, null, Speaker.VoiceForName("Microsoft Daniel"));
            Speaker.Speak(estadoAtual.msg, this.som, Speaker.VoiceForName("Microsoft Daniel"));
            //Speaker.SpeakNative(estadoAtual.msg);
        }
    }

    void Update(){
        
        // Controle da animação
        animCtr();  // Ativando balão de animação

        controleloop();

        controleDone();
    }

    public void setPassivo(){
        if(StaticValor.condicaoAnim){
            anim.SetBool(animState, false);
            anim.SetBool("Passivo_1", true);
        }
    }

    public void startAnimation(){
        if(StaticValor.condicaoAnim){
            anim.SetBool(animState, true);
            anim.SetBool("Passivo_1", false);
        }
    }

    private void animCtr(){
        /*
            Responsavel por ativar o balão com a menssagem do Agente.
        */

        if(this.som.isPlaying){
            //ballonImage.SetActive(true);
            this.agentInWork = true;
        }
        if(!this.som.isPlaying){
            //ballonImage.SetActive(false);
            this.agentInWork = false;
        }
    }

    void controleDone(){
        if(this.som.isPlaying && this.endGame){
            this.podeFinalizar = true;
        }
        if(!this.som.isPlaying && this.podeFinalizar){
            this.podeFinalizar = false;
            this.endGame = false;
            ruond();
        }
    }


    void controleloop(){

        if(!this.endGame){
            if(this.emReacao){
                if(this.som.isPlaying){
                    this.fimloop = true;
                }
                if(!this.som.isPlaying && this.fimloop){
                    this.emReacao = this.fimloop = false;
                    this.gerarDica();
                }
            }
        }
    }

    private void setConf(){
        /*
            Essa função é responsavel por receber um objeto frase e setar a frase no balão, na voz do robo e definir a animação
        */
        
        textBallon.text = estadoAtual.msg;
        animState = estadoAtual.emocao;
        
    }

    public void reacao(bool rec, int escolhas){
        /*
            Função receber um sentimento e escolhe aleatoriamente uma frase no banco de frases com o sentimento correspondente
        */
        //this.escohasCorretas = escolhas;
        Debug.Log("speek - reacao: " + this.escohasCorretas);
        if(escolhas <= 4){
            this.emReacao = true;
            if(StaticValor.condicaoAgente){
                this.ballonImage.SetActive(true);
            }
            if (rec == true){
                estadoAtual = StaticValor.arquivos.pickUpEmocao("Alegre");
            }else{

                /*
                int sentimento = (int)Random.Range(1, 3);

                if(sentimento == 1){
                    estadoAtual = StaticValor.arquivos.pickUpEmocao("Triste");
                }else if(sentimento == 2){
                    estadoAtual = StaticValor.arquivos.pickUpEmocao("Bravo");
                }else if(sentimento == 3){
                    estadoAtual = StaticValor.arquivos.pickUpEmocao("Vergonha");
                }else{
                    estadoAtual = StaticValor.arquivos.pickUpEmocao("Triste");
                }*/

                estadoAtual = StaticValor.arquivos.pickUpEmocao("Triste");
            }
            if(estadoAtual != null){
                StaticValor.arquivos.saveReacao(estadoAtual.msg, estadoAtual.codigo, estadoAtual.emocao);
                StaticValor.arquivos.registra();
                setConf();

                if(StaticValor.condicaoAnim){
                    startAnimation();
                }else{
                    this.Speek();
                }
            }
        }else{
            StaticValor.arquivos.saveReacao("n/s", "n/s", "n/s");
            StaticValor.arquivos.registra();
        }
    }

    public void setEscolhasCorretas(int valor){
        this.escohasCorretas = valor;
    }

    public void gerarDica(){
        exibir();
        Debug.Log("speek - geraDica: " + this.escohasCorretas);
        if(this.escohasCorretas <= 4){
            if(StaticValor.condicaoAgente){
                this.ballonImage.SetActive(true);
            }
            if(StaticValor.condicao == true){
                gerarDicaCorreta();
            }else{
                gerarDicaFalsa();
            }
        }else{
            StaticValor.arquivos.saveDica("n/s", "n/s", "n/s");
        }
    }

    public void gerarDicaCorreta(){
        int dica1;
        this.exibir();
        do{
            dica1 = (int)Random.Range(0, this.tabuleiro.Length);
        }while(this.tabuleiro[dica1] == -1);

        for(int i=0;i<tabuleiro.Length;i++){
            if(this.tabuleiro[dica1] == this.tabuleiro[i] && dica1 != i){
                this.dica2 = i+1;
                this.dica1 = dica1+1;
                Debug.Log(dica1+":"+i);
                falarDica();
                return;
            }
        }
    }

    public void gerarDicaFalsa(){
        int escolha1, escolha2;
        do{
            escolha2 = (int)Random.Range(0, tabuleiro.Length);
            escolha1 = (int)Random.Range(0, tabuleiro.Length);
        }while(this.tabuleiro[escolha1] == this.tabuleiro[escolha2] || this.tabuleiro[escolha1] == -1 || this.tabuleiro[escolha2] == -1 || escolha1 == escolha2);

        this.dica1 = escolha1+1;
        this.dica2 = escolha2+1;
        falarDica();
    }

    private void falarDica(){

        estadoAtual = StaticValor.arquivos.pickUpEmocao("Dica");
        if(estadoAtual == null){
            estadoAtual = StaticValor.arquivos.getFrases("3");
        }
        estadoAtual.msg = estadoAtual.msg.Replace("##", this.dica1.ToString());
        estadoAtual.msg = estadoAtual.msg.Replace("$$", this.dica2.ToString());

        StaticValor.arquivos.saveDica(estadoAtual.msg, estadoAtual.codigo, estadoAtual.emocao);
        setConf();
        Speek();
        //startAnimation();
    }

    public void intruducao(){

        if(StaticValor.condicaoAgente){
            this.estadoAtual = StaticValor.arquivos.getFrases("13");
        }else{
            this.estadoAtual = StaticValor.arquivos.getFrases("17");
        }

        this.AgenteFrase.text = estadoAtual.msg;
        Speek();
        this.talkControle = true;
    }

    public void exibir(){
        for(int j=0;j<this.tabuleiro.Length;j++){
            Debug.Log("Tabuleiro: " + j + " n: " + this.tabuleiro[j]);
        }
    }

    public void setEndGame(bool valor){
        this.endGame = valor;
    }

    public void ruond(){
        this.ballonImage.SetActive(false);
        switch(StaticValor.round){
            case 2:
                this.panelConverca.SetActive(true);
                this.startGame.GetComponentInChildren<Text>().text = "Vamos lá!";
                if(StaticValor.condicaoAgente){
                    this.estadoAtual = StaticValor.arquivos.getFrases("14");
                }else{
                    this.estadoAtual = StaticValor.arquivos.getFrases("18");
                }
                this.AgenteFrase.text = estadoAtual.msg;
                Speek();
                this.talkControle = true;
                break;
            case 3:
                this.panelConverca.SetActive(true);
                this.startGame.GetComponentInChildren<Text>().text = "Vamos começa!";
                if(StaticValor.condicaoAgente){
                    this.estadoAtual = StaticValor.arquivos.getFrases("15");
                }else{
                    this.estadoAtual = StaticValor.arquivos.getFrases("19");
                }
                this.AgenteFrase.text = estadoAtual.msg;
                Speek();
                this.talkControle = true;
                break;
            default:
                this.panelConverca.SetActive(true);
                this.startGame.GetComponentInChildren<Text>().text = "Ate mais!";
                if(StaticValor.condicaoAgente){
                    this.estadoAtual = StaticValor.arquivos.getFrases("16");
                }else{
                    this.estadoAtual = StaticValor.arquivos.getFrases("20");
                }
                this.AgenteFrase.text = estadoAtual.msg;
                Speek();
                this.talkControle = true;
                break;
        }
    }

    public void startGameButtom(){
        if(!this.agentInWork){
            this.panelConverca.SetActive(false);
            this.talkControle = false;
            if(StaticValor.round == 1){
                this.gerarDica();
            }
        }
    }
}