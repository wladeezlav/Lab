
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab1_Mamontov
{    
    class Program
    {
        static void Main(string[] args)
        {
            int n;
            Console.WriteLine("Лабораторная работа №1\nСтудента группы IS-63\nМамонтова Владислава Викторовича\n\nИгра 'Цивилизация'\n\nВ игре участвуют 2 рассы:\n");
            Client elf = new Client(new ElfFactory());
            Client gnome = new Client(new GnomeFactory());
            Console.WriteLine("У каждой рассы есть 2 класса - Воин и Рабочий.\n1) Создать Воина рассы Эльф;\n2) Создать Рабочего рассы Эльф;\n3) Создать Воина рассы Гном;\n4) Создать Рабочего класса гном.\n");
            do
            {
                n = Convert.ToInt32(Console.ReadLine());
                switch (n)
                {
                    case 1:
                        elf.ChooseWarrior();
                        break;
                    case 2:
                        elf.ChooseWorker();
                        break;
                    case 3:
                        gnome.ChooseWarrior();
                        break;
                    case 4:
                        gnome.ChooseWorker();
                        break;
                    default:
                        Console.WriteLine("Нажмите 0, чтобы выйти");
                        break;
                }
            } while (n != 0);

                Console.ReadLine();
        }

        abstract class Warrior  //абстрактный продукт - класс Воин
        {
            public abstract void WarriorChar();
        }

        abstract class Worker   //абстрактный продукт - класс Рабочий
        {
            public abstract void WorkerChar();
        }

        class ElfWarrior : Warrior  //класс конкретного продукта
        {
            public override void WarriorChar()
            {
                Console.WriteLine("Воин Эльф создан!");
                //характеристики для воина конкретной рассы
            }
        }

        class ElfWorker : Worker    //класс конкретного продукта
        {
            public override void WorkerChar()
            {
                Console.WriteLine("Работник Эльф создан!");
                //характеристики для работника конкретной рассы
            }
        }
        class GnomeWarrior : Warrior  //класс конкретного продукта
        {
            public override void WarriorChar()
            {
                Console.WriteLine("Воин Гном создан!");
                //характеристики для воина конкретной рассы
            }
        }

        class GnomeWorker : Worker    //класс конкретного продукта
        {
            public override void WorkerChar()
            {
                Console.WriteLine("Работник Гном создан!");
                //характеристики для работника конкретной рассы
            }
        }

        abstract class ClassFactory  //абстрактная фабрика
        {
            public abstract Warrior CreateWarrior();
            public abstract Worker CreateWorker();
        }

        class ElfFactory : ClassFactory    //конкретная фабрика рассы Эльфы
        {   //Ресурсы рассы:
            private int territory = 1000;   //территория - в квадратных игоровых единицах
            private int finance = 10000;    //финансовые запасы рассы - игрового золота
            private int forest = 200;       //территория лесов - в квардатных игровых единицах
            private int fields = 500;       //территория полей - в квадратных игровых единицах
            private int objects = 50;       //территория производственных объектов - в квадратных игровых единицах
            private int habitation = 250;   //территория жилищ - в квадратных игровых единицах

            public ElfFactory()
            {
                Console.WriteLine("Расса Эльфов добавлена в игру!");
                Console.WriteLine("Расса обладает такими ресурсами:\nterritory = 1000;\nfinance = 10000;\nforest = 200;\nfields = 500;\nobjects = 50;\nhabitation = 250;");
            }

            public override Warrior CreateWarrior()
            {
                return new ElfWarrior();
            }
            public override Worker CreateWorker()
            {
                return new ElfWorker();
            }
        }

        class GnomeFactory : ClassFactory   //конкретная фабрика рассы Гномы
        {   //Ресурсы рассы:
            private int territory = 1200;   //территория - в квадратных игоровых единицах
            private int finance = 8000;    //финансовые запасы рассы - игрового золота
            private int forest = 600;       //территория лесов - в квардатных игровых единицах
            private int fields = 200;       //территория полей - в квадратных игровых единицах
            private int objects = 100;       //территория производственных объектов - в квадратных игровых единицах
            private int habitation = 300;   //территория жилищ - в квадратных игровых единицах
            public GnomeFactory()
            {
                Console.WriteLine("\nРасса Гномов добавлена в игру!");
                Console.WriteLine("Расса обладает такими ресурсами:\nterritory = 1200;\nfinance = 8000;\nforest = 600;\nfields = 200;\nobjects = 100;\nhabitation = 300;\n");
            }
            public override Warrior CreateWarrior()
            {
                return new GnomeWarrior();
            }
            public override Worker CreateWorker()
            {
                return new GnomeWorker();
            }
        }

        class Client                //клиент
        {
            private Worker worker;
            private Warrior warrior;
            public Client(ClassFactory factory)
            {
                worker = factory.CreateWorker();
                warrior = factory.CreateWarrior();
            }
            public void ChooseWorker()
            {
                worker.WorkerChar();
            }
            public void ChooseWarrior()
            {
                warrior.WarriorChar();
            }
        }
    }

}


