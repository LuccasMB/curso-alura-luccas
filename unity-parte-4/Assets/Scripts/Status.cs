using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// script ligado ao jogador e ao prefab do zumbi
public class Status : MonoBehaviour
{
    public int VidaInicial = 100;
    [HideInInspector] //Atributo utilizado para esconder a variavel publica Vida do inspector
    public int Vida;
    public float Velocidade = 5;
    
    void Awake()
    {
        Vida = VidaInicial; // no start a vida é setada pro valor da vida inicial
    }

}
