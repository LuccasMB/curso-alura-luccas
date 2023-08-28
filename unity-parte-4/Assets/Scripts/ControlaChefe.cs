using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI; // pra usar navmesh
using UnityEngine.UI; // pra usar canvas


//script linkado ao chefe
public class ControlaChefe : MonoBehaviour, IMatavel
{

    private Transform jogador; // já vou buscar direto o transform do jogador pra saber a posicao dele
    private NavMeshAgent agente; //variavel para controlar o component navmeshagent
    private Status statusChefe;  // criando uma variavel (publica pois é acessado a vida no ControlaInterface) da classe criada no script status pra poder usar os métodos criados nela
    private AnimacaoPersonagem animacaoChefe; // criando uma variavel da classe criada no script animacao personagem pra poder usar os métodos criados nela
    private MovimentoPersonagem movimentoChefe; // criando uma variavel da classe criada no script movimento personagem pra poder usar os métodos criados nela
    public AudioClip SomDeMorteChefe; // variavel para armazenar o som de morte do chefe
    public bool ChefeNaoTomarTiro; // variavel criada com o intuito de fazer que o chefe não continue tendo colisoes entre bala e chefe durante a animação de morte do chefe
    public GameObject KitMedicoPrefab; //variavel que receberá o kitmedic no inspector
    public Slider SliderVidaChefe; // variavel pra receber o slide da vida do chefe la no inspector
    public Image ImagemSlider; //variavel que irá receber o fill do slider lá no inspector
    public Color CorDaVidaMaxima, CorDaVidaMinima; //definindo duas váriaves para receber as cores da vida // já vem como preta tem que mudar no inspector
    public GameObject ParticulaSangueChefe; //variavel pra receber o gameobject da particula sangue do chefe no inspector no prefab do chefe



    // Start is called before the first frame update
    void Start()
    {
        jogador = GameObject.FindWithTag("Jogador").transform; // declarando a posicao do jogador
        agente = GetComponent<NavMeshAgent>(); // declarando o navmesh
        statusChefe = GetComponent<Status>(); //declarando o statuschefe
        agente.speed = statusChefe.Velocidade; // definindo a velocidaded do agente nav mesh com a velocidade setada em status
        animacaoChefe = GetComponent<AnimacaoPersonagem>(); //declarando animacaoChefe
        movimentoChefe = GetComponent<MovimentoPersonagem>(); // declarando movimento chefe
        ChefeNaoTomarTiro = false; // quando o chefe é iniciado nós marcamos a variavel como falsa para que o chefe possa tomar tiro, que aconteça colisao entre a bala e o chefe
        SliderVidaChefe.maxValue = statusChefe.VidaInicial; //já seta o valor maximo da slider como o valor da vida inicial do chefe
        AtualizarInterface (); // atualiza a interface com a vida do chefe senão o valor maximo muda, mas o valor atual n muda

    }

    // Update is called once per frame
    void Update()
    {
        agente.SetDestination(jogador.position); //pega o navmeshagent e seta um destino para ele, que no caso é a posicao do jogador
        animacaoChefe.Movimentar(agente.velocity.magnitude); //passa a velocidade do agente como float, caso seja maior que 0.3 ele anda


        if(agente.hasPath == true) // testa se o agente já possui um caminho //só começo a testar se a unity já calculou o destino para o agente, se ele n tiver um destino ele nao entra no if
        {
            bool estouPertoDoJogador = agente.remainingDistance <= agente.stoppingDistance; // verifica se a distancia que falta pro agente chegar ao destino é maior ou igual a distancia que o agente deve parada (está setada como 5 no inspector)

            if(estouPertoDoJogador)
            {
                animacaoChefe.Atacar(true); //inicia a animacao de atacar
                Vector3 direcao = jogador.position - transform.position; //pegando uma direcao para o chefe rotacionar
                movimentoChefe.Rotacionar(direcao); // mandando o chefe rotacionar // lembrar de marcar no inspector do rigibody do chefe is Kinematic, limita a atuação do rigibody pois o chefe usa navmesh
            }
            else
            {
                animacaoChefe.Atacar(false); // não inicia animacao de atacar
            }
        }
        
    }

    void AtacaJogador()
    {
        int dano = Random.Range(30, 40); //gerar um dano de 30 a 40 no jogador de forma randomica
        jogador.GetComponent<ControlaJogador>().TomarDano(dano);// aqui nós executamos o metodo TomarDano que criamos lá no script do jogador, então quando o chefe encostar no jogador
    }

    public void TomarDano(int dano)
    {
        statusChefe.Vida -= dano;
        AtualizarInterface (); //atualiza a interface para atualizar o dano do chefe no slider
        if(statusChefe.Vida <= 0)
        {
            ChefeNaoTomarTiro = true; // define a variavel para que o chefe não tome mais tiro depois de morrer
            Morrer();  
        }
    }

        public void ParticulaSangue(Vector3 posicao, Quaternion rotacao)
    {
        Instantiate(ParticulaSangueChefe, posicao, rotacao); // vai instanciar o objeto com uma posicao e uma rotação
    }

    public void Morrer()
    {
        animacaoChefe.Morrer();
        ControlaAudio.instancia.PlayOneShot(SomDeMorteChefe); //PlayoneShot, é pra tocar uma vez o som de morte
        this.enabled = false;
        agente.enabled = false;
        Instantiate(KitMedicoPrefab, transform.position, Quaternion.identity);
        Destroy(gameObject,2);
    }

     void AnimacaoMorreu()
    {
        movimentoChefe.Morrer(); // inicia a movimentacao de sumir do mapa
    }

    void AtualizarInterface ()
    {
        SliderVidaChefe.value = statusChefe.Vida; //muda a slider de acordo com a vida do chefe
        float porcentagemDaVida = (float) statusChefe.Vida / statusChefe.VidaInicial; // define quantos porcentos de vida o chefe ainda tem //(float) coloca para converter o resultado em float, pois os dois numeros são inteiros
        Color corDaVida = Color.Lerp(CorDaVidaMinima, CorDaVidaMaxima, porcentagemDaVida); //Color.lerp faz uma interpolação em duas cores baseadas em uma porcentagem // salvamos na variavel corDaVida o resultado
        ImagemSlider.color  = corDaVida; //muda a cor da slider com a cordavida // desativar background da slider pra ficar a barrinha diminuindo
    }

}
