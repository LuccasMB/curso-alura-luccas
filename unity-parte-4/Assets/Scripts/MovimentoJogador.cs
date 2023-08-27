using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//script que herda a classe MovimentoPersonagem e está ligado ao Jogador
public class MovimentoJogador : MovimentoPersonagem // mudei de monobehavior para colocar que o Movimento Jogador é filho de Movimento Personagem, ele é herança de MovimentoPersonamge e herda inclusive o monobehavior (update, start awake etc)

{
    public GameObject posicaoMiraJogador; //posicao foi setada no inspector como o canodaarma
    public void RotacaoJogador () // precisa receber a MascaraChao para identificar o que é chão, onde o raio vai impactar para conseguir rotacionar o jogador
    { //MascaraChao parou de ser usado quando criamos o plano pra definir o local da mira

        Ray raio = Camera.main.ScreenPointToRay(Input.mousePosition); //cria uma variavel para armazenar um raio da camera até a posição do mouse
        //Debug.DrawRay(raio.origin, raio.direction * 100, Color.red); //usado para conseguirmos enchergar o raio, colocando ele com tamanho 100 e na cor vermelha
        Plane plano = new Plane(Vector3.up, posicaoMiraJogador.transform.position); // cria um plano na posicaodamira do jogador

        //RaycastHit impacto; // criamos uma variavel do tipo raycasthit para armazenar o ponto de impacto do raio
        float distanciaColisao; //criamos uma variavel do tipo float pra receber a distancia do raio até o plano

        if(plano.Raycast(raio, out distanciaColisao)) // out  é para dizer que a variavel não tem valor, mas que ela receberá valor dentro desse processo
        {    
            Vector3 localColisao = raio.GetPoint(distanciaColisao); //cria um vector3 e armazena a distancia do raio com o plano
            localColisao.y = 0; // zera o eixo y

            //direcao para onde vamos olhar baseado onde estamos
            Vector3 posicaoParaOlhar = localColisao - transform.position;

            Rotacionar(posicaoParaOlhar); //chama o metodo rotacionar
        }

        //if(Physics.Raycast(raio, out impacto, 100, MascaraChao)) // out impacto é para dizer que a variavel não tem valor, mas que ela receberá valor dentro desse processo
        //{    // aqui a gente testa a fisica do raio se ele toca o chão, através da mascarachao (layer do objeto piso do estacionamento)
        //    Vector3 posicaoMiraJogador = impacto.point - transform.position; //criamos e armazenamos num vector 3 a posicao que está a mira do jogador(ponto de impacto) diminuinndo a posição do jogador

        //    posicaoMiraJogador.y = transform.position.y; // aqui é apenas para definir no vetor que criamos que o valor do eixo y é igual ao valor do eixo y do jogador, no caso 0

        //    //Quaternion novaRotacao = Quaternion.LookRotation(posicaoMiraJogador); //quaternion variavel de rotação para calcular a rotação que o jogador deverá fazer para olhar pro ponto de impacto
        //    //rigidbodyJogador.MoveRotation(novaRotacao); //seta a rotação do jogador através do rigidbody
        //    Rotacionar(posicaoMiraJogador); // isso está substituindo as duas linhas para rotação acima
        //}
    }
        


}
