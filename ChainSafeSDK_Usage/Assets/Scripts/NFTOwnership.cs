using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//
using UnityEngine.SceneManagement;

//
using System.Numerics;
using Web3Unity.Scripts.Library.ETHEREUEM.EIP;

//
using Web3Unity.Scripts.Library.Ethers.Contracts;

public class NFTOwnership : MonoBehaviour
{
    [SerializeField] private bool _ownType1;
    [SerializeField] private bool _ownType2;

    [SerializeField] private GameObject NFT_TYPE1_OBJ;
    [SerializeField] private GameObject NFT_TYPE2_OBJ;

    [SerializeField] private bool checkedType1;
    [SerializeField] private bool checkedType2;

    [SerializeField] private GameObject WithoutNFT;

    string contract = "";
    string account = "";
    // Start is called before the first frame update
    async void Start()
    {
        NFT_TYPE1_OBJ.SetActive(false);
        NFT_TYPE2_OBJ.SetActive(false);

        checkedType1 = false;
        checkedType2 = false;

        WithoutNFT.SetActive(false);

        //contract address
        contract = "0x28C86a2E13Fc3439dC68d1EAA86F9cFeE9def11d";

        //temp ac address
        //string account = "0xB65705B5319D030A5a03171d408F85463658fFeB";
        account = PlayerPrefs.GetString("Account");

        //account balance
        BigInteger balance = await ERC721.BalanceOf(contract, account);
        print(balance);

        //check has type1?
        //CheckType(contract, account, 0);
        //CheckType(contract, account, 1);

        CheckAllTypes();
    }

    public void CheckAllTypes()
    {
        CheckType(contract, account, 0);
        CheckType(contract, account, 1);
    }

    public async void CheckType(string _contract, string _account, int checkType)  
    {
        //abi
        string abi = "[  {    \"inputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"constructor\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"approved\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"Approval\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      },      {        \"indexed\": false,        \"internalType\": \"bool\",        \"name\": \"approved\",        \"type\": \"bool\"      }    ],    \"name\": \"ApprovalForAll\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"NFTTransferred\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"previousOwner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"newOwner\",        \"type\": \"address\"      }    ],    \"name\": \"OwnershipTransferred\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": false,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"TokenMinted\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"Transfer\",    \"type\": \"event\"  },  {    \"inputs\": [],    \"name\": \"MAX_SUPPLY\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"PRICE\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"approve\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      }    ],    \"name\": \"balanceOf\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"getApproved\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"getTokenIdByType\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      }    ],    \"name\": \"isApprovedForAll\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"name\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"owner\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"ownerOf\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"user\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"ownsNFTType\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"renounceOwnership\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"safeMint\",    \"outputs\": [],    \"stateMutability\": \"payable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"safeTransferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      },      {        \"internalType\": \"bytes\",        \"name\": \"data\",        \"type\": \"bytes\"      }    ],    \"name\": \"safeTransferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      },      {        \"internalType\": \"bool\",        \"name\": \"approved\",        \"type\": \"bool\"      }    ],    \"name\": \"setApprovalForAll\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"bytes4\",        \"name\": \"interfaceId\",        \"type\": \"bytes4\"      }    ],    \"name\": \"supportsInterface\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"symbol\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"tokenURI\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"totalSupply\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"transferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"newOwner\",        \"type\": \"address\"      }    ],    \"name\": \"transferOwnership\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"withdraw\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  }]";
        //method
        string method = "ownsNFTType";

        var contract = new Contract(abi, _contract, RPC.GetInstance.Provider());

        var calldata = await contract.Call(method, new object[]
        {
            // if you need to add parameters you can do so, a call with no args is blank
            // arg1,
            // arg2
            _account,
            checkType
        });
        // display response in game
        //print("Contract Variable Total: " + calldata[0]);

        Debug.Log("Checking Type:" + checkType+" Calldata: " + calldata[0]);

        if (checkType == 0)
        {
            checkedType1 = true;
            if (calldata[0].ToString() == "True")
            {
                Type1Active();
                Debug.Log("Check Type 0: "+ calldata[0].ToString());
            }
        }
        else if (checkType == 1)
        {
            checkedType2 = true;
            if (calldata[0].ToString() == "True")
            {
                Type2Active();
                Debug.Log("Check Type 1: " + calldata[0].ToString());
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (checkedType1 && checkedType2)
        {
            if (!_ownType1 && !_ownType2)
            {
                //without nft
                WithoutNFT.SetActive(true);
            }
        }
    }

    private void Type1Active()
    {
        //
        _ownType1 = true;

        //
        NFT_TYPE1_OBJ.SetActive(true);
    }

    private void Type2Active()
    {
        //
        _ownType2 = true;

        //
        NFT_TYPE2_OBJ.SetActive(true);
    }

    public void Select_TYPE_1_NFT()
    {
        //print("Red");
        PlayerPrefs.SetInt("selectedNFT", 0);
        SceneManager.LoadScene("RecordTime");
    }

    public void Select_TYPE_2_NFT()
    {
        //print("Blue");
        PlayerPrefs.SetInt("selectedNFT", 1);
        SceneManager.LoadScene("RecordTime");
    }
    public void BACK_TO_HOMT()
    {
        Debug.Log("BACK_TO_HOME");
    }
}
