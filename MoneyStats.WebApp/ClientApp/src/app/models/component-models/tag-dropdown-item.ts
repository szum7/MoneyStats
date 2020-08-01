import { Tag } from "../service-models/tag.model";

export class TagDropdownItem {
    str: string;
    results: Tag[];

    constructor() {
        this.results = [];
    }

    public resetResults(): void {
        this.results = [];
    }

    public removeFromResults(tag: Tag): void {
        const index = this.results.indexOf(tag);
        if (index > -1) {
            this.results.splice(index, 1);
        }
    }
}