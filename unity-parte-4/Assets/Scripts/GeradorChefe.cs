using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class GeradorChefe : MonoBehaviour
{
    private float tempoParaProximaGeracao = 0; // variavel que vai controlar o tempo para o proximo chefe que ira nasscer
    private float tempoEntreGeracoes = 10; // de 60s em 60ss nasce um chefe
    public GameObject ChefePrefeb; // variavel para definir o prefab do chefe la no inspector
    private ControlaInterface scriptControlaInterface; // variavel para receber a classe de controlainterface
    public Transform [] PosicoesPossiveisDeGeracao; //criamos uma variavel do tipo transform como um array pra guardar varias posicoes // temos que colocar lá no inspector
    private Transform jogador; //variavel pra pegar a posicao do jogador
   
    // Start is called before the first frame update
    void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes; //primeira geração acontece quando tiver um ciclo de tempoentregeracoes
        scriptControlaInterface = GameObject.FindObjectOfType(typeof(ControlaInterface)) as ControlaInterface; // uma variante pra usar o find sem ser via tag // a gente fala o tipo e depois diz como criaremos o objeto
        jogador = GameObject.FindWithTag("Jogador").transform; // definindo jogador com o transform do jogador
    }
    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > tempoParaProximaGeracao) // verifica se o tempo desde que a fase começou já é maior que o tempo para a proxima geracao de chefe
        {
            Vector3 posicaoDeCriacao = CalcularPosicaoMaisDistanteDoJogador(); // salva em posicaoDeCriacao a posicao mais distante do jogador 
            Instantiate(ChefePrefeb, posicaoDeCriacao, Quaternion.identity);  // cria o chefe na posicao mais distante do jogador com rotacao 0
            scriptControlaInterface.AparecerTextoChefeCriado(); // ativa o metodo para aparecer o texto quando o chefe for criado
            tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes; // soma no tempo desde que a fase comecou o tempo definido para entre geracoes
        }
    }

    Vector3 CalcularPosicaoMaisDistanteDoJogador()
    {
        Vector3 posicaoDeMaiorDistancia = Vector3.zero; // criando uma variavel e declarando ela como vazio
        float maiorDistancia = 0; //variavel para armazenar a maior distancia que encontrar
        foreach (Transform posicao in PosicoesPossiveisDeGeracao) //para cada elemento do tipo transform dentro do array PosicoesPossiveisDeGeracao ele roda o código 1 vez // posicao vale o elemento dentro do array na hr que estiver rodando
        {
            float distanciaEntreOJogador = Vector3.Distance(posicao.position, jogador.position); // calculando a distancia entre um gerador e o jogador    
            if (distanciaEntreOJogador > maiorDistancia) // testa qual distancia é maior
            {
                maiorDistancia = distanciaEntreOJogador; // salva a maior distancia pra ir continuar testando
                posicaoDeMaiorDistancia = posicao.position; //pega a posicao de maior distancia
            }
        }
        return posicaoDeMaiorDistancia; // retornar o valor da posicao com maior distancia do jogador
    }
}
