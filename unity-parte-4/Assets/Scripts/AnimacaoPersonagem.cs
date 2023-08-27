using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimacaoPersonagem : MonoBehaviour
{
    private Animator meuAnimator;

    void Awake () 
    {
        meuAnimator = GetComponent<Animator>();
    }
    public void Atacar (bool estado)
    {
        meuAnimator.SetBool ("Atacando", estado);
    }

    public void Movimentar(float valorMovimento)
    {
        meuAnimator.SetFloat("Movendo", valorMovimento); // seta o parametro movendo que criamos na animacao do jogador como verdadeira
    }

    public void Morrer () 
    {
        meuAnimator.SetTrigger("Morrer"); //seta o trigger do parametro morrer
    }
}
