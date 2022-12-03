package simsimple;


import java.util.ArrayList;

public class SimModel {

    public static void main(String[] args) {
        Create c = new Create(0.5);
        Process p = new Process(1.0);
        Process p2 = new Process(2.0);
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

        Model model = new Model(list);
        model.simulate(1000.0);

    }
}