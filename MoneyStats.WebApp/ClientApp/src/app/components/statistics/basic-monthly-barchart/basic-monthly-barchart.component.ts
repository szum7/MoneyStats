import { Component, Input, Output, OnInit } from '@angular/core';
import { StatisticsService } from 'src/app/services/statistics.service';
import { BasicMonthlyBarchart } from 'src/app/models/service-models/basic-monthly-barchart.model';
import { Common } from 'src/app/utilities/common.static';

@Component({
    selector: 'app-basic-monthly-barchart-component',
    templateUrl: './basic-monthly-barchart.component.html',
    styleUrls: ['./basic-monthly-barchart.component.scss']
})
export class BasicMonthlyBarchartComponent implements OnInit {

    constructor(private statisticsService: StatisticsService) {
    }

    ngOnInit(): void {
        this.getBasicMonthlyBarchart(new Date(2010, 0, 1), new Date(2020, 2, 20), res => {
            
        });
    }

    private getBasicMonthlyBarchart(from: Date, to: Date, callback: (response: BasicMonthlyBarchart) => void): void {
        let self = this;
        self.statisticsService.getBasicMonthlyBarchart(from, to).subscribe(res => {
            Common.ConsoleResponse("getBasicMonthlyBarchart", res);
            callback(res);
        }, err => {
            console.log(err);
        });
    }
}
