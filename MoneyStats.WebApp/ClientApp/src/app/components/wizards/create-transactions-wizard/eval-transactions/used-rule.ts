import { Rule } from 'src/app/models/service-models/rule.model';

export class UsedRule {
    isExcluded: boolean;
    value: Rule;

    constructor(value: Rule) {
        this.value = value;
        this.isExcluded = false;
    }

    public toggleExclusion(): void {
        this.isExcluded = !this.isExcluded;
    }
}
