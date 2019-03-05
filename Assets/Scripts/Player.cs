using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

using UnityEngine;
[RequireComponent(typeof(PlayerSetup))]
public class Player : NetworkBehaviour
{
    [SyncVar]
    private bool _isDead = false;
    public bool isDead
    {
        get { return _isDead; }
        protected set { _isDead = value; }
    }


    [SerializeField]
    private int maxHealth = 100;
    
    [SyncVar]
    private int currtentHealth;

    [SyncVar]
    public int kills;
    [SyncVar]
    private int deaths;

    [SerializeField]
    private Behaviour[] disableOnDeath;
    private bool[] wasEnabled;

    [SerializeField]
    private GameObject[] disableGameObjectOnDeath;

    [SerializeField]
    GameObject deathEffect;
    [SerializeField]
    GameObject spawnEffect;

    private bool firstSetup = true;

    public void SetupPlayer()
    {

        GameManager.instance.SetSceneCameraActive(false);
        GetComponent<PlayerSetup>().playerUIInstance.SetActive(true);

        CmdBroadCastNewPlayerSetup();
    }

    [Command]
    private void CmdBroadCastNewPlayerSetup()
    {
        RpcSetupPlayerOnAllClients();
    }
    [ClientRpc]
    void RpcSetupPlayerOnAllClients()
    {
        if (firstSetup)
        {
            wasEnabled = new bool[disableOnDeath.Length];
            for (int i = 0; i < wasEnabled.Length; i++)
            {
                wasEnabled[i] = disableOnDeath[i].enabled;
            }
            firstSetup = false;
        }
        SetDefaults();
    }
   /* void Update()
    {
        if (!isLocalPlayer)
            return;
        if(Input.GetKeyDown(KeyCode.K))
        {
            RpcTakeDamage(99999);
            
        }
    }*/

    [ClientRpc]
    public void RpcTakeDamage(int _amount, string _sourceID)
    {

        if (isDead)
            return;

        currtentHealth -= _amount;

        Debug.Log(transform.name + " now has " + currtentHealth + " health.");
        if(currtentHealth<= 0)
        {
            
            Die(_sourceID);
        }


    }

    private void Die(string _sourceID)
    {
        isDead = true;

        Player sourcePlayer = GameManager.GetPlayer(_sourceID);
        if (sourcePlayer != null)
        {
            sourcePlayer.kills++;
        }
        deaths++;
        
        
        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = false;
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(false);
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)   
            _col.enabled = false;


        GameObject _gFxIns = (GameObject)Instantiate(deathEffect, transform.position, Quaternion.identity);
        Destroy(_gFxIns, 2f);



        if (isLocalPlayer)
        {
            GameManager.instance.SetSceneCameraActive(true);
            GetComponent<PlayerSetup>().playerUIInstance.SetActive(false);
        }
        Debug.Log(transform.name + " is Dead!");

        StartCoroutine(Respawn());
    }

    private IEnumerator Respawn()
    {
        yield return new WaitForSeconds(GameManager.instance.matchSettings.respawnTime);
        
        Transform _spawnPoint = NetworkManager.singleton.GetStartPosition();
        transform.position = _spawnPoint.position;
        transform.rotation = _spawnPoint.rotation;

        yield return new WaitForSeconds(0.1f);
        SetupPlayer();
        Debug.Log(transform.name + " Respawned");
    }
    public void SetDefaults()
    {

        isDead = false;
        currtentHealth = maxHealth;

        for (int i = 0; i < disableOnDeath.Length; i++)
        {
            disableOnDeath[i].enabled = wasEnabled[i];
        }

        for (int i = 0; i < disableGameObjectOnDeath.Length; i++)
        {
            disableGameObjectOnDeath[i].SetActive(true);
        }
        Collider _col = GetComponent<Collider>();
        if (_col != null)
            _col.enabled = true;
        
        
        GameObject _gFxIns = (GameObject)Instantiate(spawnEffect, transform.position, Quaternion.identity);
        Destroy(_gFxIns, 2f);
    }   
}
