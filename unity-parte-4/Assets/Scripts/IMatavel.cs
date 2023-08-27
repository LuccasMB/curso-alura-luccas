using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMatavel //repare que é uma interface e não uma classe, nao tem start nem update nem nada 
// a classe que herda essa interface, deve obrigatoriamente ter todos os métodos em seu código
{
    void TomarDano(int dano);

    void Morrer ();
}
