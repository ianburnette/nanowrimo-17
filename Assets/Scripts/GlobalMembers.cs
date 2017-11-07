using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public enum BlockType
{
    none,
    fire,
    ice,
    ghost,
    crate,
    spirit,
    water,
    wood
}
public struct Block
{
    public BlockType type;
    public GameObject myGameObject;
}

public class GlobalMembers : MonoBehaviour {
    public static BlockType SelectRandomType()
    {
        print("selecting random type");
        BlockType type = BlockType.none;
        while (type == BlockType.none)
            type = GetRandomEnum<BlockType>();
        return type;
    }
    static T GetRandomEnum<T>()
    {
        System.Array A = System.Enum.GetValues(typeof(T));
        T V = (T)A.GetValue(UnityEngine.Random.Range(0, A.Length));
        return V;
    }
}
