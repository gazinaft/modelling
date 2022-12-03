package simsimple;

import java.util.Queue;

public class Process extends Element {

    private int queue, maxqueue, failure;
    private double meanQueue;

    private double meanBusy;

    private int channel_max;
    private int channel_current;

    private Queue<Double> tNexts;

    public Process(double delay) {
        this(delay, 1);
    }

    public Process(double delay, int channels_max) {
        super(delay);
        setTcurr(0.0);
        setTnext(Double.MAX_VALUE);
        queue = 0;
        maxqueue = Integer.MAX_VALUE;
        meanQueue = 0.0;
        meanBusy = 0.0;
        channel_max = channels_max;
        channel_current = 0;
    }

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

    @Override
    public void outAct() {
        super.outAct();
        super.setTnext(Double.MAX_VALUE);
        super.setState(0);
        var nextEl = getNextElement();
        if (nextEl != null) {
            nextEl.inAct();
        }

        if (getQueue() > 0) {
            setQueue(getQueue() - 1);
            super.setState(1);
            super.setTnext(super.getTcurr() + super.getDelay());
        }
    }

//    @Override
//    public int getState() {
//        return channel_current != 0 ? 1 : 0;
//    }

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
        return false;
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
    }

    public double getMeanQueue() {
        return meanQueue;
    }

    public double getMeanBusy() { return meanBusy; }
}
