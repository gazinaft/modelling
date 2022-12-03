<h1>Звіт
з лабораторної роботи  № 2 з дисципліни
«Моделювання систем»
</h1>

# «Об’єктно-орієнтований підхід до побудови імітаційних моделей дискретно-подійних систем»

## Виконав: ІП-91 Газін Костянтин

[//]: # ()
[//]: # ()
## Перевірив: Дифучин Антон Юрійович


# Зміст
1) [Завдання](#Завдання)
2) [Виконання](#Виконання)
3) [Висновок](#Висновок)
4) [Додаток 1](#Додаток-1)

# Завдання 
1. Реалізувати алгоритм імітації простої моделі обслуговування одним пристроєм з використанням об’єктно-орієнтованого підходу. 5 балів.
2. Модифікувати алгоритм, додавши обчислення середнього завантаження пристрою. 5 балів.
3. Створити модель за схемою, представленою на рисунку 2.1. 30 балів.
4. Виконати верифікацію моделі, змінюючи значення вхідних змінних та параметрів моделі. Навести результати верифікації у таблиці. 10 балів.

<img src="/run/media/gazinaft/d/7 sem/modelling/lab2/scheme.png">

5. Модифікувати клас `PROCESS`, щоб можна було його використовувати для моделювання процесу обслуговування кількома ідентичними пристроями. 20 балів.
6. Модифікувати клас `PROCESS`, щоб можна було організовувати вихід в два і більше наступних  блоків, в тому числі з поверненням у попередні блоки. 30 балів.

# Виконання

## 1. Об'єктно-орієнтована реалізація
Код був наданий у кінці тексту до лаб. роботи
## 2. Алгоритм обчислення середнього завантаження
Треба було трохи модифікувати код. Додати лічильник занятості, сумувати зайнятість та потім ділити на загальний час
### ```Process.java```
```java
    @Override
    public void inAct() {
        if (super.getState() == 0) {
            super.setState(1);
            var delay = super.getDelay();
            meanBusy += getState() * delay;
            super.setTnext(super.getTcurr() + delay);
        } else {
            if (getQueue() < getMaxqueue()) {
                setQueue(getQueue() + 1);
            } else {
                failure++;
            }
        }
    }
```
### ```Model.java```
```java
    public void printResult() {
        System.out.println("\n-------------RESULTS-------------");
        for (Element e : list) {
            e.printResult();
            if (e instanceof Process) {
                Process p = (Process) e;
                System.out.println("mean length of queue = " +
                        p.getMeanQueue() / tcurr
                        + "\nfailure probability  = " +
                        p.getFailure() / (double) p.getQuantity() +
                        "\nmean business = " + p.getMeanBusy() / tcurr);
            }
        }
    }
```
## 3. Створити модель за схемою
Треба було модифікувати клас `Process.java`, щоб він міг передавати далі інформацію,
та мофифікувати `SimModel.java`, щоб додати нові Process'и.
### `Process.java`
```java
    var nextEl = getNextElement();
    if (nextEl != null) {
        nextEl.inAct();
    }
```
### `SimModel.java`
```java
Create c = new Create(2.0);
        Process p = new Process(1.0);
        Process p2 = new Process(1.0);
        Process p3 = new Process(1.0);

        System.out.println("id0 = " + c.getId() + "   id1=" + p.getId());
        c.setNextElement(p);
        p.setNextElement(p2);
        p2.setNextElement(p3);

        p.setMaxqueue(5);
        p2.setMaxqueue(5);
        p3.setMaxqueue(5);

        c.setName("CREATOR");
        p.setName("PROCESSOR");
        p2.setName("PROCESSOR2");
        p3.setName("PROCESSOR3");

        c.setDistribution("exp");
        p.setDistribution("exp");
        p2.setDistribution("exp");
        p3.setDistribution("exp");

        ArrayList<Element> list = new ArrayList<>();
        list.add(c);
        list.add(p);
        list.add(p2);
        list.add(p3);
```
<img src="/run/media/gazinaft/d/7 sem/modelling/lab2/4-result.png">

## 4. Верифікація моделі

q/f значить queue length/failure probability

| Cr Delay | P1 Delay | P2 Delay | P3 Delay | P1 q/f       | P2 q/f       | P3 q/f      |
|----------|----------|----------|----------|--------------|--------------|-------------|
| 2        | 1        | 1        | 1        | 0.41 / 0.004 | 0.38 / 0.002 | 0.53 / 0.01 |
| 1        | 1        | 1        | 1        | 2.18 / 0.19  | 1.75 / 0.09  | 1.36 / 0.06 |
| 1        | 0.5      | 1        | 1        | 0.58 / 0.01  | 2.18 / 0.18  | 1.61 / 0.08 |
| 0.5      | 1        | 2        | 1        | 3.99 / 0.5   | 3.87 / 0.43  | 0.77 / 0.01 |

### Модель працює коректно, результати зходяться з очікуваними

## 5. Багатоканальне обслуговування
