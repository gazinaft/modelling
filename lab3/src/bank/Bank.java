package bank;

import simsimple.Create;
import simsimple.Element;
import simsimple.Model;
import simsimple.Process;

import java.util.ArrayList;

public class Bank {
    public static void main(String[] args) {

        var create = new Create(0.5);
        var broker = new Process(0.01);
        var sw1 = new SwitchingProcess(1, 1, 0);
        var sw2 = new SwitchingProcess(1, 1, 1);

        broker.setMaxqueue(1000);
        sw1.setMaxqueue(3);
        sw2.setMaxqueue(3);
        sw1.setDelayDev(0.3);
        sw2.setDelayDev(0.3);


        create.setDistribution("exp");
        sw1.setDistribution("norm");
        sw2.setDistribution("norm");
        sw1.initPair(sw2);

        create.setName("CREATE");
        broker.setName("BROKER");
        sw1.setName("CASHIER1");
        sw2.setName("CASHIER2");

        create.setNextElement(broker);
        broker.setNextElement(sw1);
        broker.setNextElement(sw2);

        broker.setNextToCustom();

        ArrayList<Element> list = new ArrayList<>();
        list.add(create);
        list.add(broker);
        list.add(sw1);
        list.add(sw2);

        sw1.queue = 2;
        sw2.queue = 2;
        broker.putMinimal(0.1);


        Model model = new Model(list);
        model.simulate(1000.0);


    }
}