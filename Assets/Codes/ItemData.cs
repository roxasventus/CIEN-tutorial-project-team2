using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// CreateAssetMenu: 커스텀 메뉴를 생성하는 속성
[CreateAssetMenu(fileName = "Item", menuName = "Scriptble Object/ItemData")]
// 스크립터블 오브젝트(Scriptable Object)는 유니티에서 제공하는 대량의 데이터를 저장하고 관리하는데 사용할 수 있는 데이터 컨테이너
public class ItemData : ScriptableObject
{
    public enum ItemType { Melee, Range, MagicCircle, Glove, Shoe, Heal } // (근접 공격, 원거리 공격, 글러브, 신발, 힐 )

    [Header("# Main Info")]
    // 아이템의 각종 속성들을 변수로 작성하기
    public ItemType itemType;
    public int itemId;
    public string itemName;
    [TextArea]
    public string itemDesc;
    public Sprite itemIcon;

    [Header("# Level Data")]
    // 기본 데미지 
    public float baseDamage;
    // 기본 무기 갯수
    public int baseCount;
    // 추가 데미지 값들
    public float[] damages;
    // 추가 되는 무기 갯수
    public int[] counts;
    // 장판 기본 지속 시간
    public float baseMagicCircleWait;
    // 장판 추가 지속 시간
    public float[] magicCircleWaits;

    [Header("# Weapon")]
    public GameObject projectile;

}
