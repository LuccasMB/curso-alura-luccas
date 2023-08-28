using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//este script está linkado as posições de criacao do chefe lá no inspector

public class GizmosChefe : MonoBehaviour
{
    void OnDrawGizmos () // esse método é só pra criar uns gizmos vermelhos no gerador de chefe
    {
        Gizmos.color = Color.red; // falo que meus gizmos irá possuir cor vermelho
        Gizmos.DrawWireSphere(transform.position, 5); // 5 para desenhar o gizmos com o raio 5
    }
}
