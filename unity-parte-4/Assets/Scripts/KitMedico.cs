using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KitMedico : MonoBehaviour
{

    private int quantidadeDeCura = 15; //quantidade de cura de 15
    private int tempoDeDestruicao = 5; //tempo para se destruir 5s

    void Start()
    {
        Destroy(gameObject, tempoDeDestruicao); //depois de 5 segundos detroi o kitmedico se ninguem pegar
    }

    // possui tambem OnCollisionEnter, OnColissionStay e OnTriggerExit
    private void OnTriggerEnter(Collider objetoDeColisao) 
    {
        if(objetoDeColisao.tag == "Jogador") //testa se o objetodeColisao tem a tag jogador
        {
            objetoDeColisao.GetComponent<ControlaJogador>().CurarVida(quantidadeDeCura); // chama o metodo curar vida dentro do script controla jogador
            Destroy(gameObject); //destroi o kit medico
        }
    }

}
