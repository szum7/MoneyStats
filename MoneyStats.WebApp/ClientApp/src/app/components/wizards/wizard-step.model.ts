export abstract class WizardStep {
    protected abstract checkForAlerts(): void;
    public abstract previous(): void;
    public abstract next(): void;
}