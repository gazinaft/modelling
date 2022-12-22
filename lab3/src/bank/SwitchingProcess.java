package bank;

import simsimple.Process;

public class SwitchingProcess extends Process {

    public int priority;
    private int switchedCount;
    public double sum_time_to_go;
    private SwitchingProcess other;

    public SwitchingProcess(double delay, int channels_max, int priority) {
        super(delay, channels_max);
        this.priority = priority;

        switchedCount = 0;
    }

    public void initPair(SwitchingProcess other) {
        this.other = other;
        other.other = this;
    }

    @Override
    public void putMinimal(double next) {
        sum_time_to_go += next - getTcurr();
        super.putMinimal(next);
    }

    @Override
    public void outAct() {
        super.outAct();
        switchFromAnother();
    }

    // use when process done with a car
    public void switchFromAnother() {
        if (other.queue - this.queue >= 2) {
            this.queue++;
            other.queue--;
            switchedCount++;
        }
    }

    @Override
    public void printInfo() {
        super.printInfo();
        System.out.println("Switched to this line = " + switchedCount);
    }
}
