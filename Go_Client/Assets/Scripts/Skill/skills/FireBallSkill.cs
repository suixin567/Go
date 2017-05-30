﻿using UnityEngine;
using System.Collections;

public class FireBallSkill : SkillBase {

    void Update()
    {
        if (skillTar != null)
        {//有目标
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, skillTar.position, step);
            float dis = Vector3.Distance(transform.position, skillTar.position);
            if (dis <= 0.1f)
            {
                if (skillTar.tag == Tags.enemy)
                {//攻击怪物
                    skillTar.GetComponent<MonsterBase>().updateBlood(-damage);
                }
                else
                {//攻击人物
                    skillTar.GetComponent<PlayerProperties>().updateBlood(-damage);
                }
                Destroy(gameObject);
            }
        }
        else
        {//无目标的释放技能
            float step = speed * Time.deltaTime;
            transform.position = Vector3.MoveTowards(transform.position, skillTarPos, step);
            float dis = Vector3.Distance(transform.position, skillTarPos);
            if (dis <= 0.1f)
            {
                Destroy(gameObject);
            }
        }

    }
}
