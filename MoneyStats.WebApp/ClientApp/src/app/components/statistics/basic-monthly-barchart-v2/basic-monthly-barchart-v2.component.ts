import { Component, Input, Output, OnInit } from '@angular/core';
import { StatisticsService } from 'src/app/services/statistics.service';
import { BasicMonthlyBarchart } from 'src/app/models/service-models/basic-monthly-barchart.model';
import { Common } from 'src/app/utilities/common.static';
import { Transaction } from 'src/app/models/service-models/transaction.model';
import { BasicMonthlyBar } from 'src/app/models/service-models/basic-monthly-bar.model';

@Component({
    selector: 'app-basic-monthly-barchart-v2-component',
    templateUrl: './basic-monthly-barchart-v2.component.html',
    styleUrls: ['./basic-monthly-barchart-v2.component.scss']
})
export class BasicMonthlyBarchartV2Component implements OnInit {

    data: BasicMonthlyBarchart;

    constructor(private statisticsService: StatisticsService) {
    }

    ngOnInit(): void {
        let self = this;
        self.getBasicMonthlyBarchart(new Date(2010, 0, 1), new Date(2020, 2, 20), res => {
            self.data = res;
            self.initProperties();
        });
    }

    private initProperties() {
        
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
