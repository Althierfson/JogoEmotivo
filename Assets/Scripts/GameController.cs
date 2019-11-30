using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Threading;
 using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour {

    [SerializeField]
    private Sprite bgImage;

    public Sprite[] puzzles;

    public List<Sprite> gamePuzzles = new List<Sprite>();

    public List<Button> btns = new List<Button>();

    public List<Text> texts = new List<Text>();

    private bool firstGuess, secondGuess;
    private int countGuesses;
    private int countCorrectGuesses;
    private int gameGuesses;

    private int firstGuessIndex, secondGuessIndex;

    private string firstGuessPuzzle, secondGuessPuzzle;

    //private int[] tabuleiro;
    private bool openToClick = true;

    [SerializeField]
    private SpeekE speekE;

    private void Awake()
    {
        puzzles = Resources.LoadAll<Sprite>("Sprites/Candy");
    }


    private void Start()
    {
        Debug.Log("Round Atual: " + StaticValor.round);
        StaticValor.arquivos = new Arquivos(StaticValor.id);
        speekE.Start();
        GetButtons();
        AddListeneers();
        AddGamePuzzles();
        Shuffle(gamePuzzles);
        gameGuesses = gamePuzzles.Count / 2;
        //speekE.gerarDica();
    }

        void GetButtons()
    {
            GameObject[] objects = GameObject.FindGameObjectsWithTag("PuzzleButton");

            for (int i = 0; i < objects.Length; i++)
            {
                btns.Add(objects[i].GetComponent<Button>());
                btns[i].image.sprite = bgImage;
                texts.Add(objects[i].GetComponentInChildren<Text>());
                texts[i].text = (i+1).ToString();
            }
    }

    void AddGamePuzzles()
    {
        /*
            Essa é a função responsavel por gerar o tabuleiros com os pares
        */
        int looper = btns.Count;
        int index = 0;

        speekE.tabuleiro = new int[looper];
        
        for (int i=0; i< looper; i++)
        {
            if (index == looper /2)
            {
                index = 0;
            }
            gamePuzzles.Add(puzzles[index]);
            speekE.tabuleiro[i] = index;

            index++;
        }
    }

    void AddListeneers()
    {
        foreach (Button btn in btns)
        {
            btn.onClick.AddListener(() => PickAPuzzle());
        }
    }

    public void PickAPuzzle()
    {
        if(!speekE.agentInWork){
            if (!firstGuess)
            {
                firstGuess = true;
                firstGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

                firstGuessPuzzle = gamePuzzles[firstGuessIndex].name;

                btns[firstGuessIndex].image.sprite = gamePuzzles[firstGuessIndex];

                texts[firstGuessIndex].enabled = false;

            } else if (!secondGuess)
            {
                secondGuess = true;
                secondGuessIndex = int.Parse(UnityEngine.EventSystems.EventSystem.current.currentSelectedGameObject.name);

                secondGuessPuzzle = gamePuzzles[secondGuessIndex].name;

                btns[secondGuessIndex].image.sprite = gamePuzzles[secondGuessIndex];

                texts[secondGuessIndex].enabled = false;

                if(firstGuessPuzzle == secondGuessPuzzle){
                    Debug.Log("Cartas corretas");
                    speekE.tabuleiro[firstGuessIndex] = -1;
                    speekE.tabuleiro[secondGuessIndex] = -1;
                    trataInteracao();
                } else {
                    Debug.Log("Tente outra vez");
                    trataInteracao();
                }

                countGuesses++;

                StartCoroutine(CheckIfThePuzzlesMatch());

            }
        }
    }
    IEnumerator CheckIfThePuzzlesMatch()
    {
        yield return new WaitForSeconds(1f);
        if (firstGuessPuzzle == secondGuessPuzzle)
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].interactable = false;
            btns[secondGuessIndex].interactable = false;

            btns[firstGuessIndex].image.color = new Color(1, 1, 1, 1);
            btns[secondGuessIndex].image.color = new Color(1, 1, 1, 1);

            CheckIfTheGameIsFinished();

        } else
        {
            yield return new WaitForSeconds(.5f);

            btns[firstGuessIndex].image.sprite = bgImage;
            btns[secondGuessIndex].image.sprite = bgImage;

            texts[firstGuessIndex].enabled = true;
            texts[secondGuessIndex].enabled = true;
        }

        yield return new WaitForSeconds(.5f);

        firstGuess = secondGuess = false;
        this.speekE.exibir();
    }

    void CheckIfTheGameIsFinished()
    {
        countCorrectGuesses++;

        if (countCorrectGuesses == gameGuesses)
        {
            Debug.Log("Fim do Jogo");
            Debug.Log("Foram " + countGuesses + " tentativas para vencer");
            StartCoroutine(waitFor(10));

            StaticValor.round++;
            this.speekE.ruond();
            Debug.Log("Up Rund: " + StaticValor.round);

        }
    }

    public void resetarCena(){
        if(StaticValor.round > 1 && StaticValor.round <= 3){
            SceneManager.LoadScene("jogo1");
        }else if(StaticValor.round > 3){
            UnityEngine.SceneManagement.SceneManager.LoadScene("posJogo");
        }
    }

    IEnumerator waitFor(float sec){
        yield return new WaitForSeconds(sec);
    }

    void Shuffle(List<Sprite> list) // funcao randomica
    {
        for (int i=0; i < list.Count; i++)
       {
            Sprite temp = list[i];
            int inttemp = speekE.tabuleiro[i];
            int randomIndex = Random.Range(i, list.Count);
            list[i] = list[randomIndex];
            speekE.tabuleiro[i] = speekE.tabuleiro[randomIndex];
            list[randomIndex] = temp;
            speekE.tabuleiro[randomIndex] = inttemp;
        }
    }

    void trataInteracao(){
        // Função responcavel por trata o acerto de cartas

        if(firstGuessIndex+1 == speekE.dica1 || firstGuessIndex+1 == speekE.dica2){
            if(secondGuessIndex+1 == speekE.dica1 || secondGuessIndex+1 == speekE.dica2){
                blockInteracao();
                StaticValor.arquivos.saveEscolha(firstGuessIndex.ToString()+"|"+secondGuessIndex.ToString(), true);
                speekE.reacao(true);
                openInteracao();
                Debug.Log("GameController: Usuario escolheo as cartas do Agente");
            }else{
                blockInteracao();
                StaticValor.arquivos.saveEscolha(firstGuessIndex.ToString()+"|"+secondGuessIndex.ToString(), false);
                speekE.reacao(false);
                openInteracao();
                Debug.Log("GameController: Usuario não escolheo as cartas do Agente");
            }
        }else{
            StaticValor.arquivos.saveEscolha(firstGuessIndex.ToString()+"|"+secondGuessIndex.ToString(), false);
            speekE.reacao(false);
            Debug.Log("GameController: Usuario não escolheo as cartas do Agente");
        }
    }

    public void blockInteracao(){
        openToClick = false;
    }

    public void openInteracao(){
        openToClick = true;
    }

} // GameController
