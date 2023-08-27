using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// este script esta ligado ao prefab do objeto bala que é iniciado através do Objeto Jogador, ela está definida no inspector do jogador no script de controla Arma

public class Bala : MonoBehaviour {

    public float Velocidade = 20; // variavel que controla velocidade da bala
    private Rigidbody rigidbodyBala; //variavel Rigidbody para não ter que ficar escrevendo get component toda hora
    public AudioClip SomDeMorte; // variavel para armazenar o som de morte do zumbi
    public int danoDoTiro = 1; // variavel que controla o dano do tiro

    private void Start()
    {
        rigidbodyBala = GetComponent<Rigidbody>(); // definindo a variavel rigidbody como get compononent rigidbody para evitar escrever sempre

    }

    // Update is called once per frame
    
    // sempre que usa rigidbody é necessário fazer pelo FixedUpdate
    void FixedUpdate () {
        //pega a posição da bala manda ela ir para frente com a velocidade setada
        rigidbodyBala.MovePosition
            (rigidbodyBala.position + 
            transform.forward * Velocidade * Time.deltaTime);
	}

    // OnTriggerEnter detecta se tiver colisão
    void OnTriggerEnter(Collider objetoDeColisao)
    {
        // objetoDeColisao é declarado como uma variavel de colisao, o if detecta se a colisão foi com um objeto com tag de inimigo e se a variavel para o zumbi nao tomar tiro está falsa
        // caso seja verdadeiro destroi o inimigo, e no fim sempre destroi a bala
    
        switch (objetoDeColisao.tag)
        {
            case "Inimigo":
                if(objetoDeColisao.GetComponent<ControlaInimigo>().ZumbiNaoTomarTiro == false) //se o zumbi estiver em processo de animacao de morrer ele nao toma bala mais
                {
                    objetoDeColisao.GetComponent<ControlaInimigo>().TomarDano(danoDoTiro); //chama o método tomar dano do objeto de colisão que é o zumbi e o dano é 1 pois o zumbi não tem vida qualquer dano mata ele
                } 
            break;
            case "ChefeDeFase":
                if(objetoDeColisao.GetComponent<ControlaChefe>().ChefeNaoTomarTiro == false) //se o chefe estiver em processo de animacao de morrer ele nao toma bala mais
                {
                    objetoDeColisao.GetComponent<ControlaChefe>().TomarDano(danoDoTiro); //chama o método tomar dano do objeto de colisão que é o chefe e o dano é 1 
                }
            break;
        }

        
        //if(objetoDeColisao.tag == "Inimigo" && objetoDeColisao.GetComponent<ControlaInimigo>().ZumbiNaoTomarTiro == false)
        //{
        //    objetoDeColisao.GetComponent<ControlaInimigo>().TomarDano(danoDoTiro); //chama o método tomar dano do objeto de colisão que é o zumbi e o dano é 1 pois o zumbi não tem vida qualquer dano mata ele
        //}

        Destroy(gameObject); // destroi a bala
    }
}
