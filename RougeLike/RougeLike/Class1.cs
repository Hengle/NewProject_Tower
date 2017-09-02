using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public enum KeyType//钥匙类型
{
    none,
    Yellow,
    Silver,//银
    Gold,//金
}

public enum PotionType//药水类型
{
    none,
    Addatk,
    Adddfs,
    Addheal,
}

namespace RougeLike
{
    public class Prop
    {
        public string name;
        public int number;
        public int money;
    }

    public class Weapon: Prop//武器
    {
        public int atk;

    }

    public class Armor : Prop//防具
    {
        public int dfs;
    }

    public class Material : Prop//材料
    {

    }

    public class Jewelry : Prop//首饰
    {
        public int atk;
        public int dfs;

    }

    public class Key : Prop//钥匙
    {
        public KeyType type;
    }

    public class Potion : Prop//药水
    {
        public PotionType type;
        public int addAtk;
        public int addDfs;
        public int addHeal;

    }
}
