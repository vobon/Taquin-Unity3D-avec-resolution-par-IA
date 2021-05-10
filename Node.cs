using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Node 
{
    public int f;
    public int g;
    public int h;
    public int[,] etat ;
    public Node ancetre;

    // Start is called before the first frame update
    public Node(int n)
    {
        f = 0;
        g = 0;
        h = 0;
        etat = new int[n, n];
        ancetre = null;
    }

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
    
    }


}
