import { StepAlertType } from './step-alert-type.enum';

export class StepAlert {
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
