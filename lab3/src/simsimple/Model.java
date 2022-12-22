package simsimple;

import bank.SwitchingProcess;

import java.util.ArrayList;

public class Model {

    private ArrayList<Element> list = new ArrayList<>();
    double tnext, tcurr;
    int event;

    public Model(ArrayList<Element> elements) {
        list = elements;
        tnext = 0.0;
        event = 0;
        tcurr = tnext;
    }


    public void simulate(double time) {

        while (tcurr < time) {
            tnext = Double.MAX_VALUE;
            for (Element e : list) {
                if (e.getTnext() < tnext) {
                    tnext = e.getTnext();
                    event = e.getId();

                }
            }
            System.out.println("\nIt's time for event in " +
                    list.get(event).getName() +
                    ", time =   " + tnext);
            for (Element e : list) {
                e.doStatistics(tnext - tcurr);
            }
            tcurr = tnext;
            for (Element e : list) {
                e.setTcurr(tcurr);
            }
            list.get(event).outAct();
            for (Element e : list) {
                if (e.getTnext() == tcurr) {
                    e.outAct();
                }
            }
            printInfo();
        }
        printResult();
    }

    public void printInfo() {
        for (Element e : list) {
            e.printInfo();
        }
    }

    public void printResult() {
        System.out.println("\n-------------RESULTS-------------");
        var mean_people = 0;
        var sum_people = 0;
        var failed_people = 0;
        double sum_time_to_go = 0;
        for (Element e : list) {
            e.printResult();
            if (e instanceof Process) {
                Process p = (Process) e;
                System.out.println("mean length of queue = " +
                        p.getMeanQueue() / tcurr
                        + "\nfailure probability  = " +
                        p.getFailure() / (double) (p.getQuantity() + p.getFailure()) +
                        "\nmean business = " + p.getMeanBusy() / tcurr);
            if (e instanceof SwitchingProcess) {
                SwitchingProcess swp = (SwitchingProcess) e;
                mean_people += ((double) swp.getMeanQueue());
                sum_people += swp.getQuantity();
                failed_people += swp.getFailure();
                sum_time_to_go += swp.sum_time_to_go;
            }
            }
        }
        System.out.println("============================");
        System.out.println("Mean quantity of people in bank = " + mean_people / tcurr);
        System.out.println("Mean time of a man in bank = " + sum_people / (double) tcurr);
        System.out.println("Failed percentage = " + failed_people / (double) (failed_people + sum_people));
        System.out.println("Mean interval between clients leaving window = " + sum_time_to_go / (double) sum_people);
    }
}
