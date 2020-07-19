import { StepAlertType } from './step-alert-type.enum';
import { StepAlert } from './step-alert.model';

export abstract class WizardStep {


    public title: string;
    public stepAlerts: StepAlert[];
    public $output: any;


    public get isProgressable(): boolean {
        if (!this.stepAlerts) {
            return true;
        }
        if (this.stepAlerts.some(x => x.type == StepAlertType.Criteria)) {
            return false;
        }
        return true;
    }

    constructor(title: string) {
        this.title = title;
        this.stepAlerts = [];
    }


    abstract getOutput(): any;
    abstract setInput($input: any): void;
}
