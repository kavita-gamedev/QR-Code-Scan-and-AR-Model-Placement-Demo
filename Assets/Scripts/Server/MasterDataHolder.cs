using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;

namespace RestAPIModule
{
    public class MasterDataHolder : MonoBehaviour
    {

        public static MasterDataHolder Instance;
        

        
        //class Cell
        //{
        //    public int x;
        //    public int y;
        //    public float G;
        //    public float H;
        //    public float F;
        //    public Cell parent;
        //    public INode cell;

        //    //public Cell(int x, int y, float G, float F, float H, Cell parent, INode c)
        //    //{
        //    //    this.x = x;
        //    //    this.y = y;
        //    //    this.G = G;
        //    //    this.H = H;
        //    //    this.F = F;
        //    //    this.parent = parent;
        //    //    this.cell = c;
        //    //}
        //}

        public void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(this.gameObject);
            }
            else
            {
                Destroy(this.gameObject);
            }
        }

       
    }
    




}

