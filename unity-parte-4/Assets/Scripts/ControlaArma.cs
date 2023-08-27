using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// vinculado também ao gameobject Jogador

public class ControlaArma : MonoBehaviour {
	// as variaveis publicas bala, cano da arma e som do tiro devem ser definidas lá no inspector
    public GameObject Bala; // cria um gameobject pra receber o prefab da bala
    public GameObject CanoDaArma; // cria um gameobject pra receber o cano da arma, objeto que criamos só pra marcar a posição inicial da bala na frente do cano da arma
	public AudioClip SomDoTiro; // variavel para armazenar o som do tiro


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

		// verifica se o botão de atirar foi pressionado, se sim, ele cria um objeto bala, na posicao do cano da arma e com a rotação do cano da arma
		if(Input.GetButtonDown("Fire1"))
        {
			// aqui dentro é o que acontece após apertar o botão de tiro
            Instantiate(Bala, CanoDaArma.transform.position, CanoDaArma.transform.rotation); // instantiate cria um objeto do prefab Bala com a posição e rotação do objeto cano da arma
			
			ControlaAudio.instancia.PlayOneShot(SomDoTiro); //PlayoneShot, é pra tocar uma vez o som do tiro
			// a gente consegue utilizar a instancia aqui, sem definir ela de novo, por causa do static lá no controla audio, ela pega o valor que está definido lá
        }
	}
}
