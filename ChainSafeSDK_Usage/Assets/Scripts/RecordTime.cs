using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using System.Threading.Tasks;
using Web3Unity.Scripts.Library.Ethers.Contracts;
using Web3Unity.Scripts.Library.Web3Wallet;

//
using Web3Unity.Scripts.Library.Ethers.Transactions;
using Web3Unity.Scripts.Library.Ethers.Providers;
using Web3Unity.Scripts.Library.Ethers.Signers;
using Nethereum.Hex.HexTypes;
using System;

//
using UnityEngine.SceneManagement;

public class RecordTime : MonoBehaviour
{

    [SerializeField] private string account;
    [SerializeField] private int selectedType;
    [SerializeField] private int tokenId;

    [SerializeField] private GameObject OBJ_UpdateSuccessfully;
    [SerializeField] private GameObject OBJ_UpdateUnsuccessfully;

    // Start is called before the first frame update
    void Start()
    {
        //
        account = PlayerPrefs.GetString("Account");
        selectedType = PlayerPrefs.GetInt("selectedNFT");

        //PART A
        //grab the tokenId
        //nft contract addr
        string nftContractAddr = "0x28C86a2E13Fc3439dC68d1EAA86F9cFeE9def11d";

        Debug.Log("1: "+ account);
        Debug.Log("2: " + selectedType);
        Debug.Log("3: " + nftContractAddr);
        FetchTokenId(nftContractAddr);

        //CheckTime("0xD609c1E856edaB8C36C33762dD487752Ebab2c46", 1);


        //
        OBJ_UpdateSuccessfully.SetActive(false);
        OBJ_UpdateUnsuccessfully.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {
        string nftContractAddr = "0x28C86a2E13Fc3439dC68d1EAA86F9cFeE9def11d";
        //FetchTokenId(nftContractAddr);
        //CheckTime("0xD609c1E856edaB8C36C33762dD487752Ebab2c46", 0);
    }

    public async void recordTime()
    {
        try
        {
            //
            // https://chainlist.org/
            var chainId = "97";
            // contract to interact with 
            var contract = "0xD609c1E856edaB8C36C33762dD487752Ebab2c46";
            // value in wei
            var value = "0";
            // abi in json format // game abi
            string abi = "[  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"getTimestamp\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      },      {        \"internalType\": \"uint256\",        \"name\": \"enteredTimestamp\",        \"type\": \"uint256\"      }    ],    \"name\": \"recordTimestamp\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  }]";
            // smart contract method to call
            var method = "recordTimestamp";
            // account to sent tokens to
            //var toAccount = PlayerPrefs.GetString("Account");
            // token id to send
            //var tokenId = 0;
            // amount of tokens to send
            //var amount = 1;
            // bytes
            //byte[] dataObject = { };

            //input para
            int _secs = 9996;

            // array of arguments for contract
            var contractData = new Contract(abi, contract);
            var data = contractData.Calldata(method, new object[]
            {
            tokenId,
            _secs
            });

            // gas limit OPTIONAL
            var gasLimit = "";
            // gas price OPTIONAL
            var gasPrice = "";
            // send transaction
            var response = await Web3Wallet.SendTransaction(chainId, contract, value, data, gasLimit, gasPrice);
            print(response);

            //TRACK STATUS
            // Check the Transaction adn return a transaction code
            var Transaction = await RPC.GetInstance.Provider().GetTransactionReceipt(response.ToString());

            // Debug Transaction code
            Debug.Log("Transaction Code: " + Transaction.Status);

            // Conditional Statement to check Transaction Status
            if (Transaction.Status.ToString() == "0")
            {
                Debug.Log("Transaction has failed");
                OBJ_UpdateUnsuccessfully.SetActive(true);
            }
            else if (Transaction.Status.ToString() == "1")
            {
                Debug.Log("Transaction has been successful");
                OBJ_UpdateSuccessfully.SetActive(true);
            }

        } catch (Exception e)
        {
            Debug.Log("Something Wong");
            OBJ_UpdateUnsuccessfully.SetActive(true);
        }
    }

    //check time
    public async void CheckTime(string _contract, int _tokenId)
    {
        Debug.Log("Checking");
        //game abi
        string abi = "[  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"getTimestamp\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      },      {        \"internalType\": \"uint256\",        \"name\": \"enteredTimestamp\",        \"type\": \"uint256\"      }    ],    \"name\": \"recordTimestamp\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  }]";        //method
        string method = "getTimestamp";

        var contract = new Contract(abi, _contract, RPC.GetInstance.Provider());

        var calldata = await contract.Call(method, new object[]
        {
            // if you need to add parameters you can do so, a call with no args is blank
            // arg1,
            // arg2
            _tokenId
        });
        // display response in game
        //print("Contract Variable Total: " + calldata[0]);

        print("res: "+calldata[0].ToString());

    }

    public async void FetchTokenId(string _contract)
    {
        //nft abi
        string abi = "[  {    \"inputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"constructor\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"approved\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"Approval\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      },      {        \"indexed\": false,        \"internalType\": \"bool\",        \"name\": \"approved\",        \"type\": \"bool\"      }    ],    \"name\": \"ApprovalForAll\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"NFTTransferred\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"previousOwner\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"newOwner\",        \"type\": \"address\"      }    ],    \"name\": \"OwnershipTransferred\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": false,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"TokenMinted\",    \"type\": \"event\"  },  {    \"anonymous\": false,    \"inputs\": [      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"indexed\": true,        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"Transfer\",    \"type\": \"event\"  },  {    \"inputs\": [],    \"name\": \"MAX_SUPPLY\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"PRICE\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"approve\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      }    ],    \"name\": \"balanceOf\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"getApproved\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"getTokenIdByType\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"owner\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      }    ],    \"name\": \"isApprovedForAll\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"name\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"owner\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"ownerOf\",    \"outputs\": [      {        \"internalType\": \"address\",        \"name\": \"\",        \"type\": \"address\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"user\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"ownsNFTType\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"renounceOwnership\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenURIType\",        \"type\": \"uint256\"      }    ],    \"name\": \"safeMint\",    \"outputs\": [],    \"stateMutability\": \"payable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"safeTransferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      },      {        \"internalType\": \"bytes\",        \"name\": \"data\",        \"type\": \"bytes\"      }    ],    \"name\": \"safeTransferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"operator\",        \"type\": \"address\"      },      {        \"internalType\": \"bool\",        \"name\": \"approved\",        \"type\": \"bool\"      }    ],    \"name\": \"setApprovalForAll\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"bytes4\",        \"name\": \"interfaceId\",        \"type\": \"bytes4\"      }    ],    \"name\": \"supportsInterface\",    \"outputs\": [      {        \"internalType\": \"bool\",        \"name\": \"\",        \"type\": \"bool\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"symbol\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"tokenURI\",    \"outputs\": [      {        \"internalType\": \"string\",        \"name\": \"\",        \"type\": \"string\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"totalSupply\",    \"outputs\": [      {        \"internalType\": \"uint256\",        \"name\": \"\",        \"type\": \"uint256\"      }    ],    \"stateMutability\": \"view\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"from\",        \"type\": \"address\"      },      {        \"internalType\": \"address\",        \"name\": \"to\",        \"type\": \"address\"      },      {        \"internalType\": \"uint256\",        \"name\": \"tokenId\",        \"type\": \"uint256\"      }    ],    \"name\": \"transferFrom\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [      {        \"internalType\": \"address\",        \"name\": \"newOwner\",        \"type\": \"address\"      }    ],    \"name\": \"transferOwnership\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  },  {    \"inputs\": [],    \"name\": \"withdraw\",    \"outputs\": [],    \"stateMutability\": \"nonpayable\",    \"type\": \"function\"  }]";
        string method = "getTokenIdByType";

        var contract = new Contract(abi, _contract, RPC.GetInstance.Provider());

        var calldata = await contract.Call(method, new object[]
        {
            // if you need to add parameters you can do so, a call with no args is blank
            // arg1,
            // arg2
            PlayerPrefs.GetString("Account"),
            PlayerPrefs.GetInt("selectedNFT")
    });
        // display response in game
        //print("Contract Variable Total: " + calldata[0]);

        //print("TokenId: " + calldata[0].ToString());

        tokenId = int.Parse(calldata[0].ToString());

        print("TokenID: " + tokenId);
    }

    public void BACK_TO_HOMT()
    {
        Debug.Log("BACK_TO_HOME");
    }
}
