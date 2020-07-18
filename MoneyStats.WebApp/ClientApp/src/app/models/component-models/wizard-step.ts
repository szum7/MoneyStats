export class WizardStep {
    public title: string;
    public nextStepFunction: Function;

    constructor(title: string, nextStepFunction: Function){
        this.title = title;
        this.nextStepFunction = nextStepFunction;
    }
}