import { Component, Input, Output, OnInit } from '@angular/core';
import { StatisticsService } from 'src/app/services/statistics.service';
import { BasicMonthlyBarchart } from 'src/app/models/service-models/basic-monthly-barchart.model';
import { Common } from 'src/app/utilities/common.static';
import { Transaction } from 'src/app/models/service-models/transaction.model';
import { BasicMonthlyBar } from 'src/app/models/service-models/basic-monthly-bar.model';


@Component({
    selector: 'app-basic-monthly-barchart-component',
    templateUrl: './basic-monthly-barchart.component.html',
    styleUrls: ['./basic-monthly-barchart.component.scss']
})
export class BasicMonthlyBarchartComponent implements OnInit {

    data: BasicMonthlyBarchart;
    transactionList: Transaction[];
    barWidth: number = 60;
    maxHeight: number = 300;
    yUnitGap: number;
    yUnits: number[];
    xUnitsHeight: number = 30;
    xUnits: string[];

    constructor(private statisticsService: StatisticsService) {
        // TODO tesztelni, hogy 0-0-0 esetén kihagyja-e az oszlop szélességét
        // TODO minden hónapra szélességet állítani
        this.yUnits = [];
        this.xUnits = [];
        this.yUnitGap = 0;
    }

    ngOnInit(): void {
        let self = this;
        self.getBasicMonthlyBarchart(new Date(2010, 0, 1), new Date(2020, 2, 20), res => {
            self.data = res;
            self.initProperties();
        });
    }

    private initProperties() {
        let unit: number = this.calculateYUnits(this.data.maxValue);
        let unitTimes: number = this.data.maxValue / unit;
        this.yUnitGap = (this.maxHeight / unitTimes) * (100 / this.maxHeight);
        for (let i = 1; i < unitTimes - 1; i++) {
            this.yUnits.push(i * unit);
        }
    }

    getXUnitsWidth(): string {
        if (this.data == null)
            return "0px";
        return (this.data.bars.length * this.barWidth) + "px";
    }

    getBarLeftPosition(i: number): string {
        return (this.barWidth * i) + "px";
    }

    getBarHeight(value: number): string {
        let ratio = this.maxHeight / this.data.maxValue;
        return Math.max(Math.abs(value) * ratio, 2) + "px";
    }

    showTransactionList(item: BasicMonthlyBar): void {
        this.transactionList = item.transactions;
    }

    getDateLiteralX(dateString: string): string {
        return new Date(dateString).toDateString();
    }

    getDateLiteral(dateString: string): Date {
        return new Date(dateString);
    }

    private getBasicMonthlyBarchart(from: Date, to: Date, callback: (response: BasicMonthlyBarchart) => void): void {
        this.statisticsService.getBasicMonthlyBarchart(from, to).subscribe(res => {
            Common.ConsoleResponse("getBasicMonthlyBarchart", res);
            callback(res);
        }, err => {
            console.log(err);
        });
    }

    private calculateYUnits(max: number): number {
        /* Unit  |  Max
         * 1     |  0 - 50
         * 5     |  50 - 100
         * 10    |  100 - 500
         * 50    |  500 - 1000
         * 100   |  1000 - 5000
         * ...
         */
        let ret: number = 1;
        let cap: number = 50; // starting cap
        let c: number = 2;
        while (max > cap) {
            if (c % 2 === 0) {
                ret *= 5;
            } else {
                ret *= 2;
            }
            if (c % 2 === 0) {
                cap *= 2;
            } else {
                cap *= 5;
            }
            c++;
        }
        return ret;
    }
}
