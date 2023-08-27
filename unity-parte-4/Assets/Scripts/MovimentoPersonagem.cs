using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script vinculado ao jogador e ao prefab do zumbi
// este código tem o intuito de não ter que ficar repitindo esses comandos para controlar o jogador e o zumbi, escrevemos aqui uma vez, e acionamos lá
public class MovimentoPersonagem : MonoBehaviour
{
    private Rigidbody meuRigidbody; // variavel para receber o rigidbody para evitar ficar escrevendo get component toda hr

    void Awake ()
    {
        meuRigidbody = GetComponent<Rigidbody>(); // definindo a variavel com o getcomponent
    }
    public void Movimentar (Vector3 direcao, float velocidade) // método criado para poder movimentar tanto o jogador quanto o zumbi
    {
        meuRigidbody.MovePosition(
                    meuRigidbody.position +
                    direcao.normalized * velocidade * Time.deltaTime); //normalized é pra normalizar o vetor direção
    }

    public void Rotacionar(Vector3 direcao) // método criado para poder rotacionar tanto o jogador quanto o zumbi
    {
        // quaternion é uma variavel do unity que faz os calculos de rotacao para nós
        Quaternion novaRotacao = Quaternion.LookRotation(direcao); // lookrotation está setando a direção do jogador para a variavel nova rotacao
        meuRigidbody.MoveRotation(novaRotacao); // aqui colocamos o rigidbody do zumbi com a rotaçao do jogador, agora ele olha pro jogador
    }

    public void Morrer ()
    {
        meuRigidbody.constraints = RigidbodyConstraints.None; // tira todas as travas de freeze do zumbi
        meuRigidbody.isKinematic = false; //tem que desativar o iskinematic para a gravidade voltar a funcionar no rigibody na hora do chefe morrer pois ele tem iskinematic true devido ao navmesh
        meuRigidbody.velocity = Vector3.zero; // para a velocidade do rigibody do zumbi
        GetComponent<Collider>().enabled = false; //desativa qualquer colisao do objeto
    }
}
