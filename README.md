# Игра - цивилизации
Вариант 11
Задача: Необходимо реализовать задачу «Игра - цивилизации». В игре участвуют несколько цивилизаций. В каждой представлены индивидуумы нескольких видов: воин, рабочий, аристократ и пр. Каждая раса обладает финансовым запасом и некоторым набором территорий с первоначально размещенными на них объектами типов: леса, поля, жилища и производящие объекты.

Для выполнения даной задачи был использован паттерн Абстрактная фабрика. Данный паттерн есть самым подходящим, поскольку его спецификация полностью совпадает с сутью задачи: 
1) в задании есть семейства взаимосвязанных объектов (каждая Расса/Цивилизация - отдельное такое семейство) с общими характеристиками для всех типов данного объекта (не зависимо, какой у игрока будет Классов героя, начальный набор ресурсов будет зависеть именно от Рассы);
2) система не должна зависеть от способа создания и компоновки новых классов.

Сначала в программе создаются 2 Рассы. После этого игроку дается выбор, персонажа какого Класса создать и к какой Рассе он будет относиться. Таким образом создание Рассы абстрагируется от создания Класса. Класс также создается как отдельная единица и привязывается к определенной Рассе, таким образом приобретая характеристики Рассы.

Паттерн определяет следующих участников (на примере конкретного задания):
1) Абстрактные классы Warrior и Worker определяют интерфейс для всех Классов в игре.
2) Конкретные классы ElfWarrior, ElfWorker, GnomeWarrior и GnomeWorker - конкретная реализация каждого класса определенной рассы.
3) Абстрактный класс фабрики ClassFactory определяет методы для создания Класса. Методы возвращают абстрактные продукты.
4) Конкретные классы фабрик ElfFactory и GnomeFactory реализуют абстрактные методы базового класса и определяют конкретную рассу, которая создается.
5) Класс клиента Client использует класс фабрики для создания объектов. При этом он использует исключительно абстрактный класс фабрики ClassFactory и абстрактные классы продуктов.
