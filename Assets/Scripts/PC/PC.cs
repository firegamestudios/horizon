using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class PC : MonoBehaviour
{
    public List<GameObject> baseZero;
    public List<GameObject> baseOne;
    public List<GameObject> baseTwo;

    int myIndex;

    //Photon
    PhotonView view;

    private void Awake()
    {
        view = GetComponent<PhotonView>();
    }

    // Start is called before the first frame update
    void Start()
    {
        //Local Player
        if (view.IsMine)
        {
            myIndex = PlayerPrefs.GetInt("Base");
            view.RPC("RPC_UpdateMyRemote", RpcTarget.AllBuffered, myIndex);
            
        }
    }

    void SetupCharacter(int index)
    {
        switch (index)
        {
            case 0:
                for (int i = 0; i < baseZero.Count; i++)
                {
                    baseZero[i].SetActive(true);
                }
                break;
            case 1:
                for (int i = 0; i < baseOne.Count; i++)
                {
                    baseOne[i].SetActive(true);
                }
                break;
            case 2:
                for (int i = 0; i < baseTwo.Count; i++)
                {
                    baseTwo[i].SetActive(true);
                }
                break;
        }
    }

    //Update remote
    [PunRPC]
    void RPC_UpdateMyRemote(int index)
    {
        SetupCharacter(index);
    }

}
