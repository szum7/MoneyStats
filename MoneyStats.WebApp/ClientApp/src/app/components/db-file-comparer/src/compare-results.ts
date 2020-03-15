export class CompareResults {

    isSameContent: boolean;
    isSameOriginalContent: boolean;

    get isNew(): boolean { return !this.isSameContent && !this.isSameOriginalContent; }

    constructor() {
        this.isSameContent = false;
        this.isSameOriginalContent = false;
    }
}
