import { Component, OnInit, Input, Output, EventEmitter } from '@angular/core';

enum StepAlertType {
    Criteria,
    Message,
    GreenText
}

class StepAlert {
    title: string;
    type: StepAlertType;

    constructor(title: string) {
        this.title = title;
    }

    public setToCriteria() {
        this.type = StepAlertType.Criteria;
        return this;
    }

    public setToMessage() {
        this.type = StepAlertType.Message;
        return this;
    }

    public setToGreenText() {
        this.type = StepAlertType.GreenText;
        return this;
    }
}

export class WizardStep {

    public title: string;
    public description: string[];
    public stepAlerts: StepAlert[];

    public get isProgressable(): boolean {
        if (!this.stepAlerts) {
            return true;
        }
        if (this.stepAlerts.some(x => x.type == StepAlertType.Criteria)) {
            return false;
        }
        return true;
    }

    constructor(title: string, description: string[]) {
        this.title = title;
        this.description = description;
        this.stepAlerts = [];
    }
}

export class Wizard {

    public hoverStepsAt: number;
    private _stepsAt: number;
    private _steps: WizardStep[];

    public get stepsAt(): number { return this._stepsAt; }
    public get steps(): WizardStep[] { return this._steps; }

    public get currentStep(): WizardStep {
        if (this._stepsAt < 0) {
            return null;
        }
        return this._steps[this._stepsAt];
    }

    public get hoverStep(): WizardStep {
        return this._steps[this.hoverStepsAt];
    }

    constructor(steps: WizardStep[]) {
        if (steps != null && steps.length > 0) {
            this._steps = steps;
            this.reset();
        } else {
            console.error("Wizard was initialized with no steps!");
        }
    }

    public isProgressable(): boolean {
        return this.currentStep ? this.currentStep.isProgressable : false;
    }

    public reset(): void {
        this._stepsAt = 0;
    }

    public next(): boolean {
        if (this._stepsAt == this._steps.length - 1)
            return false;

        if (!this.isProgressable())
            return false;

        this._stepsAt++;
        return true;
    }

    public previous(): boolean {
        if (this._stepsAt <= 0)
            return false;

        this._stepsAt--;

        return true;
    }

    public clearAlerts(): void {
        this.currentStep.stepAlerts = [];
    }

    public addCriteria(title: string): void {
        this.currentStep.stepAlerts.push(new StepAlert(title).setToCriteria());
    }

    public addMessage(title: string): void {
        this.currentStep.stepAlerts.push(new StepAlert(title).setToMessage());
    }

    public addGreenText(title: string): void {
        this.currentStep.stepAlerts.push(new StepAlert(title).setToGreenText());
    }
}

@Component({
  selector: 'app-wizard-navigator-component',
  templateUrl: './wizard-navigator.component.html',
  styleUrls: ['./wizard-navigator.component.scss']
})
export class WizardNavigatorComponent implements OnInit {

    @Input() wizard: Wizard;
    @Input() stepsAt: number;
    
    private isHoverOn: boolean = false;

    constructor() { 
    }

    ngOnInit() {
    }

    get lineLength(): string {
        return (100 / (this.wizard.steps.length - 1)) + "%";
    }

    get arrowLeftPosition(): string {
        return this.isHoverOn ? 
        (this.wizard.hoverStepsAt * (100 / (this.wizard.steps.length - 1))) + "%" : 
        (this.wizard.stepsAt * (100 / (this.wizard.steps.length - 1))) + "%";
    }

    hoverToStep(i: number): void {
        this.wizard.hoverStepsAt = i;
        this.isHoverOn = true;
    }

    hoverOutOfStep(): void {
        this.isHoverOn = false;
    }

    get infoBoxStep(): WizardStep {
        return this.isHoverOn ? this.wizard.hoverStep : this.wizard.currentStep;
    }

    /* Test area #1
    addError(){
        this.wizard.currentStep.stepAlerts.push(new StepAlert("Problem").setToCriteria());
    }
    removeError(){
        this.wizard.currentStep.stepAlerts = [];
    }
    sout() {
        console.log(this.wizard);
    }
    */
}
