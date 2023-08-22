namespace ConsoleApp3
{
    using System.ComponentModel.DataAnnotations; // 이 시스템들은 뭐임?
    using System.Numerics;
    using System.Reflection.PortableExecutable;

    public class Character
    {
        public string Name { get; set; }
        public string Job { get; set; }
        public int Level { get; set; }
        public float Atk { get; set; }
        public int Def { get; set; }
        public int Hp { get; set; }
        public int Gold { get; set; }

        public Character(string name, string job, int level, int atk, int def, int hp, int gold)
        {
            Name = name;
            Job = job;
            Level = level;
            Atk = atk;
            Def = def;
            Hp = hp;
            Gold = gold;
        }
    }

    public class Equipment // 장비
    {
        public int SellGold { get; set; }
        public int EquipPart { get; set; }
        public bool Isequip { get; set; }
        public bool Ishave { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }

        public Equipment(int sellGold, int equipPart, bool isequip, bool ishave, string description, string name)
        {
            Ishave = ishave;
            SellGold = sellGold;
            EquipPart = equipPart;
            Isequip = isequip;
            Description = description;
            Name = name;
        }

        public virtual void DisplayEquipment(bool shop, bool isSell) // 이 가상메서드는 뭐지?
        {
        }

        public virtual int GetNumber()
        {
            return 0;
        }
        public void setLayout(int length, int remain, string str)
        {
            for (int i = 0; i < length; i++)
            {
                if (i == remain)
                {
                    Console.Write($"{str}");
                }
                else
                {
                    Console.Write(" ");
                }
            }
        }
    }

    public class Weapon : Equipment // 무기정보
    {
        public int Attack { get; }
        public Weapon(int sellGold, int equipPart, bool isequip, bool ishave, string description, string name, int attack) : base(sellGold, equipPart, isequip, ishave, description, name)
        {
            Attack = attack;
        }

        public override void DisplayEquipment(bool shop, bool isSell)
        {
            string Equip = (Isequip) ? "[E]" : "[X]";
            Equip = shop ? "" : Equip;
            int NameLengthRemain = (10 - Name.Length) / 2;
            int AttackLengthRemain = (4 - Attack.ToString().Length) / 2;
            int DescriptionRemain = (30 - Description.ToString().Length) / 2;

            setLayout(10 - Name.Length, NameLengthRemain, Equip + Name);
            Console.Write("|");
            setLayout(4 - Attack.ToString().Length, AttackLengthRemain, $"공격력 : +{Attack}");
            Console.Write("|");
            setLayout(30 - Description.ToString().Length, DescriptionRemain, Description);
            Console.WriteLine();
        }

        public override int GetNumber()
        {
            return Attack;
        }
    }

    public class Armor : Equipment // 방어구 정보
    {
        public int Protect { get; }
        public Armor(int sellGold, int equipPart, bool isequip, bool ishave, string description, string name, int protect) : base(sellGold, equipPart, isequip, ishave, description, name)
        {
            Protect = protect;
        }

        public override void DisplayEquipment(bool shop, bool isSell)
        {
            string Equip = (Isequip) ? "[E]" : "[X]";
            int NameLengthRemain = (10 - Name.Length) / 2;
            int ProtectLengthRemain = (4 - Protect.ToString().Length) / 2;
            int DescriptionRemain = (30 - Description.ToString().Length) / 2;

            setLayout(10 - Name.Length, NameLengthRemain, Equip + Name);
            Console.Write("|");
            setLayout(4 - Protect.ToString().Length, ProtectLengthRemain, $"방어력 : +{Protect}");
            Console.Write("|");
            setLayout(30 - Description.ToString().Length, DescriptionRemain, Description);
            Console.WriteLine();
        }

        public override int GetNumber()
        {
            return Protect;
        }
    }

    internal class Program
    {
        private static Character player;
        private static List<Equipment> equipments = new List<Equipment>();

        static int CheckValidInput(int min, int max) // 원하는 숫자기 아닐 때 오류 메세지 보내는 코드
        {
            while (true)
            {
                string input = Console.ReadLine();

                bool parseSuccess = int.TryParse(input, out var ret);
                if (parseSuccess)
                {
                    if (ret >= min && ret <= max)
                        return ret;
                }

                Console.WriteLine("잘못된 입력입니다.");
            }

        }

        static void EquipmentDataSetting() // 장비탬 설명
        {
            Armor ironArmor = new Armor(500, 1, true, true, "무쇠로 만들어져 튼튼한 갑옷입니다.", "무쇠갑옷", 5);
            Weapon oldSword = new Weapon(600, 0, false, true, "쉽게 볼 수 있는 낡은 검입니다.", "낡은 검", 2);

            equipments.Add(oldSword);
            equipments.Add(ironArmor);

        }

        static int Checknum(int equippart)
        {
            int num = 0;
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].Isequip && equipments[i].EquipPart == equippart)
                {
                    num += equipments[i].GetNumber();
                }
            }
            return num;
        }

        static void GameDataSetting()
        {
            player = new Character("Chad", "전사", 1, 10 + Checknum(0), 5 + Checknum(1), 100, 1500);
        }
        static void EquipmentPlayerDataSetting(int num, int equippart, bool istake)
        {
            //공격력
            if (equippart == 0)
            {
                if (istake)
                    player.Atk += num;
                else
                    player.Atk -= num;
            }
            //방어력
            else
            {
                if (istake)
                    player.Def += num;
                else
                    player.Def -= num;
            }
        }

        static void Main(string[] args)
        {
            EquipmentDataSetting();
            GameDataSetting();
            DisplayGameIntro();
        }

        static void DisplayGameIntro()
        {
            Console.Clear();

            Console.WriteLine("스파르타 마을에 오신 여러분 환영합니다.");
            Console.WriteLine("이곳에서 던전으로 들어가기 전 활동을 할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("1. 상태보기");
            Console.WriteLine("2. 인벤토리");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            int input = CheckValidInput(1, 2);
            switch (input)
            {
                case 1:
                    DisplayMyInfo();
                    break;
                case 2:
                    DisplayInventory();
                    break;
            }
        }

        static void DisplayMyInfo()  //상태보기
        {
            Console.Clear();

            Console.WriteLine("상태보기");
            Console.WriteLine("캐릭터의 정보를 표시합니다.");
            Console.WriteLine();
            Console.WriteLine($"Lv.{player.Level}");
            Console.WriteLine($"{player.Name}({player.Job})");
            int plusAtk = Checknum(0);
            int plusDef = Checknum(1);
            Console.Write($"공격력 :{player.Atk} ");
            if (plusAtk == 0)
            {
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine($"({Checknum(0)})");
            }
            Console.Write($"방어력 : {player.Def} ");

            if (plusDef == 0)
            {
                Console.WriteLine("");
            }
            else
            {
                Console.WriteLine($"({Checknum(1)})");
            }
            Console.WriteLine($"체력 : {player.Hp}");
            Console.WriteLine($"Gold : {player.Gold} G");
            Console.WriteLine();
            Console.WriteLine("0. 나가기");

            int input = CheckValidInput(0, 0);
            switch (input)
            {
                case 0:
                    DisplayGameIntro();
                    break;
            }
        }

        static void DisplayInventory() //인벤토리
        {
            Console.Clear();
            Console.WriteLine("인벤토리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].Ishave)
                {
                    Console.Write($"- ");
                    equipments[i].DisplayEquipment(false, false);
                }
            }
            Console.WriteLine();
            Console.WriteLine("1. 장착 관리");
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");
            int input = CheckValidInput(0, 1);

            switch (input)
            {
                case 0:
                    DisplayGameIntro();
                    break;
                case 1:
                    InvenEquipManagement();
                    break;
            }
        }

        static void Sort(int num) //--이해가 안감
        {
            for (int i = 0; i < equipments.Count - 1; i++)
            {
                int minIndex = i;
                if (num == 2 && equipments[i].Isequip)
                    continue;

                for (int j = i + 1; j < equipments.Count; j++)
                {
                    if (num == 1)
                    {
                        if (equipments[j].Name.Length > equipments[minIndex].Name.Length)
                        {
                            minIndex = j;
                        }
                    }
                    else if (num == 2)
                    {
                        if (equipments[j].Isequip && !equipments[minIndex].Isequip)
                        {
                            minIndex = j;
                        }
                    }
                    else if (num == 3)
                    {
                        if (equipments[j].GetNumber() > equipments[minIndex].GetNumber())
                        {
                            minIndex = j;
                        }
                    }
                }

                Equipment temp = equipments[i];
                equipments[i] = equipments[minIndex];
                equipments[minIndex] = temp;
            }
        }

        static void InvenEquipManagement()
        {
            Console.Clear();
            Console.WriteLine("인벤토리 - 장착 관리");
            Console.WriteLine("보유 중인 아이템을 관리할 수 있습니다.");
            Console.WriteLine();
            Console.WriteLine("[아이템 목록]");
            Dictionary<int, int> dic = new Dictionary<int, int>(); //------ 이해가 안감
            int num = 1;
            for (int i = 0; i < equipments.Count; i++)
            {
                if (equipments[i].Ishave)
                {
                    Console.Write($"- {num}. ");
                    equipments[i].DisplayEquipment(false, false);
                    dic.Add(num, i);
                    num++;
                }
            }
            Console.WriteLine();
            Console.WriteLine("0. 나가기");
            Console.WriteLine();
            Console.WriteLine("원하시는 행동을 입력해주세요.");
            Console.Write(">> ");

            // 밑 코드는 무슨말인지 이해가 안감.

            int input = CheckValidInput(0, num);

            if (input == 0)
            {
                DisplayInventory();
            }
            else
            {
                if (!equipments[dic[input]].Isequip)
                {
                    for (int i = 0; i < equipments.Count; i++)
                    {
                        if (equipments[dic[input]].EquipPart == equipments[i].EquipPart && equipments[i].Isequip)
                        {
                            EquipmentPlayerDataSetting(equipments[i].GetNumber(), equipments[i].EquipPart, !equipments[i].Isequip);
                            equipments[i].Isequip = false;
                        }
                    }
                }
                equipments[dic[input]].Isequip = equipments[dic[input]].Isequip ? false : true;
                EquipmentPlayerDataSetting(equipments[dic[input]].GetNumber(), equipments[dic[input]].EquipPart, equipments[dic[input]].Isequip);
                InvenEquipManagement();
            }

        }

    }

}