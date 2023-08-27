using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeradorChefe : MonoBehaviour
{
    private float tempoParaProximaGeracao = 0; // variavel que vai controlar o tempo para o proximo chefe que ira nasscer
    private float tempoEntreGeracoes = 10; // de 60s em 60ss nasce um chefe
    public GameObject ChefePrefeb; // variavel para definir o prefab do chefe la no inspector


    // Start is called before the first frame update
    void Start()
    {
        tempoParaProximaGeracao = tempoEntreGeracoes; //primeira geração acontece quando tiver um ciclo de tempoentregeracoes

    }

    // Update is called once per frame
    void Update()
    {
        if(Time.timeSinceLevelLoad > tempoParaProximaGeracao) // verifica se o tempo desde que a fase começou já é maior que o tempo para a proxima geracao de chefe
        {
            Instantiate(ChefePrefeb, transform.position, Quaternion.identity);  // cria o chefe na posicao do gerador com rotacao 0
            tempoParaProximaGeracao = Time.timeSinceLevelLoad + tempoEntreGeracoes; // soma no tempo desde que a fase comecou o tempo definido para entre geracoes
        }
    }
}