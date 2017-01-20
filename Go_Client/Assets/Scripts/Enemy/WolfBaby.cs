//using UnityEngine;
//using System.Collections;

//public enum WolfState{
//    Idle,
//    Walk,
//    Attack,
//    Death
//}

//public class WolfBaby : MonoBehaviour {

//    public WolfState state = WolfState.Idle;
//    public int hp = 100;
//    public int exp = 20;
//    public int attack = 10;
//    public float miss_rate = 0.2f;
//    public string aniname_death;

//    public string aniname_idle;
//    public string aniname_walk;
//    public string aniname_now;
//    public float time = 1;
//    private float timer = 0;

//    private CharacterController cc;
//    public float speed = 1;

//    private bool is_attacked = false;
//    private Color normal;
//    public AudioClip miss_sound;

//    private GameObject hudtextFollow;
//    private GameObject hudtextGo;
//    public GameObject hudtextPrefab;

//    private HUDText hudtext;
//    private UIFollowTarget followTarget;
//    public GameObject body;

//    public string aniname_normalattack;
//    public float time_normalattack;

//    public string aniname_crazyattack;
//    public float time_crazyattack;
//    public float crazyattack_rate;

//    public string aniname_attack_now;
//    public int attack_rate =1;//攻击速率 每秒
//    private float attack_timer = 0;

//    public Transform target;

//    public float minDistance=2;
//    public float maxDistance=5;

//    public WolfSpawn spawn;
//    private PlayerStatus ps;

//    void Awake() {
//        aniname_now = aniname_idle;

//        cc = this.GetComponent<CharacterController>();
//        normal = body.GetComponent<Renderer>().material.color;
//        hudtextFollow = transform.Find("HUDText").gameObject;
//    }
//    void Start() {
//        //hudtextGo = GameObject.Instantiate(hudtextPrefab, Vector3.zero, Quaternion.identity) as GameObject;
//        //hudtextGo.transform.parent = HUDTextParent._instance.gameObject.transform;
//        hudtextGo= NGUITools.AddChild(HUDTextParent._instance.gameObject, hudtextPrefab);

//        hudtext = hudtextGo.GetComponent<HUDText>();
//        followTarget = hudtextGo.GetComponent<UIFollowTarget>();
//        followTarget.target = hudtextFollow.transform;
//        followTarget.gameCamera = Camera.main;

//        ps = GameObject.FindGameObjectWithTag(Tags.player).GetComponent<PlayerStatus>();
//        //followTarget.uiCamera = UICamera.current.GetComponent<Camera>();
//    }


//    void Update() {

//        if (state == WolfState.Death) {//死亡
//            GetComponent<Animation>().CrossFade(aniname_death);
//        } else if (state == WolfState.Attack) {//自动攻击状态
//            AutoAttack();
//        } else {//巡逻
//            GetComponent<Animation>().CrossFade(aniname_now);//播放当前动画
//            if (aniname_now == aniname_walk) {
//                cc.SimpleMove(transform.forward * speed);
//            }

//            timer += Time.deltaTime;
//            if (timer >= time) {//计时结束  切换状态
//                timer = 0;
//                RandomState();
//            }

//        }

//    }

//    void RandomState() {
//        int value = Random.Range(0, 2);
//        if (value == 0) {
//            aniname_now = aniname_idle;
//        } else {
//            if (aniname_now != aniname_walk) {
//                transform.Rotate(transform.up * Random.Range(0, 360));//当状态切换的时候，重新生成方向
//            }
//            aniname_now = aniname_walk;
//        }
//    }

//    public void TakeDamage(int attack) {//受到伤害
//        if (state == WolfState.Death) return;
//        target = GameObject.FindGameObjectWithTag(Tags.player).transform;
//        state = WolfState.Attack;
//        float value = Random.Range(0f, 1f);
//        if (value < miss_rate) {// Miss效果
//            AudioSource.PlayClipAtPoint(miss_sound, transform.position);
//            hudtext.Add("Miss", Color.gray, 1);
//        } else {//打中的效果
//            hudtext.Add("-"+attack, Color.red, 1);
//            this.hp -= attack;
//            StartCoroutine( ShowBodyRed() );
//            if (hp <= 0) {
//                state = WolfState.Death;
//                Destroy(this.gameObject, 2);
//            }
//        }
//    }


//    IEnumerator ShowBodyRed() {
//        body.GetComponent<Renderer>().material.color = Color.red;
//        yield return new WaitForSeconds(1f);
//        body.GetComponent<Renderer>().material.color = normal;
//    }

//    void AutoAttack() {
//        if (target != null) {
//            PlayerState playerState = target.GetComponent<PlayerAttack>().state;
//            if (playerState == PlayerState.Death) {
//                target = null;
//                state = WolfState.Idle; return;
//            }
//            float distance = Vector3.Distance(target.position, transform.position);
//            if (distance > maxDistance) {//停止自动攻击
//                target = null;
//                state = WolfState.Idle;
//            } else if (distance <= minDistance) {//自动攻击
//                attack_timer+=Time.deltaTime;
//                GetComponent<Animation>().CrossFade(aniname_attack_now);
//                if (aniname_attack_now == aniname_normalattack) {
//                    if (attack_timer > time_normalattack) {
//                        //产生伤害 
//                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
//                        aniname_attack_now = aniname_idle;
//                    }
//                } else if (aniname_attack_now == aniname_crazyattack) {
//                    if (attack_timer > time_crazyattack) {
//                        //产生伤害 
//                        target.GetComponent<PlayerAttack>().TakeDamage(attack);
//                        aniname_attack_now = aniname_idle;
//                    }
//                }
//                if (attack_timer > (1f / attack_rate)) {
//                    RandomAttack();
//                    attack_timer = 0;
//                }
//            } else {//朝着角色移动
//                transform.LookAt(target);
//                cc.SimpleMove(transform.forward * speed);
//                GetComponent<Animation>().CrossFade(aniname_walk);
//            }
//        } else {
//            state = WolfState.Idle;
//        }
//    }

//    void RandomAttack() {
//        float value = Random.Range(0f, 1f);
//        if (value < crazyattack_rate) {//进行疯狂攻击
//            aniname_attack_now = aniname_crazyattack;
//        } else {//进行普通攻击
//            aniname_attack_now = aniname_normalattack;
//        }
//    }

//    void OnDestroy() {
//        spawn.MinusNumber();
//        ps.GetExp(exp);
//        BarNPC._instance.OnKillWolf();
//        GameObject.Destroy(hudtextGo);
//    }

//    void OnMouseEnter() {
//        if(PlayerAttack._instance.isLockingTarget==false)
//        CursorManager._instance.SetAttack();
//    }
//    void OnMouseExit() {
//        if (PlayerAttack._instance.isLockingTarget == false)
//        CursorManager._instance.SetNormal();
//    }

//}
