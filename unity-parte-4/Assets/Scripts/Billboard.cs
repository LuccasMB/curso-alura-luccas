using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Billboard : MonoBehaviour
{
    // Update is called once per frame
    void Update()
    {
        //transform.lookat é um método que voce pode setar pra onde voce quer olhar, nós queremos olhar para a camera principal
        //a camera tem uma posicao pra frente que é o eixo Z, nós queremos mandar o canvas olhar para -Z
        //Camera.main é o objeto da camera principal
        //nós mandamos o canvas olhar para a posição oposta a frente da camera baseada na posicao que o canvas está por isso tem que subtrair
        //assim onde estiver o canvas sempre irá estar olhando para a camera, de qualquer posicao
        transform.LookAt(transform.position + Camera.main.transform.forward); // normalmente o sinal aqui é de menos, mas como é uma barra de vida, vamos colocar + pois isso vai inverter o lado que a barra de vida diminui
    }
}
