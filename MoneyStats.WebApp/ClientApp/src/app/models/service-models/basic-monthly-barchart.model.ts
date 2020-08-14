import { BasicMonthlyBar } from "./basic-monthly-bar.model";

export class BasicMonthlyBarchart {

    from: Date;
    to: Date;
    bars: BasicMonthlyBar[];
    maxValue: number;
    
    constructor() {
        this.bars = [];
    }
}