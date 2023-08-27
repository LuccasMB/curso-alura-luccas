using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlaCamera : MonoBehaviour {

    public GameObject Jogador; // cria uma variavel para receber o gameobject do jogador
    private Vector3 distCompensar; // cria um vetor tridimensional para calcular a distancia entre a camera e o jogador

	// Use this for initialization
	void Start () {
        distCompensar = transform.position - Jogador.transform.position; // quando se inicia o jogo calculamos a distancia entre a camera e o jogador
	}
	
	// Update is called once per frame
	void Update () {
        transform.position = Jogador.transform.position + distCompensar; // a medida que o jogador anda, a camera atualiza sua posição somando a distancia que ela tem que estar do jogador pra nao ficar em cima dele
	}
}
