using UnityEngine;
using System.Collections;

public class SkillBase : MonoBehaviour
{

    public float speed = 5;
    public Transform tar;
    public int damage = 0;//攻击力

    public void Start()
    {
    }

    public void Update()
    {
        float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, tar.position, step);
            float dis = Vector3.Distance(transform.position, tar.position);
            if (dis <= 0.1f)
            {
                if (tar.tag == Tags.enemy)
                {//攻击怪物
                    tar.GetComponent<MonsterBase>().updateBlood(-damage);
                }
                else
                {//攻击人物
                    tar.GetComponent<PlayerProperties>().updateBlood(-damage);
                }
                des();
            }        
    }

    void des() {
        Destroy(gameObject);
    }
}
