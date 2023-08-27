using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // usado para buscar a biblioteca de UI, no caso pra usar Slider
using UnityEngine.SceneManagement; // é utilizado quando tem controle de scena, no nosso caso iremos reiniciar o jogo quando aparecer a tela de game over
using TMPro;

// script ligado ao objeto canvas


public class ControlaInterface : MonoBehaviour
{

    private ControlaJogador scriptControlaJogador; // CRIANDO UMA VARIAVEL DO TIPO DA CLASSE QUE CRIAMOS CONTROLA JOGADOR
    public Slider SliderVidaJogador; //variavel do tipo slider para controlar a vida do jogador que aparece na tela, defini arrastando a slider no inspector do canvas de controla interface
    public GameObject PainelDeGameOver; // setamos o gameobject no inspector

    public GameObject Audio; // variavel setada para armazenar o objeto controla audio pra pausar o audio inicial
    public AudioClip SomGameOver; // variavel para armazenar o som de game over
    public TMP_Text TextoTempoDeSobrevivencia; // variaval para armazenar o tempo de sobrevivencia // setado no inspector
    public TMP_Text TextoPontuacaoMaxima; // texto do maior tempo que o jogador faz

    private float tempoPontuacaoSalva; //váriavel que vai guardar o tempo maximo de sobrevivencia

    private int quantidadeDeZumbisMortos; //variavel pra controlar a quantindade de zumbis mortos

    public Text TextoQuantidadeDeZumbisMortos; //vai receber o textozumbisMortos la no inspector

    // Start is called before the first frame updateS
    void Start()
    {
        scriptControlaJogador = GameObject.Find("Jogador").GetComponent<ControlaJogador>(); // SETANDO O SCRIPT CONTROLA JOGADOR NESTA VARIAVEL

        SliderVidaJogador.maxValue = scriptControlaJogador.statusJogador.Vida; //seta o valor maximo do slider como a vida do jogador
        AtualizarSliderVidaJogador(); //chama o metodo atualizar slider para atualizar a vida do jogador no maximo
        
        Time.timeScale = 1; // 0 pausa o jogo, 1 volta ao tempo normal, 2 o tempo fica 2x

        tempoPontuacaoSalva = PlayerPrefs.GetFloat("PontuacaoMaxima"); // buscando a pontuacao maxima que estava salva com o nome PontuacaoMaxima
    }

    public void AtualizarSliderVidaJogador()
    {
       SliderVidaJogador.value = scriptControlaJogador.statusJogador.Vida;
    }

    public void AtualizarQuantidadeDeZumbisMorotos () 
    {
        quantidadeDeZumbisMortos ++; //o ++ soma 1 na quantidade de Zumbis Mortos
        TextoQuantidadeDeZumbisMortos.text = string.Format("x {0}",quantidadeDeZumbisMortos); //muda o texto pra quantidade de zumbis mortos
    }

    public void GameOver()
    {
        PainelDeGameOver.SetActive(true); // assim colocamos o objeto do painel como ativo
        Time.timeScale = 0; // 0 pausa o jogo, 1 volta ao tempo normal, 2 o tempo fica 2x

        int minutos = (int)(Time.timeSinceLevelLoad / 60); //timesincelevelload passa o tempo desde que a faze começou, divide por 60 pq tá em segundos, (int) descarta as partes depois da virgula, pega só o inteiro
        int segundos = (int)(Time.timeSinceLevelLoad % 60); // % pega o resto da divisão "pega o quebrado do numero interio"
        TextoTempoDeSobrevivencia.text = "Voce sobreviveu por " + minutos + "min e " + segundos + "s"; //muda o texto

        AjustarPontuacaoMaxima(minutos, segundos);

        Audio.GetComponent<ControlaAudio>().StopAudio(); // pausa o audio inicial
        ControlaAudio.instancia.PlayOneShot(SomGameOver); //PlayoneShot, é pra tocar uma vez o som de gameover
    }
    public void AjustarPontuacaoMaxima(int min, int seg)
    {
        if (Time.timeSinceLevelLoad > tempoPontuacaoSalva)
        {
            tempoPontuacaoSalva = Time.timeSinceLevelLoad;
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg); //string.format é uma forma mais organizada de concatenar texto

            //salva as preferencias do jogador em sua maquina // playerpref funciona pra todas as plataformas, web, mobile etc
            PlayerPrefs.SetFloat("PontuacaoMaxima", tempoPontuacaoSalva); // salva o tempo pontuacao maxima com o nome PontuacaoMaxima
        }

        if(TextoPontuacaoMaxima.text == "")
        {
            min = (int)(tempoPontuacaoSalva / 60); //timesincelevelload passa o tempo desde que a faze começou, divide por 60 pq tá em segundos, (int) descarta as partes depois da virgula, pega só o inteiro
            seg = (int)(tempoPontuacaoSalva % 60); // % pega o resto da divisão "pega o quebrado do numero
            TextoPontuacaoMaxima.text = string.Format("Seu melhor tempo é {0}min e {1}s", min, seg); //string.format é uma forma mais organizada de concatenar texto      
        }
    }        

    public void Reiniciar()
    {
        SceneManager.LoadScene("game"); //da load na cena do jogo "game"
    }

}
