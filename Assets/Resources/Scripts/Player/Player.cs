using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    public VariableJoystick variableJoystick;
    private PlayerJob playerJob;

    public string playerName;

    private Vector3 movement;
    private float angle;

    public bool isPlayer = false;

    private PlayerSound playerSound;

    public Text         levelText;
    public Text         scoreText;
    public Text         gameOverScoreText;
    public Text         playernameText;
    public Image        hpBar;
    public Image        ExpBar;
    public Button       levelUpButton;
    public GameObject   skillUpPanel;

    public GameObject   playerImage;

    public Image skillSlider;
    public bool isCooldown = false;
    public float skillCooldownTime = 0.0f;

    public int level = 1;
    public float hp;
    public float maxHp;
    public float exp;
    public float maxExp = 10.0f;
    public float damage;
    public float defense;
    public float speed;

    public int score;

    private bool isReflect;

    public float shootDelay = 0.5f;
    private float shootTime = 0.0f;

    public float skillDelay = 5.0f;
    private float skillTime = 0.0f;

    public bool _attack;
    public bool isAttack
    {
        get { return _attack; }
        set { _attack = value; }
    }

    public bool _skill;
    public bool isSkill
    {
        get { return _skill; }
        set { _skill = value; }
    }

    public PlayerJob.PLAYERJOP jobNumber = PlayerJob.PLAYERJOP.Beginner;

    // Start is called before the first frame update
    void Start()
    {
        playerJob = gameObject.GetComponent<PlayerJob>();
        playerSound = gameObject.GetComponent<PlayerSound>();
        ChangeJob(jobNumber);
        if(isPlayer)
        {
            if (PlayerInfo.Instance == null)
                return;
            playerName = PlayerInfo.Instance.playername;
            playernameText.text = playerName;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if(isPlayer)
        {
            float horizontal = variableJoystick.Horizontal;
            float vertical = variableJoystick.Vertical;

            levelText.text = level + "";
            hpBar.fillAmount = (hp / maxHp);
            ExpBar.fillAmount = (exp / maxExp);

            if (Input.GetKeyDown(KeyCode.B))
            {
                ChangeJob(jobNumber);
            }

            if (Input.GetKeyDown(KeyCode.A))
            {
                exp += 2.0f;
            }

            if (exp >= maxExp )
            {
                maxExp = 10.0f;
                exp = 0.0f;
                level++;
                if(level < 3)
                {
                    levelUpButton.interactable = true;
                    LevelUpCheck(jobNumber);
                }
            }

            if ((int)jobNumber >= 1 && (int)jobNumber <= 4 && level > 2)
            {
                levelUpButton.interactable = true;
                LevelUpCheck(jobNumber);
            }

            if (isAttack)
            {
                Shooting();
            }

            if (isSkill && !isCooldown)
            {
                isCooldown = true;
                skillSlider.gameObject.SetActive(true);
                ActiveSkill();
            }

            if (isCooldown)
            {
                skillCooldownTime += Time.deltaTime;
                skillSlider.fillAmount = 1.0f - (Mathf.SmoothStep(0, 100, skillCooldownTime / skillDelay) / 100);
                if (skillCooldownTime > skillDelay)
                {
                    skillSlider.fillAmount = 0.0f;
                    skillSlider.gameObject.SetActive(false);
                    skillCooldownTime = 0.0f;
                    isCooldown = false;
                }
            }

            scoreText.text = score + "";

            angle += horizontal * 1.0f * Time.deltaTime;

            Move(horizontal, vertical);

            Turning(angle);
        }
        else
        {
            Move(0.0f, 0.0f);
            speed = 0.0f;
            Shooting();
        }

        if(hp >= maxHp)
        {
            hp = maxHp;
        }

        if (hp <= 0.0f)
        {
            SoundManager.Instance.PlayerDieSoundPlay();
            if(isPlayer)
            {
                gameOverScoreText.text = score + "";
                GameManager.Instance.gameObject.GetComponent<GameManager_GameOver>().TurnOnGameOverPanel();
            }
            for (int i = 0; i < 20 + ((level - 1) * 10); i++)
            {
                GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.PlayerDieBalloon);
                if (obj == null)
                    break;
                obj.transform.position = transform.position + new Vector3(Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f), Random.Range(-5.0f, 5.0f));
                obj.SetActive(true);
            }
            hp = 0.0f;
            gameObject.SetActive(false);

        }
    }

    void Move(float horizontal, float vertical)
    {
        movement.Set(horizontal, vertical, 1.0f);

        movement = movement.normalized * speed * Time.deltaTime;

        gameObject.transform.Translate(movement);
    }

    void Turning(float angle)
    {
        gameObject.transform.rotation = Quaternion.LookRotation(new Vector3(Mathf.Sin(angle), 0.0f, Mathf.Cos(angle)));
    }

    public void Shooting()
    {

        if(Time.time > shootTime)
        {
            playerSound.AttackSoundPlay();
            shootTime = Time.time + shootDelay;

            Debug.Log(playerName + "은(는) 발사했다!");
            GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
            if (obj == null)
                return;

            Bullet bul = obj.GetComponent<Bullet>();
            bul.speed = 10.0f;
            bul.damage = damage;
            obj.name = playerName + "Bullet";
            obj.transform.position = transform.Find("Beginner").position;
            obj.transform.rotation = transform.rotation;
            obj.SetActive(true);
        }
    }

    //void Sniping()
    //{
    //    if(isSniping)
    //    {
    //        Debug.Log(playerName + "은(는) 스나이퍼총으로 발사했다!");
    //        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
    //        if (obj == null)
    //            return;

    //        Bullet bul = obj.GetComponent<Bullet>();
    //        bul.speed = 50.0f;
    //        bul.damage = 10.0f;
    //        obj.name = playerName + "Bullet";
    //        obj.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
    //        obj.transform.position = transform.Find("Sniper").position;
    //        obj.transform.rotation = transform.rotation;
    //        obj.SetActive(true);

    //        isSniping = false;
    //        speed = 1.5f;
    //    }
    //}

    public void ActiveSkill()
    {
        StartCoroutine(Skill());
    }

    private IEnumerator Skill()
    {
        if(Time.time > skillTime)
        {
            skillTime = Time.time + skillDelay;
            switch (jobNumber)
            {
                #region ---------- Level 1 ----------
                case PlayerJob.PLAYERJOP.Beginner:
                    break;
                #endregion

                #region ---------- Level 2 ----------
                case PlayerJob.PLAYERJOP.Attacker:
                    break;
                case PlayerJob.PLAYERJOP.Defender:
                    break;
                case PlayerJob.PLAYERJOP.Quicker:
                    break;
                case PlayerJob.PLAYERJOP.Specialist:
                    break;
                #endregion

                #region ---------- Level 3 ----------
                /* 스킬 구현 상태
                 * ---- LV 3 ----
                 * Rifler           o
                 * Hunter           o
                 * Shielder         o
                 * Mirrorlian       o
                 * Ghost            
                 * Flash            o
                 * Vulture          o
                 * Magician         
                 * --------------
                 */
                // --------- Attacker ---------
                case PlayerJob.PLAYERJOP.Rifler:
                    Debug.Log(jobNumber + "스킬 : 좌우 라이플 2개에서 탄환 발사");
                    playerSound.RiflerSkillSoundPlay();
                    // Left
                    {
                        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
                        if (obj == null)
                            yield return null;
                        // Left
                        Bullet bul = obj.GetComponent<Bullet>();
                        bul.speed = 15.0f;
                        bul.damage = 2.0f;
                        obj.name = playerName + "Bullet";
                        obj.transform.position = transform.Find("Rifler").transform.Find("Left").position;
                        obj.transform.rotation = transform.rotation;
                        obj.SetActive(true);
                    }
                    // Right
                    {
                        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
                        if (obj == null)
                            yield return null;
                        // Left
                        Bullet bul = obj.GetComponent<Bullet>();
                        bul.speed = 15.0f;
                        bul.damage = 2.0f;
                        obj.name = playerName + "Bullet";
                        obj.transform.position = transform.Find("Rifler").transform.Find("Right").position;
                        obj.transform.rotation = transform.rotation;
                        obj.SetActive(true);
                    }
                    break;
                case PlayerJob.PLAYERJOP.Hunter:
                    Debug.Log(jobNumber + "스킬 : 정면 포신 12개에서 탄환 발사");
                    playerSound.HunterSkillSoundPlay();
                    {
                        for (int i = 0; i < 3; i++)
                        {
                            for (int j = 0; j < 4; j++)
                            {
                                GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
                                if (obj == null)
                                    yield return null;
                                // Left
                                Bullet bul = obj.GetComponent<Bullet>();
                                bul.speed = 15.0f;
                                bul.damage = 0.5f;
                                obj.name = playerName + "Bullet";
                                obj.transform.localScale = new Vector3(0.2f, 0.2f, 0.2f);
                                obj.transform.position = transform.Find("Hunter").position + new Vector3(-0.2f * i, -0.2f * j, 0.0f);
                                obj.transform.rotation = transform.rotation;
                                obj.SetActive(true);
                            }
                        }
                    }
                    break;
                // --------- --------- ---------

                // --------- Defender ---------
                case PlayerJob.PLAYERJOP.Shielder:
                    Debug.Log(jobNumber + "스킬 : 15초간 방어력 2배");
                    playerSound.ShielderSkillSoundPlay();
                    {
                        defense *= 2.0f;
                        yield return new WaitForSeconds(15.0f);
                        defense /= 2.0f;
                    }
                    break;
                case PlayerJob.PLAYERJOP.Mirrorlian:
                    Debug.Log(jobNumber + "스킬 : 15초간 반사상태(적의 공격력 1/2만큼 피해반사");
                    playerSound.MirrorlianSkillSoundPlay();
                    {
                        isReflect = true;
                        yield return new WaitForSeconds(15.0f);
                        isReflect = false;
                    }
                    break;
                // --------- --------- ---------

                // --------- Quicker ---------
                case PlayerJob.PLAYERJOP.Ghost:
                    playerSound.GhostSkillSoundPlay();
                    break;
                case PlayerJob.PLAYERJOP.Flash:
                    Debug.Log(jobNumber + "스킬 : 15초간 속도 2배");
                    playerSound.FlashSkillSoundPlay();
                    {
                        speed *= 2.0f;
                        yield return new WaitForSeconds(15.0f);
                        speed /= 2.0f;
                    }
                    break;
                // --------- --------- ---------

                // --------- Specialist ---------
                case PlayerJob.PLAYERJOP.Vulture:
                    Debug.Log(jobNumber + "스킬 : 현재 위치 지뢰 생성");
                    playerSound.VultureSkillSoundPlay();
                    {
                        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
                        if (obj == null)
                            yield return null;

                        Bullet bul = obj.GetComponent<Bullet>();
                        bul.speed = 0.0f;
                        bul.damage = 5.0f;
                        obj.name = playerName + "Bullet";
                        obj.transform.position = transform.Find("Vulture").position;
                        obj.SetActive(true);
                    }
                    break;
                case PlayerJob.PLAYERJOP.Magician:
                    playerSound.MagicianSkillSoundPlay();
                    break;
                // --------- --------- ---------
                #endregion

                #region ---------- Level 4 ----------
                /* 스킬 구현 상태
                 * ---- LV 4 ----
                 * Sniper       
                 * Cannon_Shooter   o
                 * Knight           o
                 * Reflectist       o
                 * Rogue            -
                 * Thief            -
                 * Bomber           
                 * Soulmaster          
                 * --------------
                 */
                // --------- Attacker ---------
                case PlayerJob.PLAYERJOP.Sniper:
                    Debug.Log(jobNumber + "스킬 : 캐릭터 정지 후 1인칭 스코프 스나이핑");
                    {
                    }
                    break;
                case PlayerJob.PLAYERJOP.Cannon_Shooter:
                    Debug.Log(jobNumber + "스킬 : 정면 포신 1개에서 대포 발사");
                    {
                        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.Bullet);
                        if (obj == null)
                            yield return null;

                        Bullet bul = obj.GetComponent<Bullet>();
                        bul.speed = 5.0f;
                        bul.damage = 5.0f;
                        obj.name = playerName + "Bullet";
                        obj.transform.localScale = new Vector3(5.0f, 5.0f, 5.0f);
                        obj.transform.position = transform.Find("Cannon_Shooter").position;
                        obj.transform.rotation = transform.rotation;
                        obj.SetActive(true);
                    }
                    break;
                // --------- --------- ---------

                // --------- Defender ---------
                case PlayerJob.PLAYERJOP.Knight:
                    Debug.Log(jobNumber + "스킬 : 10초간 무적");
                    {
                        defense = 10.0f;
                        yield return new WaitForSeconds(10.0f);
                        defense = 2.5f;
                    }
                    break;
                case PlayerJob.PLAYERJOP.Reflectist:
                    Debug.Log(jobNumber + "스킬 : 10초간 완전반사");
                    {
                        isReflect = true;
                        yield return new WaitForSeconds(10.0f);
                        isReflect = false;
                    }
                    break;
                // --------- --------- ---------

                // --------- Quicker ---------
                case PlayerJob.PLAYERJOP.Rogue:
                    Debug.Log(jobNumber + "스킬 : 5초간 투명상태, 스피드 3배, 공격력 3배");
                    {
                        speed *= 3.0f;
                        damage *= 3.0f;
                        yield return new WaitForSeconds(5.0f);
                        speed /= 3.0f;
                        damage /= 3.0f;
                    }
                    break;
                case PlayerJob.PLAYERJOP.Thief:
                    Debug.Log(jobNumber + "스킬 : 15초간 스피드 1.5배, 적 scr, exp 약탈(0.5)");
                    {
                        speed *= 1.5f;
                        yield return new WaitForSeconds(1.5f);
                        speed /= 1.5f;
                    }
                    break;
                // --------- --------- ---------

                // --------- Specialist ---------
                case PlayerJob.PLAYERJOP.Bomber:
                    break;
                case PlayerJob.PLAYERJOP.Soulmaster:
                    break;
                // --------- --------- ---------
                #endregion

                #region ---------- Level 5 ----------
                // --------- Attacker ---------
                case PlayerJob.PLAYERJOP.Razerlist:
                    break;
                case PlayerJob.PLAYERJOP.Biochemist:
                    break;
                // --------- --------- ---------

                // --------- Defender ---------
                case PlayerJob.PLAYERJOP.King:
                    break;
                case PlayerJob.PLAYERJOP.Spiker:
                    break;
                // --------- --------- ---------

                // --------- Quicker ---------
                case PlayerJob.PLAYERJOP.Assassin:
                    break;
                case PlayerJob.PLAYERJOP.Sneaker:
                    break;
                // --------- --------- ---------

                // --------- Specialist ---------
                case PlayerJob.PLAYERJOP.General:
                    break;
                case PlayerJob.PLAYERJOP.Wizard:
                    break;
                // --------- --------- ---------
                #endregion

                default:
                    break;
            }
        }
    }

    void ChangeJob(PlayerJob.PLAYERJOP index)
    {
        switch(index)
        {
            #region ---------- Level 1 ----------
            case PlayerJob.PLAYERJOP.Beginner:
                SetPlayerStat(5.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f);
                break;
            #endregion

            #region ---------- Level 2 ----------
            case PlayerJob.PLAYERJOP.Attacker:
                SetPlayerStat(10.0f, 2.0f, 1.0f, 1.5f, 0.5f, 0.0f);
                break;
            case PlayerJob.PLAYERJOP.Defender:
                SetPlayerStat(15.0f, 1.0f, 2.5f, 1.0f, 0.5f, 0.0f);
                break;
            case PlayerJob.PLAYERJOP.Quicker:
                SetPlayerStat(10.0f, 1.5f, 1.0f, 2.0f, 0.5f, 0.0f);
                break;
            case PlayerJob.PLAYERJOP.Specialist:
                SetPlayerStat(12.5f, 1.5f, 1.5f, 1.5f, 0.5f, 0.0f);
                break;
            #endregion

            #region ---------- Level 3 ----------
            // --------- Attacker ---------
            case PlayerJob.PLAYERJOP.Rifler:
                SetPlayerStat(10.0f, 2.0f, 1.0f, 1.5f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Hunter:
                SetPlayerStat(10.0f, 2.0f, 1.0f, 1.5f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Defender ---------
            case PlayerJob.PLAYERJOP.Shielder:
                SetPlayerStat(15.0f, 1.0f, 2.5f, 1.0f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Mirrorlian:
                SetPlayerStat(15.0f, 1.0f, 2.5f, 1.0f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Quicker ---------
            case PlayerJob.PLAYERJOP.Ghost:
                SetPlayerStat(10.0f, 1.5f, 1.0f, 2.0f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Flash:
                SetPlayerStat(10.0f, 1.5f, 1.0f, 2.0f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Specialist ---------
            case PlayerJob.PLAYERJOP.Vulture:
                SetPlayerStat(12.5f, 1.5f, 1.5f, 1.5f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Magician:
                SetPlayerStat(12.5f, 1.5f, 1.5f, 1.5f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------
            #endregion

            #region ---------- Level 4 ----------
            // --------- Attacker ---------
            case PlayerJob.PLAYERJOP.Sniper:
                SetPlayerStat(12.5f, 2.5f, 1.0f, 1.5f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Cannon_Shooter:
                SetPlayerStat(12.5f, 2.5f, 1.0f, 1.5f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Defender ---------
            case PlayerJob.PLAYERJOP.Knight:
                SetPlayerStat(20.0f, 1.5f, 2.5f, 1.25f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Reflectist:
                SetPlayerStat(20.0f, 1.5f, 2.5f, 1.25f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Quicker ---------
            case PlayerJob.PLAYERJOP.Rogue:
                SetPlayerStat(12.5f, 2.0f, 1.0f, 2.25f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Thief:
                SetPlayerStat(12.5f, 2.0f, 1.0f, 2.25f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Specialist ---------
            case PlayerJob.PLAYERJOP.Bomber:
                SetPlayerStat(15.0f, 1.75f, 1.75f, 1.75f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Soulmaster:
                SetPlayerStat(15.0f, 1.75f, 1.75f, 1.75f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------
            #endregion

            #region ---------- Level 5 ----------
            // --------- Attacker ---------
            case PlayerJob.PLAYERJOP.Razerlist:
                SetPlayerStat(15.0f, 3.0f, 1.5f, 1.75f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Biochemist:
                SetPlayerStat(15.0f, 3.0f, 1.5f, 1.75f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Defender ---------
            case PlayerJob.PLAYERJOP.King:
                SetPlayerStat(30.0f, 1.75f, 3.5f, 1.25f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Spiker:
                SetPlayerStat(30.0f, 1.75f, 3.5f, 1.25f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Quicker ---------
            case PlayerJob.PLAYERJOP.Assassin:
                SetPlayerStat(15.0f, 2.25f, 1.5f, 2.75f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Sneaker:
                SetPlayerStat(15.0f, 2.25f, 1.5f, 2.75f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------

            // --------- Specialist ---------
            case PlayerJob.PLAYERJOP.General:
                SetPlayerStat(20.0f, 2.0f, 2.0f, 2.0f, 0.5f, 1.0f);
                break;
            case PlayerJob.PLAYERJOP.Wizard:
                SetPlayerStat(20.0f, 2.0f, 2.0f, 2.0f, 0.5f, 1.0f);
                break;
            // --------- --------- ---------
            #endregion

            default:
                SetPlayerStat(5.0f, 1.0f, 1.0f, 1.0f, 0.5f, 0.0f);
                break;

        }
        jobNumber = index;
        if(isPlayer)
            playerImage.GetComponent<PlayerImage>().ChangePlayerImage(index);
        playerJob.SetPlayerJob(index);
    }

    public void LevelUpCheck(PlayerJob.PLAYERJOP index)
    {
        switch (index)
        {
            case PlayerJob.PLAYERJOP.Beginner:
                skillUpPanel.transform.Find("Level2").gameObject.SetActive(true);
                break;
            case PlayerJob.PLAYERJOP.Attacker:
                skillUpPanel.transform.Find("Level3").transform.Find("Attacker").gameObject.SetActive(true);
                break;
            case PlayerJob.PLAYERJOP.Defender:
                skillUpPanel.transform.Find("Level3").transform.Find("Defender").gameObject.SetActive(true);
                break;
            case PlayerJob.PLAYERJOP.Quicker:
                skillUpPanel.transform.Find("Level3").transform.Find("Quicker").gameObject.SetActive(true);
                break;
            case PlayerJob.PLAYERJOP.Specialist:
                skillUpPanel.transform.Find("Level3").transform.Find("Specialist").gameObject.SetActive(true);
                break;
        }
    }

    public void LevelUp(int index)
    {
        SoundManager.Instance.ButtonClickPlay();
        ChangeJob((PlayerJob.PLAYERJOP)index);
        skillUpPanel.transform.Find("Level2").gameObject.SetActive(false);
        skillUpPanel.transform.Find("Level3").transform.Find("Attacker").gameObject.SetActive(false);
        skillUpPanel.transform.Find("Level3").transform.Find("Defender").gameObject.SetActive(false);
        skillUpPanel.transform.Find("Level3").transform.Find("Quicker").gameObject.SetActive(false);
        skillUpPanel.transform.Find("Level3").transform.Find("Specialist").gameObject.SetActive(false);
        levelUpButton.interactable = false;
    }

    void SetPlayerStat(float maxHp, float damage, float defense, float speed, float shootDelay, float skillDelay)
    {
        this.maxHp = maxHp;
        this.hp = maxHp;
        this.exp = 0.0f;
        this.damage = damage;
        this.defense = defense;
        this.speed = speed;
        this.shootDelay = shootDelay;
        this.skillDelay = skillDelay;
    }

    void PlayerEffect(Vector3 position)
    {
        playerSound.DamageSoundPlay();
        GameObject obj = GameManager.Instance.objectPooler.GetPooledObject((int)ObjectPooler.OBJECTPOOLER.ExplosionEffect);
        if (obj == null)
            return;
        obj.transform.position = position;
        obj.SetActive(true);
    }

    IEnumerator BackSpeed()
    {
        yield return new WaitForSeconds(2.0f);
        speed *= -1.0f;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player") || other.tag.Equals("Airship"))
        {
            playerSound.PlayerToObjectHitSoundPlay();

            speed *= -1.0f;
            StartCoroutine(BackSpeed());
        }

        if(other.tag.Equals("Item"))
        {
            playerSound.GetBalloonSoundPlay();

            Balloon bal = other.gameObject.GetComponent<Balloon>();

            exp += bal.exp;
            score += bal.score;
            hp += bal.hp;

            other.gameObject.SetActive(false);
        }

        if (other.tag.Equals("Bullet") && !other.name.Equals(playerName + "Bullet"))
        {
            Bullet bul = other.gameObject.GetComponent<Bullet>();

            if (isReflect)
            {
                if (jobNumber == PlayerJob.PLAYERJOP.Mirrorlian)
                {
                    bul.speed *= -1.0f;
                    bul.damage *= 0.5f;
                    Debug.Log(playerName + "은(는) " + bul.damage + "의 데미지로 팅겨낸다!");
                    other.gameObject.name = playerName + "Bullet";
                }
                else if(jobNumber == PlayerJob.PLAYERJOP.Reflectist)
                {
                    bul.speed *= -1.0f;
                    Debug.Log(playerName + "은(는) " + bul.damage + "의 데미지로 팅겨낸다!");
                    other.gameObject.name = playerName + "Bullet";
                }
            }
            else
            {
                PlayerEffect(other.transform.position);
                if (defense == 10.0f)
                {
                    Debug.Log(playerName + "은(는) 무적이다!");
                    other.gameObject.SetActive(false);
                }
                else
                {
                    if (hp <= 0.0f)
                    {
                        Debug.Log(playerName + "은(는) 쥬겄당..그만때려..");
                    }
                    else
                    {
                        hp -= (bul.damage - ((defense * 0.1f) * bul.damage));
                        Debug.Log(playerName + "은(는) " + bul.damage + "의 데미지를 받았고 " + defense + "의 방어력에 의해 " + (bul.damage - ((defense * 0.1f) * bul.damage)) + "만큼 체력이 감소 되었다");
                        Debug.Log(playerName + "은(는) 체력이" + hp + "만큼 남았다.");
                        if (hp <= 0.0f)
                        {
                            Debug.Log(playerName + "은(는) 쥬겄당..");
                        }
                    }

                    other.gameObject.SetActive(false);
                    return;
                }
            }
        }
    }
}
