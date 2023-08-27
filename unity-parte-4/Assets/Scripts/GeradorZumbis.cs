using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorZumbis : MonoBehaviour {

    public GameObject Zumbi; // gameobject definido no inspector com o prefab do zumbi
    private float contadorTempo = 0; // cria e define a variavel que irá contar o tempo de update
    public float TempoGerarZumbi = 1; // setamos o tempo para gerar zumbi como 1 segundo
    public LayerMask LayerZumbi; //setamos uma layer lá no inspector, setamos duas zumbi e cenario para testar nas duas

    private float distanciaDeGeracao = 3; //raio da esfera de geração randomica de zumbi
    private float distanciaDoJogadorParaGeracao = 20; // essa variavel é pra não nascer zumbi numa distancia menor que 20 com o jogador 
    private GameObject jogador; // variavel para receber o gameobject do jogador

    private int quantidadeMaximaDeZumbisVivos = 2; // esse numéro é por gerador
    private int quantidadeDeZumbisVivos; //quantidaded de zumbis vivos atual
    private float tempoProximoAumentoDeDificuldade = 30; // a cada 30 segundos vai aumentar a dificuldade
    private float contadorDeAumentarDificuldade; // variavel pra contar o tempo pra aumentar a dificuldade


    void Start() 
    {
        jogador = GameObject.FindWithTag("Jogador"); // definindo o gameobject do jogador atraves de tag
        contadorDeAumentarDificuldade = tempoProximoAumentoDeDificuldade; //define o tempo pro primeiro aumento de dificuldade com o tempo setado para o proxiumo aumento de dificuldade
        // criaremos um for pra criar a quantidade maxima de zumbis logo que o jogo começar
        for (int i = 0; i < quantidadeMaximaDeZumbisVivos; i++)
        {
            StartCoroutine(GerarNovoZumbi()); //chama a coroutine para gerar novo zumbi
        }
    }

	// Update is called once per frame
	void Update () {

        bool possoGerarZumbisPelaDistancia = Vector3.Distance(transform.position, jogador.transform.position) > distanciaDoJogadorParaGeracao;// testa de se a distancia do gerador para o jogador é maior que a distancia para geração neste caso 20, caso seja ela fica salva como true
        bool possoGerarZumbisPelaQuantidade = quantidadeDeZumbisVivos < quantidadeMaximaDeZumbisVivos; // testa se a quantidade de zumbi vivo é menor que a quantiade maxima de zumbi vivo e fica true

        // testa de se a distancia do gerador para o jogador é maior que a distancia para geração neste caso 20, se for pode gerar zumbi e a quantidade de zumbis vivos tem que ser menor que a quantidade maxima de zumbis vivos
        if (possoGerarZumbisPelaDistancia == true && possoGerarZumbisPelaQuantidade == true)
        {
            contadorTempo += Time.deltaTime; // é a mesma coisa de contadorTempo = contadorTempo + Time.deltaTime // deltatime é a diferença de tempo entre um update e o outro

            if(contadorTempo >= TempoGerarZumbi) // quando o contador de tempo chegar em 1 segundo vai criar um gameobject zumbi com a posição e rotação do objeto que possui esse codigo
            {
                StartCoroutine(GerarNovoZumbi());
                contadorTempo = 0; // zera o contador de tempo
            }
        }

        if(Time.timeSinceLevelLoad > contadorDeAumentarDificuldade) //timesincelevelload é o temmpo que a fase está rodando
        {
            quantidadeMaximaDeZumbisVivos ++; // aumenta a quantidade máxima de zumbi em 1
            contadorDeAumentarDificuldade = Time.timeSinceLevelLoad + tempoProximoAumentoDeDificuldade; //soma o tempo para o proximo aumento de dificuldade
        } 
    }

    void OnDrawGizmos () // esse método é só pra criar uns gizmos amarelos em volta dos geradores de zumbi, pra gente identificar eles mais facil
    {
        Gizmos.color = Color.yellow; // falo que meus gizmos irá possuir cor amarela
        Gizmos.DrawWireSphere(transform.position, distanciaDeGeracao); // distanciaDeGeração para desenhar o gizmos com o raio de geração na posicao do gerador de zumbi
    }

    IEnumerator GerarNovoZumbi() //IEnumerator é uma coroutine, para ser chamado o método voce tem que chara assim StartCoroutine(GerarNovoZumbi())
    // essa coroutine permite que a gente use o yield dentro do while. O yield return null diz para retornar nulo, passar de quadro o jogo caso fique rodando e nao encontre, 
    // isso é usado para que o jogo não trave dentro desse while em busca de uma posicao, ele testa se nao estiver encontrando e para depois testa denovo
    {
        Vector3 posicaoDeCriacao = AleatorizarPosicao();
        // colchetes no collider é pra informar que a variavel colisores será um vetor ou seja, irá receber varias posicoes de colisao na mesma variavel
        Collider[] colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi); //Overlapshepere vai testar na posicao de criacao uma esfera de raio 1 tudo que for uma colisao com a layer LayerZumbi (no caso outro zumbi e a layer cenario) e armazenar em colisores
        
        while (colisores.Length > 0) // se achar algum zumbi, tem que criar outra posicao e testar colisao de novo
        {
            posicaoDeCriacao = AleatorizarPosicao();
            // colchetes no collider é pra informar que a variavel colisores será um vetor ou seja, irá receber varias posicoes de colisao na mesma variavel
            colisores = Physics.OverlapSphere(posicaoDeCriacao, 1, LayerZumbi); //Overlapshepere vai testar na posicao de criacao uma esfera de raio 1 tudo que for uma colisao com a layer LayerZumbi (no caso outro zumbi) e armazenar em colisores
            yield return null;
        }
        ControlaInimigo zumbi = Instantiate(Zumbi, posicaoDeCriacao, transform.rotation).GetComponent<ControlaInimigo>(); // instantiate gera um zumbi do prefab zumbi com a posicao e a rotacao do geradorzumbi // e logo armazena no zumbi o componente ControlaInimigo que o prefab do zumbi possui
        zumbi.meuGerador = this; // aqui informa que o meuGerador do zumbi é esse script (o this fala que é esse script)
        quantidadeDeZumbisVivos ++; //quando cria um zumbi soma 1 
    }

    Vector3 AleatorizarPosicao()
    {
        Vector3 posicao = Random.insideUnitSphere * distanciaDeGeracao;
        posicao += transform.position;
        posicao.y = 0;
    
        return posicao;
    }

    public void DiminuirQuantidadeDeZumbisVivos ()
    {
        quantidadeDeZumbisVivos--; //diminui um zumbi vivo
    }
}
