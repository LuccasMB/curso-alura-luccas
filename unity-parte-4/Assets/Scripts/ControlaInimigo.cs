using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using UnityEngine;

public class ControlaInimigo : MonoBehaviour, IMatavel
{
    public GameObject Jogador; //cria uma variavel pra receber o gameobject do jogador
    // public float Velocidade = 5; // seta a velocidade do zumbi //com o script Status, este atributo será tratado lá
    // private Rigidbody rigidbodyInimigo; // variavel rigidbody pra não precisar ficar escrevendo getcomponent toda hr // parou de ser necessario por causa da classe movimentopersonagem
    // private Animator animatorInimigo; // variavel animator pra nao precisar ficar escrevendo getcomponent todda hr // parou de ser necessario por causa da classe AnimacaoPersonagem
    private int dano; //variavel para passar o dano que o zumbi dará ao jogador
    private MovimentoPersonagem movimentoInimigo; // criando uma variavel da classe criada no script movimento personagem pra poder usar os métodos criados nela
    private AnimacaoPersonagem animacaoInimigo;  // criando uma variavel da classe criada no script animacao personagem pra poder usar os métodos criados nela
    private Status statusInimigo; // criando uma variavel da classe criada no script status pra poder usar os métodos criados nela
    public AudioClip SomDeMorte; // variavel para armazenar o som de morte do zumbi
    private Vector3 posicaoAleatoria; // variavel para auxiliar o zumbi a ficar vagando
    private Vector3 direcao; // variavel para indicar a direcao que o zumbi vai andar
    private float contadorVagar; // contador para contar o tempo que o zumbi esta vagando
    private float tempoEntrePosicoesAleatorias = 4; // vai demorar 4 segundos para que mude a posicao aleatoria
    private float porcentagemGerarKitMedico = 0.1f; // porcentagem de gerar de 10% //tem que colocar o f para o código saber que o numero quebrado é float
    public GameObject KitMedicoPrefab; //variavel para receber o prefab do kitmedico no inspector
    private ControlaInterface scriptControlaInterface; // CRIANDO UMA VARIAVEL DO TIPO DA CLASSE QUE CRIAMOS CONTROLA INTERFACE
    [HideInInspector]
    public bool ZumbiNaoTomarTiro; // variavel criada com o intuito de fazer que o zumbi não continue tendo colisoes entre bala e zumbi durante a animação de morte do zumbi
    [HideInInspector]
    public GeradorZumbis meuGerador; //// CRIANDO UMA VARIAVEL DO TIPO DA CLASSE QUE CRIAMOS gerador zumbis // não precisa aparecer no inspector
    public GameObject ParticulaSangueZumbi; //variavel pra receber o gameobject da particula sangue do zumbi no inspector no prefab do zumbi


	// Use this for initialization
	void Start () {
        Jogador = GameObject.FindWithTag(Tags.Jogador); // busca o gameobject que tem a tag Jogador e armazena na variavel Jogador
        AleatorizarZumbi ();
        // rigidbodyInimigo = GetComponent<Rigidbody>(); // definindo a variavel rigidbody pra nao ter que ficar reescrevendo // parou de ser necessario por causa da classe movimentopersonagem
        // animatorInimigo = GetComponent<Animator>(); // definindo a variavel animator pra nao ter que ficar reescrevendo // parou de ser necessario por causa da classe AnimacaoPersonagem
        movimentoInimigo = GetComponent<MovimentoPersonagem>(); //definindo a classe na variavel criada para poder usar os métodos dela
        animacaoInimigo = GetComponent<AnimacaoPersonagem>(); //definindo a classe na variavel criada para poder usar os métodos dela
        statusInimigo = GetComponent<Status>(); //definindo a classe na variavel criada para poder usar os métodos dela
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface; // uma variante pra usar o find sem ser via tag // a gente fala o tipo e depois diz como criaremos o objeto
        ZumbiNaoTomarTiro = false; // quando o zumbi é iniciado nós marcamos a variavel como falsa para que o zumbi possa tomar tiro, que aconteça colisao entre a bala e o zumbi
    }

    void FixedUpdate() // se usa FixedUpdate sempre que for trabalhar com rigidbody
    {
        // criar variavel para calcular distancia entre o zumbi e o jogador
        float distancia = Vector3.Distance(transform.position, Jogador.transform.position);
        if(direcao != Vector3.zero)
        {
            movimentoInimigo.Rotacionar(direcao); // chama o metodo rotacionar da classe movimento personagem, enviando a direção
        }
        

        animacaoInimigo.Movimentar(direcao.magnitude); //preenchendo o valor do parametro Movendo da animação do zumbi para ele andar ou parar

        if (distancia >15)
        {
            //se a distancia for maior que 15 vamos fazer o zumbi vagar
            Vagar(); 
        }
        //quando a distancia entre o jogador e o zumbi for maior que 2,5 mandamos o zumbi andar em direção ao jogador
        else if (distancia > 2.5)
        {
            Perseguir ();
        }
        else
        {
            direcao = Jogador.transform.position - transform.position; //isso é para o zumbi rotacionar enquanto estiver atacando caso o jogador se mova
            // animatorInimigo.SetBool("Atacando", true); //seta o parametro do evento de atacarinimigo na animação de ataque como verdadeiro // parou de ser necessario por causa da classe AnimacaoPersonagem
            animacaoInimigo.Atacar(true);
        }
    }

    void Perseguir ()
    {
        //faz a diferença entre o jogador e o zumbi
        direcao = Jogador.transform.position - transform.position;

        movimentoInimigo.Movimentar(direcao, statusInimigo.Velocidade); //Chamando o método movimentar da classe movimento personagem, enviando direção e velocidade

        // animatorInimigo.SetBool("Atacando", false); //seta o parametro do evento de atacarinimigo na animação de ataque como falso // parou de ser necessario por causa da classe AnimacaoPersonagem
        animacaoInimigo.Atacar(false);
    }

    void Vagar() //método pro zumbi ficar vagando
    {
        contadorVagar -= Time.deltaTime; // fazendo uma contagem decrescente
        if (contadorVagar <= 0)
        {
            posicaoAleatoria = AleatorizarPosicao (); // busca uma posicao aleatoria
            contadorVagar += tempoEntrePosicoesAleatorias + Random.Range(-1f, 1f); // vai somar 4 segundos ao contador e aleatoriamente de -1 a 1 segundo de forma float pois pode ser decimal
            // com o random.range cada zumbi vai vagar em tempos diferentes
        }
        
        bool ficouPertoOSuficiente = Vector3.Distance (transform.position, posicaoAleatoria) <= 0.05; // testando se o zumbi chegou perto da posicao aleatoria, 0 absoluto da erro
        if(ficouPertoOSuficiente == false) // se ficar perto sera true e ele vai parar de andar, quando for falso ele vai vagar até a posicao aleatoria
        {
            direcao = posicaoAleatoria - transform.position; // faz a diferença entre a posicao aleatoria e o zumbi
            movimentoInimigo.Movimentar(direcao, statusInimigo.Velocidade); //Chamando o método movimentar da classe movimento personagem, enviando direção e velocidade
        }
    }

    Vector3 AleatorizarPosicao() //criamos um método do tipo Vector3 pois queremos retornar uma posicao Vector3
    {
        int raioZumbiVagar = 10; // setando o raio do terreno que o zumbi irá ficar vagando
        Vector3 posicao = Random.insideUnitSphere*raioZumbiVagar; //ESSE METODO PEGA UMA ESFERA DE RAIO NESTE CASO 10 E PEGA UMA POSICAO QUALQUER
        posicao += transform.position; // vai gerar uma posicao aleatoria baseada na posicao que o personagem está
        posicao.y = transform.position.y; // cancelar o y pq nao queremos ninguem voando
        
        return posicao; // quando executa esse metodo retorna essa posicao
    }

    void AtacaJogador () // este evento foi criado lá na animação de ataque, através dessa void nós realizamos esses 
    // passos após ocorrer o evento na animação, este evento ocorre quando zumbi encosta no jogador 
    {
        // comentamos essas 3 linhas pois não vamos executar essa ação aqui mais e vamos começar a controlar a vida do jogador lá no script controla jogador 
            //Time.timeScale = 0; // 0 pausa o jogo, 1 volta ao tempo normal, 2 o tempo fica 2x
            //Jogador.GetComponent<ControlaJogador>().TextoGameOver.SetActive(true); // ativa o texto de gameover
            //Jogador.GetComponent<ControlaJogador>().Vivo = false; // e seta jogador como morto

            
        dano = Random.Range(20,31); //gerar um dano de 20 a 30 no jogador de forma randomica
        Jogador.GetComponent<ControlaJogador>().TomarDano(dano); // aqui nós executamos o metodo TomarDano que criamos lá no script do jogador, então quando o zumbi encostar no jogador
         // ele executa este método

    }

    void AleatorizarZumbi ()
    {
        int geraTipoZumbi = Random.Range(1, transform.childCount); // numero inteiro para gerar randomicamente o tipo do zumbi, existem 26 tipos no prefab, 0 é o esqueleto e transform.childcount ele conta quantos filhos tem
        transform.GetChild(geraTipoZumbi).gameObject.SetActive(true); // primeiro pega o objeto dentro do gameobject que refere ao tipo do zumbi com getchild depois volta pra gameobject e seta ele como ativo
    }

    public void TomarDano(int dano)
    {
        statusInimigo.Vida -= dano;
        if (statusInimigo.Vida<= 0)
        {
            ZumbiNaoTomarTiro = true; // define a variavel para que o zumbi não tome mais tiro depois de morrer
            Morrer();
        }
    }

    public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ParticulaSangueZumbi, posicao, rotacao); // vai instanciar o objeto com uma posicao e uma rotação
    }

    public void Morrer()
    {
        Destroy(gameObject, 2) ; // Destroi este objeto após 2 segundos, pois espera ele descer do cenario pq ele vai desativar a colisao em movimentoInimigo e depois destroi
        animacaoInimigo.Morrer(); // inicia a animação de morrer
        this.enabled = false;
        ControlaAudio.instancia.PlayOneShot(SomDeMorte); //PlayoneShot, é pra tocar uma vez o som de morte
        // a gente consegue utilizar a instancia aqui, sem definir ela de novo, por causa do static lá no controla audio, ela pega o valor que está definido lá
        VerificarGeracaoKitMedico(porcentagemGerarKitMedico); //envia a porcentagem de gerar para o metodo verificageracaokitmedico no caso 10%
        scriptControlaInterface.AtualizarQuantidadeDeZumbisMorotos();
        meuGerador.DiminuirQuantidadeDeZumbisVivos(); // chama o método Diminuir quantiade de zumbis vivos do script gerador zumbis
    }

    void AnimacaoMorreu()
    {
        movimentoInimigo.Morrer(); // inicia a movimentacao de sumir do mapa
    }
    void VerificarGeracaoKitMedico(float porcentagemGeracao)
    {
        if(Random.value <= porcentagemGeracao) //Random.value gera um numero aleatorio de 0 a 1
        {
            Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity); //gera um kitmedico no pe do zumbi com rotação zerada
        }
    }
}
