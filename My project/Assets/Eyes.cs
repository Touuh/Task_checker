using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MorphController : MonoBehaviour
{
    MMD4MecanimModel model;
    MMD4MecanimModel.Morph[] morph;

    Dictionary<string, int> morphIndex = new Dictionary<string, int>(); // Morphの名前とインデックスの登録
    HashSet<int> changedMorphIndex = new HashSet<int>();

    void Start() {
        model = GetComponent<MMD4MecanimModel>();
        morph = model.morphList;
        int i = 0;
        foreach (var tmp in morph) {
            morphIndex.Add(tmp.morphData.nameJp, i++);
        }
    }

    void Update() {
        int screenX = Screen.width;
        int screenY = Screen.height;
        Vector3 mouse = Input.mousePosition;
        // マウス座標を画面中心を0, 0として-1~1に変換
        mouse.x = mouse.x / screenX * 2 - 1;
        mouse.y = mouse.y / screenY * 2 - 1;
        Debug.Log(Input.mousePosition);
        setMorph("瞳_上", mouse.y);
        setMorph("瞳_下", -mouse.y);
        setMorph("瞳_左", mouse.x);
        setMorph("瞳_右", -mouse.x);
    }

    // Morphの名前を渡すとvalueをセットしてくれる関数
    // たとえば，"ぐるぐる目", 1.0f で，目がぐるぐるする
    void setMorph(string name, float value) {
        model.morphList[morphIndex[name]].weight = value;
        changedMorphIndex.Add(morphIndex[name]);
    }
}
