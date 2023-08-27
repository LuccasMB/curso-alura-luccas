using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.SceneManagement; // é utilizado quando tem controle de scena, no nosso caso iremos reiniciar o jogo quando aparecer a tela de game over

public class ControlaJogador : MonoBehaviour, IMatavel, ICuravel
{

    //public float Velocidade = 10; // variavel pra controlar velocidade do jogador //com o script Status, este atributo será tratado lá
    private Vector3 direcao; // vector3 para calcular direcao que o jogador vai andar
    //public LayerMask MascaraChao; // mascara chao foi criado uma mascara de apenas o gameobject do chao para o raycast identificar apenas o chao, como se fosse uma tag só que layer
    public GameObject TextoGameOver; //cria o gameobject que receberá o gameobject de texto do game over
    // comentamos a variavel vivo pois quando criamos a vida, não precisamos mais dela
    //public bool Vivo = true; // variavel booleana para indicar se o jogador está vivo ou morto 
    //private Rigidbody rigidbodyJogador; // variavel rigidbody pra não precisar ficar escrevendo getcomponent toda hr //com o script movimentoJogador, não precisa mais de declarar rigidbody
    //private Animator animatorJogador; // variavel animator pra nao precisar ficar escrevendo getcomponent toda hr //com o script AnimacaoPersonagem, não precisa mais de declarar Animator
    public ControlaInterface scriptControlaInterface; // CRIANDO UMA VARIAVEL DO TIPO DA CLASSE QUE CRIAMOS CONTROLA INTERFACE, COMO ELA É PUBLICA ARRASTAMOS O OBJETO CANVAS LA NO INSPETOR NO JOGADOR PRA DEFINIR A VARIAVEL

   // public int Vida = 100; //cria uma variavel para controlar a vida do jogador, do tipo inteiro foi só opção e setamos como 100 //com o script Status, este atributo será tratado lá
    
    
    public AudioClip SomDeDano; // variavel para armazenar o som de dano
    
    private MovimentoJogador meuMovimentoJogador;  // criando uma variavel da classe criada no script movimento Jogador pra poder usar os métodos criados nela
    private AnimacaoPersonagem animacaoJogador;  // criando uma variavel da classe criada no script animação personagem pra poder usar os métodos criados nela
    public Status statusJogador; // criando uma variavel (publica pois é acessado a vida no ControlaInterface) da classe criada no script status pra poder usar os métodos criados nela

    private void Start()
    {
        //rigidbodyJogador = GetComponent<Rigidbody>(); // definindo a variavel rigidbody pra nao ter que ficar reescrevendo //com o script movimentoJogador, não precisa mais de declarar rigidbody
        //animatorJogador = GetComponent<Animator>(); // definindo a variavel animator pra nao ter que ficar reescrevendo //com o script AnimacaoPersonagem, não precisa mais de declarar Animator
        meuMovimentoJogador = GetComponent<MovimentoJogador>(); // definindo a variavel Movimento Jogador pra nao ter que ficar reescrevendo
        animacaoJogador = GetComponent<AnimacaoPersonagem>(); // definindo a variavel Animaçao Personagem pra nao ter que ficar reescrevendo
        statusJogador = GetComponent<Status>(); // definindo a variavel status pra nao ter que ficar reescrevendo
    }

    // Update is called once per frame
    void Update() // usamos update quando queremos tratar dados, por isso os inputs que são usados no rigidbody estão sendo definidos aqui
    {
        //eixoY é 0 no sistema da unity, pois nosso boneco nao voa
        float eixoX = Input.GetAxis("Horizontal"); // salva em eixoX se for apertado as setas direita esquerda ou a e d
        float eixoZ = Input.GetAxis("Vertical"); // salva em eixoZ se for apertado as setas pra cima ou pra baixo ou w e z

        direcao = new Vector3(eixoX, 0, eixoZ); // quando vamos iniciar um vector tres colocamos o new, pois o vector 3 vai mudar a cada update

        //testamos se a direcao é diferente de 0, se sim, muda a animação para se mover, caso o contrario deixa ele parado
        //if (direcao != Vector3.zero)
        //{
        //animatorJogador.SetBool("Movendo", true); // seta o parametro movendo que criamos na animacao do jogador como verdadeira
        // }
        //else
        //{
        //    animatorJogador.SetBool("Movendo", false); // seta o parametro movendo que criamos na animacao do jogador como falsa
        //}

        //esta linha de codigo vai substituir as linhas acima para controlar a animação para se mover e parar
        animacaoJogador.Movimentar(direcao.magnitude); // magnitude é o módulo do vector3 direcao, só será 0 quando o vetor for (0,0,0) 
     
        
        //todas essas linhas foram inativas pois o reinicio do jogo sera controlado pela ControlaInterface
        // testamos se o jogador está vivo, se ele não estiver esperamos o jogador clicar com o botao de atirar para resetar o jogo
        //if(statusJogador.Vida <= 0)
        // {
        //    if(Input.GetButtonDown("Fire1")) //ve se o botao atirado foi pressionado
        //    {
        //         SceneManager.LoadScene("game"); //da load na cena do jogo "game"
        //    }
        //}
    }

    void FixedUpdate() // FixedUpdate usado sempre que trabalhamos com rigidbody 
    {
        //rigidbodyJogador.MovePosition(rigidbodyJogador.position + (direcao * Velocidade * Time.deltaTime)); // função de mover o jogador para a direção setada nos inputs com a velocidade definida
        meuMovimentoJogador.Movimentar(direcao, statusJogador.Velocidade); //Chamando o método movimentar da classe movimento personagem, enviando direção e velocidade // substitui a linha de cima
        meuMovimentoJogador.RotacaoJogador(); // Mascarachao é um gameobject que foi definido através da layer chão que contempla tudo que foi marcado no cenario na layer chao //mascara chao nao usa mais
    }

    public void TomarDano(int dano) // void só executa tudo que está dentro e não faz mais nada, publico para ser acessado por outros scripts
    {
        statusJogador.Vida -= dano; // mesma coisa de Vida = Vida - 30

        scriptControlaInterface.AtualizarSliderVidaJogador(); // ativa o método para atualizar a interface com o slider da vida do jogador
        
        ControlaAudio.instancia.PlayOneShot(SomDeDano); //PlayoneShot, é pra tocar uma vez o som de dano
        // a gente consegue utilizar a instancia aqui, sem definir ela de novo, por causa do static lá no controla audio, ela pega o valor que está definido lá

        if(statusJogador.Vida <= 0) // testa se a vida já chegou a 0
        {
            Morrer(); // chama o método morrer
        }

    }

    public void Morrer() //executa as ações quando o jogador morre
    {
        //TextoGameOver.SetActive(true); // ativa o texto de gameover substituido pelo Painel de game over
        scriptControlaInterface.GameOver(); //chama o metodo game over do script controla interface
    }
    public void CurarVida(int quantidadeDeCura) // recuperar vida do jogador
    {
        statusJogador.Vida += quantidadeDeCura; // soma na vida atual a quantidade de cura
        if(statusJogador.Vida > statusJogador.VidaInicial) // testa se a vida nova do jogador passou a vida inicial dele
        {
            statusJogador.Vida = statusJogador.VidaInicial; //se passou, seta a vida atual como vida inicial 
        }
        scriptControlaInterface.AtualizarSliderVidaJogador(); // ativa o método para atualizar a interface com o slider da vida do jogador
    }

}
