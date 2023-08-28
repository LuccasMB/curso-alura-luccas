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
        SceneManager.LoadScene("game"); // da load na cena "game"
    }

    public void SairDoJogo()
    {
        Application.Quit(); // Sair do Jogo
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false; // faz sair do jogo quando clicar em sair no editor da unity
        #endif
    }
}
