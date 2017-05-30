using UnityEngine;
using System.Collections;

public class AttackMonDTO {

    public string Player { get; set; }
    public string TarPlayer { get; set; }

    public int FirstIndex { get; set; }
    public int SecondIndex { get; set; }
    public int Skill { get; set; }
    //空技能的目标点
    public Assets.Model.Vector3 TarPos = new Assets.Model.Vector3();


    public AttackMonDTO() {
        FirstIndex = -1;
        SecondIndex = -1;
    }

}
