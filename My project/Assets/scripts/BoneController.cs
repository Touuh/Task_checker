using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneController : MonoBehaviour
{
    MMD4MecanimModel model;
    MMD4MecanimModel.Morph[] morph;

    Dictionary<string, int> boneIndex = new Dictionary<string, int>(); // Boneの名前とインデックスを入れるところ
    HashSet<int> changedBoneIndex = new HashSet<int>();

    float Turn_Propotional_bias = 6.0f;

    // Start is called before the first frame update
    void Start()
    {
        model = GetComponent<MMD4MecanimModel>();

        foreach (var tmp in model.boneList){
            boneIndex.Add(tmp.boneData.nameJp, tmp.boneID); //Boneの名前とインデックスをすべて登録
        }
    }

    // Update is called once per frame
    void Update()
    {
        int screenX = Screen.width;
        int screenY = Screen.height;
        Vector3 mouse = Input.mousePosition;
        mouse.x = mouse.x / screenX *2 -1;
        mouse.y = mouse.y / screenY *2 -1;

        setBone("頭",
        Mathf.Abs(Turn_Propotional_bias * -mouse.y) <= 20f ? Turn_Propotional_bias * -mouse.y : 20f * Mathf.Sign(mouse.y),
        Mathf.Abs(Turn_Propotional_bias * -mouse.x) <= 10f ? Turn_Propotional_bias * -mouse.x : 10f * Mathf.Sign(mouse.x),
        Mathf.Abs(Turn_Propotional_bias / 2 * mouse.x) <= 5f ? Turn_Propotional_bias /2 * mouse.x : 5f * Mathf.Sign(mouse.x));

        setBone("腰",
        Mathf.Abs(Turn_Propotional_bias / 2 * -mosue.y) <= 3f ? Turn_Propotional_bias / 2 * -mouse.y : 3f * -Mathf.Sign(mouse.y),
        Mathf.Abs(Turn_Propotional_bias / 2 * -mouse.x) <= 10f ? Turn_Propotional_bias / 2 * -mouse.x : 10f * -Mathf.Sign(mouse.x),
        Mathf.Abs(Turn_Propotional_bias / 2 * mouse.x) <= 5f ? Turn_Propotional_bias / 2 * mouse.x : 5f * Mathf.Sign(mouse.x));
    }

    void setBone(string name, float x, float y, float z){
        Vector4 value = new Vector3(x, y, z);
        model.boneList[boneIndex[name]].userEulerAngles = value;
        changedBoneIndex.Add(boneIndex[name]);
    }
}