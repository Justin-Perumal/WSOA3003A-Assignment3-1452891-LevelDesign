using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public enum BattleStates {START, PLAYERTURN, PLAYERTURNEND, ENEMYTURN, ENEMYTURNEND, WON, LOST}

public class BattleController : MonoBehaviour
{
    [Header("GameObjects")]
    public GameObject PlayerPrefab;
    public GameObject PlayerObj;
    public GameObject EnemyPrefab;
    public GameObject EnemyObj;
    public Transform PlayerPos;
    public Transform EnemyPos;
    public GameObject CommandText;
    public Transform CardPos1;
    public Transform CardPos2;
    public Transform CardPos3;
    public GameObject Choice1, Choice2, Choice3;
    public BattleStates states;

    [Header("Scripts")]
    public CardManager CM;
    Unit PlayerUnit;
    Unit EnemyUnit;
    public HUDControl PlayerHUD;
    public HUDControl EnemyHUD;

    [Header("Attacking and death status")]
    public int RandomEnemyAttack;
    private bool EnemyDead = false;
    private bool PlayerDead = false;

    [Header("Defend status")]
    public bool PlayerIsDefending = false;
    public bool EnemyIsDefending = false;

    [Header("Cripple")]
    public bool EnemyIsCrippled = false;
    public int EnemyCrippleCounter;
    public bool PlayerIsCrippled = false;
    public int PlayerCrippleCounter;

    [Header("Poison")]
    private bool EnemyIsPoisoned = false;
    private int EnemyPoisonCounter;
    private bool PlayerIsPoisoned = false;
    private int PlayerPoisonCounter;

    [Header("HUD")]
    // public GameObject EnemyCrippleText, PlayerCrippleText, EnemyPoisonText, PlayerPoisonText;
    public GameObject EnemyCrippleIcon, PlayerCrippleIcon, EnemyPoisonIcon, PlayerPoisonIcon;
    public GameObject EnemyPoisonCounterText, EnemyCrippleCounterText;

    [Header("Particles and Audio")]
    public ParticleSystem PlayerParticleHeal;
    public ParticleSystem EnemyParticleHeal;
    public ParticleSystem EnemyPoisonParticle;
    public ParticleSystem PlayerLifestealParticle;
    public AudioSource PlayerAudio;
    public AudioSource EnemyAudio;
    public AudioClip HitSound;

    // Start is called before the first frame update
    void Start()
    {
        states = BattleStates.START;
        StartCoroutine(SetupBattle());
    }

    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape))
        {
            QuitGame();
            Debug.Log("quit game");
        }
    }

    public void QuitGame()
    {
    Application.Quit();
    }

    IEnumerator SetupBattle()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Battle has begun!";

        PlayerObj = Instantiate(PlayerPrefab, PlayerPos);
        PlayerUnit = PlayerObj.GetComponent<Unit>();
        PlayerAudio = PlayerObj.GetComponent<AudioSource>();
        EnemyObj = Instantiate(EnemyPrefab, EnemyPos);
        EnemyUnit = EnemyObj.GetComponent<Unit>();
        EnemyAudio = EnemyObj.GetComponent<AudioSource>();

        PlayerHUD.SetHUD(PlayerUnit);
        EnemyHUD.SetHUD(EnemyUnit);

        yield return new WaitForSeconds(3f);

        states = BattleStates.PLAYERTURN;
        PlayerTurn();
    }

    void PlayerTurn()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Choose a command";

        int Random1 = Random.Range(0,1);
        Choice1 = Instantiate(CM.Cards[Random1],CardPos1);

        if (Choice1.CompareTag("Attack"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice1.CompareTag("Defend"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice1.CompareTag("Heal"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice1.CompareTag("Cripple"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice1.CompareTag("Poison"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice1.CompareTag("Lifesteal"))
        {
            Choice1.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 

        int Random2 = Random.Range(1,3);
        Choice2 = Instantiate(CM.Cards[Random2],CardPos2);
        if (Choice2.CompareTag("Attack"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice2.CompareTag("Defend"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice2.CompareTag("Heal"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice2.CompareTag("Cripple"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice2.CompareTag("Poison"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice2.CompareTag("Lifesteal"))
        {
            Choice2.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 

        int Random3 = Random.Range(3,CM.Cards.Length);
        Choice3 = Instantiate(CM.Cards[Random3],CardPos3);
        if (Choice3.CompareTag("Attack"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Attack);
        }
        if(Choice3.CompareTag("Defend"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Defend);
        }
        if(Choice3.CompareTag("Heal"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Heal);
        } 
        if(Choice3.CompareTag("Cripple"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Cripple);
        } 
        if(Choice3.CompareTag("Poison"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Poison);
        } 
        if(Choice3.CompareTag("Lifesteal"))
        {
            Choice3.GetComponent<Button>().onClick.AddListener(Lifesteal);
        } 
  
        if(PlayerIsDefending)
        {
            PlayerIsDefending = false;
            PlayerObj.GetComponent<Image>().sprite = PlayerUnit.CharacterSprites[0];
        }
        
        if(PlayerUnit.CurrentHP > PlayerUnit.MaxHP/2)
        {
            PlayerObj.GetComponent<Image>().sprite = PlayerUnit.CharacterSprites[0];
        }
        else if(PlayerUnit.CurrentHP < PlayerUnit.MaxHP/2)
        {
            PlayerObj.GetComponent<Image>().sprite = PlayerUnit.CharacterSprites[1];
        }
        
    }

    void DestroyChoice()
    {
        Destroy(Choice1);
        Destroy(Choice2);
        Destroy(Choice3);
    }

    public void Attack()
    {
        if(states != BattleStates.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerAttack());
        DestroyChoice();

        Debug.Log("Attacking");
    }

    public void Defend()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerDefend());
        DestroyChoice();

        Debug.Log("Defending");
    }

    public void Heal()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerHeal());
        DestroyChoice();

        Debug.Log("Healing");
    }

    public void Cripple()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerCripple());
        DestroyChoice();
    }

    public void Poison()
    {
        if(states != BattleStates.PLAYERTURN)
            return;

        StartCoroutine(PlayerPoison());
        DestroyChoice();
    }

    public void Lifesteal()
    {
        if(states != BattleStates.PLAYERTURN)
            return;
        
        StartCoroutine(PlayerLifesteal());
        DestroyChoice();
    }

    IEnumerator PlayerAttack()
    {
        PlayerDmg();
        CommandText.GetComponent<TextMeshProUGUI>().text = "Attacking!";

        yield return new WaitForSeconds(2f);

        EnemyHUD.SetHUD(EnemyUnit);

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }
    }

    IEnumerator PlayerDefend()
    {
        PlayerIsDefending = true;
        PlayerObj.GetComponent<Image>().sprite = PlayerUnit.CharacterSprites[2];
        CommandText.GetComponent<TextMeshProUGUI>().text = "Defending!";

        yield return new WaitForSeconds(2f);

        states = BattleStates.ENEMYTURN;
        StartCoroutine(EnemyTurn());
    }

    IEnumerator PlayerHeal()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Healing!";

        PlayerParticleHeal.Play();

        yield return new WaitForSeconds(2f);

        PlayerUnit.CurrentHP += PlayerUnit.HealAmount;
        PlayerHUD.SetHUD(PlayerUnit);
        if(PlayerUnit.CurrentHP > PlayerUnit.MaxHP)
        {
            PlayerHUD.SetHUD(PlayerUnit);
            PlayerUnit.CurrentHP = PlayerUnit.MaxHP;
        }

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerCripple()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Crippling enemy!";

        EnemyIsCrippled = true;
        EnemyCrippleCounter = 3;

        EnemyCrippleCounterText.GetComponent<TextMeshProUGUI>().text = EnemyCrippleCounter.ToString();
        EnemyCrippleIcon.SetActive(true);

        yield return new WaitForSeconds(2f);

        //EnemyCrippleText.GetComponent<TextMeshProUGUI>().text = "Enemy is crippled";

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerPoison()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Poisoning enemy!";

        EnemyPoisonCounter = 3;

        EnemyPoisonIcon.SetActive(true);
        EnemyPoisonCounterText.GetComponent<TextMeshProUGUI>().text = EnemyPoisonCounter.ToString();

        yield return new WaitForSeconds(2f);

        EnemyIsPoisoned = true;
        //EnemyPoisonText.GetComponent<TextMeshProUGUI>().text = "Enemy is poisoned";
  
        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator PlayerLifesteal()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "Lifestealing from enemy!";
        PlayerLifestealParticle.Play();

        PlayerUnit.CurrentHP++;
        EnemyUnit.CurrentHP--;

        EnemyHUD.SetHUD(EnemyUnit);
        PlayerHUD.SetHUD(PlayerUnit);

        yield return new WaitForSeconds(2f);

        if(PlayerUnit.CurrentHP > PlayerUnit.MaxHP)
        {
            PlayerHUD.SetHUD(PlayerUnit);
            PlayerUnit.CurrentHP = PlayerUnit.MaxHP;
        }

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
            EnemyDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.ENEMYTURN;
            StartCoroutine(EnemyTurn());
        }

    }

    IEnumerator EnemyTurn()
    {
        if(EnemyIsPoisoned)
        {
            EnemyUnit.CurrentHP--;
            EnemyHUD.SetHUD(EnemyUnit);
            EnemyPoisonParticle.Play();
            if(EnemyPoisonCounter > 0)
            {
                EnemyPoisonCounterText.GetComponent<TextMeshProUGUI>().text = (EnemyPoisonCounter-1).ToString();
                EnemyPoisonCounter--;
                if(EnemyPoisonCounter == 0)
                {
                    EnemyIsPoisoned = false;
                    EnemyPoisonIcon.SetActive(false);
                    //EnemyPoisonText.GetComponent<TextMeshProUGUI>().text = "";
                }
            }
        }

        if(EnemyIsCrippled)
        {
            if(EnemyCrippleCounter > 0)
            {
                EnemyCrippleCounterText.GetComponent<TextMeshProUGUI>().text = (EnemyCrippleCounter-1).ToString();
                EnemyCrippleCounter--;
                if(EnemyCrippleCounter==0)
                {
                    EnemyIsCrippled = false;
                    EnemyCrippleIcon.SetActive(false);
                }
            }
        }

        if(EnemyIsDefending)
        {
            EnemyIsDefending = false;
            EnemyObj.GetComponent<Image>().sprite = EnemyUnit.CharacterSprites[0];
        }
        if(EnemyUnit.CurrentHP <= (EnemyUnit.MaxHP/2))
        {
            EnemyObj.GetComponent<Image>().sprite = EnemyUnit.CharacterSprites[1];
        }

        if(EnemyUnit.CurrentHP <= 0)
        {
            states = BattleStates.WON;
        }

        CommandText.GetComponent<TextMeshProUGUI>().text = "Enemy's turn!";

        if(EnemyCrippleCounter == 0)
        {
            EnemyIsCrippled = false;
            EnemyCrippleIcon.SetActive(false);
           // EnemyCrippleText.GetComponent<TextMeshProUGUI>().text = "";
        }

        if(EnemyUnit.UnitName == "Grunt")
        {
            if(EnemyUnit.CurrentHP < EnemyUnit.MaxHP)
            {
                RandomEnemyAttack = Random.Range(1,4);
            }
            else
            {
                RandomEnemyAttack = Random.Range(1,3);
            }

            if(RandomEnemyAttack == 1)
            {
                EnemyDmg();
            }
            if(RandomEnemyAttack == 2)
            {
                EnemyDefend();
            }
            if(RandomEnemyAttack == 3)
            {
                EnemyHeal();
            } 
        }
        else if(EnemyUnit.UnitName == "Beserker")
        {
            if(EnemyUnit.CurrentHP < (EnemyUnit.MaxHP/2))
            {
                EnemyDmg();
            }
            else
            {
                 RandomEnemyAttack = Random.Range(1,3);
                 if(RandomEnemyAttack == 1)
                 {
                     EnemyDmg();
                 }
                 if(RandomEnemyAttack == 2)
                 {
                     EnemyDefend();
                 }
            }
        }
        else if(EnemyUnit.UnitName == "Blademail")
        {
            if(EnemyUnit.CurrentHP < (EnemyUnit.MaxHP/3))
            {
                RandomEnemyAttack = Random.Range(1,3);
                if(RandomEnemyAttack == 1)
                {
                    EnemyHeal();
                }
                if(RandomEnemyAttack == 2)
                {
                    EnemyDefend();
                }
            }
            else
            {
                RandomEnemyAttack = Random.Range(1,3);
                if(RandomEnemyAttack == 1)
                {
                    EnemyDmg();
                }
                    if(RandomEnemyAttack == 2)
                {
                    EnemyDefend();
                }
            }
        }

        yield return new WaitForSeconds(2f);

        //PlayerHUD.SetHUD(PlayerUnit);

        if(PlayerUnit.CurrentHP <= 0)
        {
            states = BattleStates.LOST;
            PlayerDead = true;
            EndBattle();
        }
        else
        {
            states = BattleStates.PLAYERTURN;
            PlayerTurn();
        }
    }

    public void EndBattle()
    {
        if(states == BattleStates.WON)
        {
            StartCoroutine(BattleWon());
            EnemyObj.GetComponent<Image>().sprite = EnemyUnit.CharacterSprites[3];
            Debug.Log("You have won");
        }
        else if(states == BattleStates.LOST)
        {
            StartCoroutine(BattleLost());
            PlayerObj.GetComponent<Image>().sprite = PlayerUnit.CharacterSprites[3];
            Debug.Log("You have lost");
        }
    }

    IEnumerator BattleWon()
    {
        if(EnemyUnit.UnitName == "Grunt")
        {
            CommandText.GetComponent<TextMeshProUGUI>().text = "You won, heal up and get ready for the next!";
            yield return new WaitForSeconds (4f);
            SceneManager.LoadScene("BeserkerBattle");
        }

        if(EnemyUnit.UnitName == "Beserker")
        {
            CommandText.GetComponent<TextMeshProUGUI>().text = "You won, heal up and get ready for the next!";
            yield return new WaitForSeconds (4f);
            SceneManager.LoadScene("BossBattle");
        }
        
        if(EnemyUnit.UnitName == "Blademail")
        {
            CommandText.GetComponent<TextMeshProUGUI>().text = "You won the gauntlet! Congratulations!";
            yield return new WaitForSeconds (6f);
            SceneManager.LoadScene("GruntBattle");
        }
    }

     IEnumerator BattleLost()
    {
        CommandText.GetComponent<TextMeshProUGUI>().text = "You have lost the battle!";
        yield return new WaitForSeconds (4f);
        SceneManager.LoadScene("GruntBattle");
    }

    void PlayerDmg()
    {
        EnemyAudio.PlayOneShot(HitSound);

        if(EnemyIsDefending)
        {
          EnemyUnit.CurrentHP -= Mathf.RoundToInt(PlayerUnit.Damage/2);  
        }
        else
        {
          EnemyUnit.CurrentHP -= PlayerUnit.Damage;
        }


        if(EnemyUnit.UnitName == "Blademail")
        {
            PlayerUnit.CurrentHP -= 2;
        }
        PlayerHUD.SetHUD(PlayerUnit);
        EnemyHUD.SetHUD(EnemyUnit);
    }

    void EnemyDmg()
    {
        PlayerAudio.PlayOneShot(HitSound);
        CommandText.GetComponent<TextMeshProUGUI>().text = "Enemy is attacking!";

        if(EnemyIsCrippled)
        {
            if(PlayerIsDefending)
            {
                PlayerUnit.CurrentHP -= Mathf.RoundToInt((EnemyUnit.Damage/2)-1);
            }
            else
            {
                PlayerUnit.CurrentHP -= (EnemyUnit.Damage-1);
            }
        }
        else if(!EnemyIsCrippled)
        {
             if(PlayerIsDefending)
            {
                PlayerUnit.CurrentHP -= Mathf.RoundToInt((EnemyUnit.Damage)/2);
                PlayerIsDefending = false;
            }
            else
            {
                PlayerUnit.CurrentHP -= (EnemyUnit.Damage);
            }
        }

        PlayerHUD.SetHUD(PlayerUnit);
    }

    void EnemyDefend()
    {
        EnemyIsDefending = true;
        EnemyObj.GetComponent<Image>().sprite = EnemyUnit.CharacterSprites[2];
        CommandText.GetComponent<TextMeshProUGUI>().text = "Enemy is defending!";
    }

    void EnemyHeal()
    {
        EnemyParticleHeal.Play();
        EnemyUnit.CurrentHP += EnemyUnit.HealAmount;
        if(EnemyUnit.CurrentHP > EnemyUnit.MaxHP)
        {
            EnemyUnit.CurrentHP = EnemyUnit.MaxHP;
        }
        CommandText.GetComponent<TextMeshProUGUI>().text = "Enemy is healing!";
        EnemyHUD.SetHUD(EnemyUnit);
    }

}
