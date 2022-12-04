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
6. Модифікувати клас `PROCESS`, щоб можна було організовувати вихід в два і більше наступних блоків, в тому числі з поверненням у попередні блоки. 30 балів.

# Виконання

## 1. Об'єктно-орієнтована реалізація
Код був наданий у кінці тексту до лаб. роботи
## 2. Алгоритм обчислення середнього завантаження
Треба було трохи модифікувати код. Додати лічильник занятості, сумувати зайнятість та потім ділити на загальний час
### ```Process.java```
```java
    public void doStatistics(double delta) {
        meanQueue = getMeanQueue() + queue * delta;
        meanBusy += getState() * delta;
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
Для цього я додав підрахунок зайнятих каналів та список з часом завершення
обробки різних каналів.
### `Process.java`
```java
    ..................

    public Process(double delay, int channels_max) {
        super(delay);
        setTcurr(0.0);
        setTnext(Double.MAX_VALUE);
        tNexts = new ArrayList<>();
        queue = 0;
        maxqueue = Integer.MAX_VALUE;
        meanQueue = 0.0;
        meanBusy = 0.0;
        channel_max = channels_max;
        channel_current = 0;
    }

    @Override
    public void inAct() {
        if (inChannel()) {
            var delay = super.getDelay();
            meanBusy += getState() * delay;
            putMinimal(super.getTcurr() + delay);
        } else {
            if (getQueue() < getMaxqueue()) {
            setQueue(getQueue() + 1);
            } else {
                failure++;
            }
        }
    }

    @Override
    public void outAct() {
        super.outAct();
        removeTnext();
        outChannel();

        var nextEl = getNextElement();
        if (nextEl != null) {
            nextEl.inAct();
        }

        if (getQueue() > 0) {
            setQueue(getQueue() - 1);
            inChannel();
            var delay= super.getDelay();
            meanBusy += delay * getState();
            putMinimal(super.getTcurr() + delay);
        }
    }


    @Override
    public int getState() {
        return channel_current != 0 ? 1 : 0;
    }

    private boolean inChannel() {
        if (channel_current == channel_max) {
            return false;
        }
        channel_current++;
        super.setState(1);
        return true;
    }

    private boolean outChannel() {
        if (channel_current < 1) {
            super.setState(0);
            return true;
        }
        if (channel_current == 1) {
            channel_current--;
            super.setState(0);
            return true;
        }
        channel_current--;
        super.setState(1);
        return false;
    }

    public void putMinimal(double next) {
        tNexts.removeIf(x -> x < getTcurr());

        tNexts.add(next);
        tNexts.sort(Comparator.naturalOrder());
    }

    @Override
    public double getTnext() {
        if (tNexts.size() == 0) return Double.MAX_VALUE;
        return tNexts.get(0);
    }

    public void removeTnext() {
        tNexts.remove(getTnext());
    }
        ......................
```
## 6. Вихід у 2+ наступних блоки
Я модифікував метод `getNextElement` у класі `Process.java`
### `Process.java`
```java
    @Override
    public Element getNextElement() {
        var sumOfProbs = nextElementProbabilities.stream().reduce(Double::sum);
        if (sumOfProbs.isEmpty()) {
            return null;
        }
        if (sumOfProbs.get() > 1) {
            throw new RuntimeException("Probabilities not matching");
        }
        if (nextElements.size() == 1) {
            return nextElements.get(0);
        }

        var rand = rng.nextDouble();
        var acc = 0.0;
        for (int i = 0; i < nextElementProbabilities.size(); i++) {
            acc += nextElementProbabilities.get(i);
            if (rand <= acc) {
                return nextElements.get(i);
            }
        }
        return null;
    }
```

# Висновок
Ми дослідили системи масового обслуговування, методи їх роботи та збір статистики.
За допомогою них ми можемо моделювати системи реального світу достатньо точно.
І лише за допомогою 2х елементів: продюсера та СМО ми можемо достатньо гнучко
налаштувати систему