package simsimple;

public class Create extends Element {

    public Create(double delay) {
        super(delay);
        super.setTnext(0.0);
    }

    @Override
    public void outAct() {
        super.outAct();
        super.setTnext(super.getTcurr() + super.getDelay());
        super.getNextElement().inAct();
    }
}
