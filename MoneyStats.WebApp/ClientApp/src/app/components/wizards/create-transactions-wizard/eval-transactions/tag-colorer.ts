
export class TagColorer {

    private colors: string[];
    private colorCounter: number;

    constructor() {
        this.colors = ["#7ea4e0", "#ace09b", "#e0c99b"];
        //this.colors = ["#e8e8e8", "#d6d6d6"];
        this.colorCounter = 0;
    }

    getColor(): string {
        if (this.colorCounter > this.colors.length - 1) {
            this.colorCounter = 0;
        }
        return this.colors[this.colorCounter++];
    }
}
