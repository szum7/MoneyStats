export class CompareResults {

    isSameContent: boolean;

    get isNew(): boolean { return !this.isSameContent }

    constructor() {
        this.isSameContent = false;
    }
}
