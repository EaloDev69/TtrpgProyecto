using UnityEngine;

[System.Serializable]
public class AtaqueDato
{
    public string nombreAtaque = "Ataque";
    public int    dañoMin      = 1;
    public int    dañoMax      = 6;

    [TextArea(1, 2)]
    public string descripcion = "";          // Opcional, para tooltips o log
}
