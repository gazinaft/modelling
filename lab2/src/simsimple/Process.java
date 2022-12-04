package simsimple;

import java.util.ArrayList;
import java.util.Comparator;
import java.util.Deque;
import java.util.Queue;

public class Process extends Element {

    private int queue, maxqueue, failure;
    private double meanQueue;

    private double meanBusy;

    private int channel_max;
    private int channel_current;

    private ArrayList<Double> tNexts;

    public Process(double delay) {
        this(delay, 1);
    }

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

    public int getFailure() {
        return failure;
    }

    public int getQueue() {
        return queue;
    }


    public void setQueue(int queue) {
        this.queue = queue;
    }


    public int getMaxqueue() {
        return maxqueue;
    }


    public void setMaxqueue(int maxqueue) {
        this.maxqueue = maxqueue;
    }

    @Override
    public void printInfo() {
        super.printInfo();
        System.out.println("failure = " + this.getFailure());
    }

    @Override
    public void doStatistics(double delta) {
        meanQueue = getMeanQueue() + queue * delta;
        meanBusy += getState() * delta;
    }

    public double getMeanQueue() {
        return meanQueue;
    }

    public double getMeanBusy() { return meanBusy; }
}
