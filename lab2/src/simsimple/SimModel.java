package simsimple;


import java.util.ArrayList;

public class SimModel {

    public static void main(String[] args) {
        Create c = new Create(0.5);
        Process p = new Process(1.0, 2);
        Process p2 = new Process(2.0, 4);
        Process p2alt = new Process(5.0, 1);
        Process p3 = new Process(1.0);

        System.out.println("id0 = " + c.getId() + "   id1=" + p.getId());
        c.setNextElement(p);
        p.setNextElement(p2, 0.7);
        p.setNextElement(p2alt, 0.3);
        p2.setNextElement(p3);
        p2alt.setNextElement(p3, 0.1);

        p.setMaxqueue(5);
        p2.setMaxqueue(5);
        p2alt.setMaxqueue(5);
        p3.setMaxqueue(5);

        c.setName("CREATOR");
        p.setName("PROCESSOR");
        p2.setName("PROCESSOR2");
        p2alt.setName("PROCESSOR2_ALTERNATIVE");
        p3.setName("PROCESSOR3");

        c.setDistribution("exp");
        p.setDistribution("exp");
        p2.setDistribution("exp");
        p2alt.setDistribution("exp");
        p3.setDistribution("exp");

        ArrayList<Element> list = new ArrayList<>();
        list.add(c);
        list.add(p);
        list.add(p2);
        list.add(p2alt);
        list.add(p3);

        Model model = new Model(list);
        model.simulate(1000.0);

    }
}