using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // para controlar as mudanças de scena

public class ControlaMenu : MonoBehaviour
{
    public GameObject BotaoSair; //variavel pra receber o gameobject do botao sair la no inspector

    private void Start ()
    {
     #if UNITY_STANDALONE || UNITY_EDITOR // testa se esse jogo está em modo sem ser webgl
        BotaoSair.SetActive(true); 
     #endif
    }

    public void JogarJogo()
    {
        StartCoroutine(MudarCena("game")); // manda dar load na cena game
    }

    IEnumerator MudarCena (String name)
    {
        yield return new WaitForSeconds(0.3f); //esperando um tempo pra mudar de cena de 0,3s
        SceneManager.LoadScene(name); // da load na cena 
    }

    public void SairDoJogo()
    {
        StartCoroutine(Sair()); // chama a coroutine de sair
    }

    IEnumerator Sair()
    {
        yield return new WaitForSecondsRealtime(0.3f); //esperando um tempo de 0,3s para sair // Realtime é independente de o jogo estar pausado, o que acontece no game over
        Application.Quit(); // Sair do Jogo
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // faz sair do jogo quando clicar em sair no editor da unity
        #endif
    }
}
