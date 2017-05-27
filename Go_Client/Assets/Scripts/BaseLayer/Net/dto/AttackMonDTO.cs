using UnityEngine;
using System.Collections;

public class AttackMonDTO {

    public string Player { get; set; }
    public string TarPlayer { get; set; }

    public int FirstIndex { get; set; }
    public int SecondIndex { get; set; }
    public int Skill { get; set; }


    public AttackMonDTO() {
        FirstIndex = -1;
        SecondIndex = -1;
    }

}
