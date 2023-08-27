using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// vinculado ao gameobject ControlaAudio
public class ControlaAudio : MonoBehaviour
{
    private AudioSource meuAudioSource; // criando uma variavel audiosource pra nao ter que ficar digitando GetComponent
    public static AudioSource instancia; // static faz com que a váriavel tenha o mesmo valor em todos os scripts, então se eu mudar o valor dela, muda em todos os scripts


    void Awake() // Este método roda antes do Start, é utilizado para tocar audio assim que o jogo starta
    {
        meuAudioSource = GetComponent<AudioSource>(); // define a variavel audiosource pra nao ter que ficar digitando GetComponent
        instancia = meuAudioSource; //definindo a instancia como GetComponent também, só que agora ela é uma variavel visivel e sempre igual para todos os scripts

    }

    public void StopAudio()
    {
        meuAudioSource.Stop();
    }
}
